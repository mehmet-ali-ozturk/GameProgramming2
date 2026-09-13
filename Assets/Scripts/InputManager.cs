using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference jumpAction;
    [SerializeField] PlayerController playerController;

    void OnEnable()
    {
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMove;
        jumpAction.action.performed += OnJump;

        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled -= OnMove;
        jumpAction.action.performed -= OnJump;

        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        playerController.SetMoveInput(context.ReadValue<Vector2>());
    }

    void OnJump(InputAction.CallbackContext context)
    {
        playerController.RequestJump();
    }
}
