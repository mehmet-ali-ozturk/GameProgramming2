using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(PlayerAudio))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float sprintStaminaPerSecond = 20f;
    [SerializeField] float acceleration = 15f;
    [SerializeField] float deceleration = 20f;
    [SerializeField] float jumpSpeed = 6f;
    [SerializeField] float jumpStaminaCost = 20f;

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
    bool groundStateInitialized;
    bool jumpRequested;
    bool isSprinting;

    public float MovementSpeed => movementVelocity.magnitude;
    public bool IsGrounded => isGrounded;

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

    public void ResetMovement()
    {
        moveInput = Vector2.zero;
        movementVelocity = Vector3.zero;
        jumpRequested = false;
        isSprinting = false;
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

        if (groundStateInitialized && isGrounded && !wasGrounded)
        {
            playerAudio.PlayLanding();
        }

        wasGrounded = isGrounded;
        groundStateInitialized = true;

        Vector3 platformVelocity = Vector3.zero;

        if (isGrounded && groundHit.collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            MovingPlatform platform = groundHit.collider.GetComponent<MovingPlatform>();

            if (platform != null)
            {
                platformVelocity = platform.Velocity;
            }
        }

        Quaternion cameraRotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
        body.MoveRotation(cameraRotation);

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction = cameraForward * moveInput.y + cameraRight * moveInput.x;
        bool sprinting = isSprinting && isGrounded && moveInput.sqrMagnitude > 0.01f;

        if (sprinting)
        {
            sprinting = playerManager.UseStamina(sprintStaminaPerSecond * Time.fixedDeltaTime);
        }

        float currentSpeed = sprinting ? sprintSpeed : moveSpeed;
        Vector3 targetVelocity = Vector3.ClampMagnitude(direction, 1f) * currentSpeed;
        float rate = moveInput.sqrMagnitude > 0f ? acceleration : deceleration;

        movementVelocity = Vector3.MoveTowards(movementVelocity, targetVelocity, rate * Time.fixedDeltaTime);
        Vector3 finalVelocity = movementVelocity + platformVelocity;
        body.linearVelocity = new Vector3(finalVelocity.x, body.linearVelocity.y, finalVelocity.z);

        if (jumpRequested && isGrounded && playerManager.UseStamina(jumpStaminaCost))
        {
            Vector3 velocity = body.linearVelocity;
            velocity.y = jumpSpeed;
            body.linearVelocity = velocity;
            playerAudio.PlayJumping();
        }
        jumpRequested = false;
    }
}
