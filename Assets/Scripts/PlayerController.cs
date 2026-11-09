using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(PlayerAudio))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float aimMoveSpeed = 3.5f;
    [SerializeField] float rotationSpeed = 720f;
    [SerializeField] float sprintStaminaPerSecond = 20f;
    [SerializeField] float acceleration = 15f;
    [SerializeField] float deceleration = 20f;
    [SerializeField] float airAcceleration = 5f;
    [SerializeField] float jumpSpeed = 6f;
    [SerializeField] float jumpStaminaCost = 20f;
    [SerializeField] float fallGravityMultiplier = 2.5f;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] float groundCheckDistance = 0.1f;
    [SerializeField] float maxGroundAngle = 60f;
    [SerializeField] LayerMask groundMask;

    [SerializeField] Transform cameraTransform;

    Vector2 moveInput;
    Vector3 movementVelocity;
    Rigidbody body;
    PlayerManager playerManager;
    PlayerAudio playerAudio;
    bool isGrounded;
    bool wasGrounded;
    bool jumpRequested;
    bool isSprinting;
    bool isAiming;

    public float MovementSpeed => movementVelocity.magnitude;
    public bool IsGrounded => isGrounded;
    public bool IsJumping { get; private set; }

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        playerAudio = GetComponent<PlayerAudio>();
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void RequestJump()
    {
        jumpRequested = true;
    }

    public void SetSprinting(bool sprinting)
    {
        isSprinting = sprinting;
    }

    public void SetAiming(bool aiming)
    {
        isAiming = aiming;
    }

    public void ResetMovement()
    {
        moveInput = Vector2.zero;
        movementVelocity = Vector3.zero;
        jumpRequested = false;
        isSprinting = false;
        IsJumping = false;
    }

    void FixedUpdate()
    {
        Vector3 groundCheckOrigin = groundCheck.position + Vector3.up * (groundCheckRadius + groundCheckDistance);

        bool groundFound = Physics.SphereCast(
            groundCheckOrigin,
            groundCheckRadius,
            Vector3.down,
            out RaycastHit groundHit,
            groundCheckDistance * 2f,
            groundMask,
            QueryTriggerInteraction.Ignore
        );

        isGrounded = groundFound && Vector3.Angle(groundHit.normal, Vector3.up) <= maxGroundAngle;
        bool wasGroundedLastStep = wasGrounded;

        if (isGrounded && body.linearVelocity.y <= 0f)
        {
            IsJumping = false;
        }

        wasGrounded = isGrounded;

        Vector3 platformVelocity = Vector3.zero;

        if (isGrounded && groundHit.collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            IMovingSurface platform = groundHit.collider.GetComponentInParent<IMovingSurface>();

            if (platform != null)
            {
                platformVelocity = platform.Velocity;
            }
        }

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction = cameraForward * moveInput.y + cameraRight * moveInput.x;
        bool sprinting = isSprinting && !isAiming && isGrounded && moveInput.sqrMagnitude > 0.01f;

        if (sprinting)
        {
            sprinting = playerManager.UseStamina(sprintStaminaPerSecond * Time.fixedDeltaTime);
        }

        Quaternion targetRotation = body.rotation;

        if (isAiming)
        {
            targetRotation = Quaternion.LookRotation(cameraForward, Vector3.up);
        }
        else if (direction.sqrMagnitude > 0.01f)
        {
            targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        body.MoveRotation(Quaternion.RotateTowards(
            body.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        ));

        float currentSpeed = isAiming ? aimMoveSpeed : sprinting ? sprintSpeed : moveSpeed;
        Vector3 targetVelocity = Vector3.ClampMagnitude(direction, 1f) * currentSpeed;
        float rate = moveInput.sqrMagnitude > 0f ? acceleration : deceleration;

        if (isGrounded)
        {
            movementVelocity = Vector3.MoveTowards(
                movementVelocity,
                targetVelocity,
                rate * Time.fixedDeltaTime
            );
        }
        else if (moveInput.sqrMagnitude > 0.01f)
        {
            float airSpeed = Mathf.Max(movementVelocity.magnitude, currentSpeed);
            Vector3 airTargetVelocity = Vector3.ClampMagnitude(direction, 1f) * airSpeed;

            movementVelocity = Vector3.MoveTowards(
                movementVelocity,
                airTargetVelocity,
                airAcceleration * Time.fixedDeltaTime
            );
        }

        Vector3 finalVelocity = movementVelocity + platformVelocity;
        body.linearVelocity = new Vector3(finalVelocity.x, body.linearVelocity.y, finalVelocity.z);

        if (jumpRequested && isGrounded && playerManager.UseStamina(jumpStaminaCost))
        {
            movementVelocity += new Vector3(platformVelocity.x, 0f, platformVelocity.z);

            Vector3 velocity = body.linearVelocity;
            velocity.x = movementVelocity.x;
            velocity.y = jumpSpeed;
            velocity.z = movementVelocity.z;
            body.linearVelocity = velocity;
            IsJumping = true;
        }

        if (!isGrounded && body.linearVelocity.y < 0f)
        {
            body.AddForce(
                Physics.gravity * (fallGravityMultiplier - 1f),
                ForceMode.Acceleration
            );
        }

        if (!IsJumping && (isGrounded || wasGroundedLastStep) && body.linearVelocity.y > 0f)
        {
            Vector3 velocity = body.linearVelocity;
            velocity.y = 0f;
            body.linearVelocity = velocity;
        }

        bool isWalking = isGrounded
            && !IsJumping
            && movementVelocity.sqrMagnitude > 0.1f;

        playerAudio.UpdateFootsteps(isWalking, sprinting);

        jumpRequested = false;
    }
}
