using UnityEngine;

public interface IInteractable
{
    Transform InteractionTransform { get; }
    string InteractionPrompt { get; }
    bool CanInteract { get; }
    void Interact(PlayerManager player);
}
