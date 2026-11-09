using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Elevator : MonoBehaviour, IMovingSurface
{
    [SerializeField] Transform locationA;
    [SerializeField] Transform locationB;
    [SerializeField] float speed = 2f;
    [SerializeField] AudioSource interactionAudioSource;
    [SerializeField] AudioSource movementAudioSource;
    [SerializeField] AudioClip interactionSound;
    [SerializeField] AudioClip movementSound;

    public bool IsMoving { get; private set; }
    public Vector3 Velocity { get; private set; }

    Rigidbody body;
    Transform targetLocation;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.isKinematic = true;
        body.useGravity = false;

        float distanceToA = Vector3.SqrMagnitude(body.position - locationA.position);
        float distanceToB = Vector3.SqrMagnitude(body.position - locationB.position);
        targetLocation = distanceToA <= distanceToB ? locationB : locationA;
    }

    public void Activate()
    {
        if (IsMoving)
        {
            return;
        }

        IsMoving = true;

        if (interactionSound != null)
        {
            interactionAudioSource.PlayOneShot(interactionSound);
        }

        if (movementSound != null)
        {
            movementAudioSource.clip = movementSound;
            movementAudioSource.loop = true;
            movementAudioSource.Play();
        }
    }

    void FixedUpdate()
    {
        if (!IsMoving)
        {
            Velocity = Vector3.zero;
            return;
        }

        Vector3 nextPosition = Vector3.MoveTowards(
            body.position,
            targetLocation.position,
            speed * Time.fixedDeltaTime
        );

        Velocity = (nextPosition - body.position) / Time.fixedDeltaTime;
        body.MovePosition(nextPosition);

        if ((nextPosition - targetLocation.position).sqrMagnitude < 0.0001f)
        {
            IsMoving = false;
            Velocity = Vector3.zero;
            movementAudioSource.Stop();
            targetLocation = targetLocation == locationA ? locationB : locationA;
        }
    }
}
