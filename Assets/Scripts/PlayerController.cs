using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float acceleration = 15f;
    [SerializeField] float deceleration = 20f;
    [SerializeField] float jumpSpeed = 6f;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundMask;

    [SerializeField] Transform cameraTransform;

    Vector2 moveInput;
    Rigidbody body;
    bool isGrounded;
    bool jumpRequested;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void RequestJump()
    {
        jumpRequested = true;
    }

    void FixedUpdate()
    {
        //QueryTriggerInteraction.Ignore = Something about the trigger colliders? I don't know what this does exactly 
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction = cameraForward * moveInput.y + cameraRight * moveInput.x;
        Vector3 targetVelocity = Vector3.ClampMagnitude(direction, 1f) * moveSpeed;
        Vector3 horizontalVelocity = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z);
        float rate = moveInput.sqrMagnitude > 0f ? acceleration : deceleration;

        horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, targetVelocity, rate * Time.fixedDeltaTime);
        body.linearVelocity = new Vector3(horizontalVelocity.x, body.linearVelocity.y, horizontalVelocity.z);

        if (jumpRequested && isGrounded)
        {
            Vector3 velocity = body.linearVelocity;
            velocity.y = jumpSpeed;
            body.linearVelocity = velocity;
        }
        jumpRequested = false;
    }
}
