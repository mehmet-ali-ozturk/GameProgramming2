using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;

public class TestInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] string interactionPrompt = "Rotate Cube";
    Quaternion targetRotation = Quaternion.Euler(90f, 0f, 0f);

    public Transform InteractionTransform => transform;
    public string InteractionPrompt => interactionPrompt;

    public void Interact(PlayerManager player)
    {
        Debug.Log("Object interacted");

        if (DOTween.IsTweening(transform)) return;

        transform.DORotate(new Vector3(45f, 0f, 0f), 0.3f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

                if (angleDifference < 0.5f)
                {
                    Debug.Log("90d");
                }
            });
    }
}
