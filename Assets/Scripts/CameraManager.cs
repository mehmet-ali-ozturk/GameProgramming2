using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] InputActionReference aimAction;
    [SerializeField] CinemachineCamera thirdPersonCamera;
    [SerializeField] CinemachineCamera aimCamera;
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject crosshair;
    [SerializeField] int normalPriority = 10;
    [SerializeField] int aimingPriority = 20;

    public bool IsAiming { get; private set; }

    void Awake()
    {
        SetAiming(false);
    }

    void OnEnable()
    {
        aimAction.action.performed += OnAimStarted;
        aimAction.action.canceled += OnAimStopped;
        aimAction.action.Enable();
    }

    void OnDisable()
    {
        aimAction.action.performed -= OnAimStarted;
        aimAction.action.canceled -= OnAimStopped;
        aimAction.action.Disable();
        SetAiming(false);
    }

    void Update()
    {
        if (crosshair != null)
        {
            crosshair.SetActive(IsAiming && Time.timeScale > 0f);
        }
    }

    void OnAimStarted(InputAction.CallbackContext context)
    {
        if (Time.timeScale > 0f)
        {
            SetAiming(true);
        }
    }

    void OnAimStopped(InputAction.CallbackContext context)
    {
        SetAiming(false);
    }

    void SetAiming(bool aiming)
    {
        IsAiming = aiming;
        playerController.SetAiming(aiming);

        thirdPersonCamera.Priority = normalPriority;
        aimCamera.Priority = aiming ? aimingPriority : 0;
    }
}
