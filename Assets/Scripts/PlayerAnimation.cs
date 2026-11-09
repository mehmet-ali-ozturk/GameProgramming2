using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] float airborneAnimationDelay = 0.3f;

    Animator animator;
    float airborneTimer;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetFloat(
            "Speed",
            playerController.MovementSpeed,
            0.1f,
            Time.deltaTime
        );

        if (playerController.IsJumping)
        {
            airborneTimer = airborneAnimationDelay;
            animator.SetBool("IsGrounded", false);
        }
        else if (playerController.IsGrounded)
        {
            airborneTimer = 0f;
            animator.SetBool("IsGrounded", true);
        }
        else
        {
            airborneTimer += Time.deltaTime;

            if (airborneTimer >= airborneAnimationDelay)
            {
                animator.SetBool("IsGrounded", false);
            }
        }
    }
}
