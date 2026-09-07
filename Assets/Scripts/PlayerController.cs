using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float acceleration = 15f;
    [SerializeField] float deceleration = 20f;

    Vector2 moveInput;
    Rigidbody body;


    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    void FixedUpdate()
    {
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 targetVelocity = Vector3.ClampMagnitude(direction, 1f) * moveSpeed;
        Vector3 horizontalVelocity = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z);
        float rate = moveInput.sqrMagnitude > 0f ? acceleration : deceleration;

        horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, targetVelocity, rate * Time.fixedDeltaTime);
        body.linearVelocity = new Vector3(horizontalVelocity.x, body.linearVelocity.y, horizontalVelocity.z);
    }
}
