using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    Animator animator;

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

        animator.SetBool("IsGrounded", playerController.IsGrounded);
    }
}
