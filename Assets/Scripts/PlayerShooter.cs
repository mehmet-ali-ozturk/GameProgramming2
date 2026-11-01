using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] InputActionReference shootAction;
    [SerializeField] CameraManager cameraManager;
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform firePoint;
    [SerializeField] Rigidbody projectilePrefab;
    [SerializeField] float projectileSpeed = 25f;
    [SerializeField] float aimDistance = 200f;
    [SerializeField] LayerMask aimMask;

    Collider[] playerColliders;

    void Awake()
    {
        playerColliders = GetComponentsInChildren<Collider>();
    }

    void OnEnable()
    {
        shootAction.action.performed += OnShoot;
        shootAction.action.Enable();
    }

    void OnDisable()
    {
        shootAction.action.performed -= OnShoot;
        shootAction.action.Disable();
    }

    void OnShoot(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f || !cameraManager.IsAiming)
        {
            return;
        }

        Ray aimRay = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 targetPoint = aimRay.GetPoint(aimDistance);

        if (Physics.Raycast(
            aimRay,
            out RaycastHit hit,
            aimDistance,
            aimMask,
            QueryTriggerInteraction.Ignore
        ))
        {
            targetPoint = hit.point;
        }

        Vector3 direction = (targetPoint - firePoint.position).normalized;
        Rigidbody projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        projectile.GetComponent<Projectile>().IgnoreCollisions(playerColliders);
        projectile.linearVelocity = direction * projectileSpeed;
    }
}
