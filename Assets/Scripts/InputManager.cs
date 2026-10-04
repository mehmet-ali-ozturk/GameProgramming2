using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference jumpAction;
    [SerializeField] InputActionReference sprintAction;
    [SerializeField] PlayerController playerController;

    void OnEnable()
    {
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMove;
        jumpAction.action.performed += OnJump;
        sprintAction.action.performed += OnSprint;
        sprintAction.action.canceled += OnSprint;

        moveAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled -= OnMove;
        jumpAction.action.performed -= OnJump;
        sprintAction.action.performed -= OnSprint;
        sprintAction.action.canceled -= OnSprint;

        moveAction.action.Disable();
        jumpAction.action.Disable();
        sprintAction.action.Disable();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        playerController.SetMoveInput(context.ReadValue<Vector2>());
    }

    void OnJump(InputAction.CallbackContext context)
    {
        playerController.RequestJump();
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        playerController.SetSprinting(context.ReadValueAsButton());
    }
}
