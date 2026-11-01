using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTargetController : MonoBehaviour
{
    [SerializeField] InputActionReference lookAction;
    [SerializeField] float mouseSensitivity = 0.1f;
    [SerializeField] float controllerSensitivity = 120f;
    [SerializeField] float minimumPitch = -30f;
    [SerializeField] float maximumPitch = 70f;

    float yaw;
    float pitch;

    void Awake()
    {
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        if (pitch > 180f)
        {
            pitch -= 360f;
        }
    }

    void OnEnable()
    {
        lookAction.action.Enable();
    }

    void OnDisable()
    {
        lookAction.action.Disable();
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();
        bool usingMouse = lookAction.action.activeControl?.device is Mouse;
        float sensitivity = usingMouse
            ? mouseSensitivity
            : controllerSensitivity * Time.deltaTime;

        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minimumPitch, maximumPitch);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
