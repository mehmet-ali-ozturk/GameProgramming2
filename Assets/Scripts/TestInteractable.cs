using UnityEngine;
using DG.Tweening;

public class TestInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] string interactionPrompt = "Rotate Cube";
    [SerializeField] RotationPuzzle rotationPuzzle;
    [SerializeField] float correctAngle = 90f;
    [SerializeField] float rotationStep = 45f;
    [SerializeField] float rotationDuration = 0.3f;
    [SerializeField] float angleTolerance = 0.5f;
    [SerializeField] Renderer[] poweredRenderers;
    [SerializeField] Material unpoweredMaterial;
    [SerializeField] Material poweredMaterial;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip poweredSound;

    public Transform InteractionTransform => transform;
    public string InteractionPrompt => interactionPrompt;
    public bool CanInteract => !DOTween.IsTweening(transform);
    public bool IsPowered { get; private set; }

    void Start()
    {
        UpdatePoweredState(false);

        if (rotationPuzzle != null)
        {
            rotationPuzzle.Register(this);
        }
    }

    public void Interact(PlayerManager player)
    {
        if (!CanInteract)
        {
            return;
        }

        transform.DOLocalRotate(
            new Vector3(rotationStep, 0f, 0f),
            rotationDuration,
            RotateMode.LocalAxisAdd
        )
        .SetEase(Ease.InOutQuad)
        .OnComplete(() =>
        {
            UpdatePoweredState(true);

            if (rotationPuzzle != null)
            {
                rotationPuzzle.CheckPuzzle();
            }
        });
    }

    void UpdatePoweredState(bool playSound)
    {
        float angleDifference = Mathf.Abs(
            Mathf.DeltaAngle(transform.localEulerAngles.x, correctAngle)
        );

        bool wasPowered = IsPowered;
        IsPowered = angleDifference <= angleTolerance;
        Material material = IsPowered ? poweredMaterial : unpoweredMaterial;

        foreach (Renderer poweredRenderer in poweredRenderers)
        {
            if (poweredRenderer != null && material != null)
            {
                poweredRenderer.material = material;
            }
        }

        if (playSound && IsPowered && !wasPowered && audioSource != null && poweredSound != null)
        {
            audioSource.PlayOneShot(poweredSound);
        }
    }
}
