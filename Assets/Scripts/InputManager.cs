using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionReference moveAction;
    [SerializeField] PlayerController playerController;

    void OnEnable()
    {
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMove;
        moveAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled -= OnMove;
        moveAction.action.Disable();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        playerController.SetMoveInput(context.ReadValue<Vector2>());
    }
}
