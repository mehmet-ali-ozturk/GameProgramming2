using UnityEngine;

public class ElevatorButton : MonoBehaviour, IInteractable
{
    [SerializeField] Elevator elevator;
    [SerializeField] string interactionPrompt = "Use Elevator";

    public Transform InteractionTransform => transform;
    public string InteractionPrompt => interactionPrompt;
    public bool CanInteract => !elevator.IsMoving;

    public void Interact(PlayerManager player)
    {
        elevator.Activate();
    }
}
