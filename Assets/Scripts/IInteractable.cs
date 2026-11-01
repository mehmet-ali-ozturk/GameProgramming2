using UnityEngine;

public interface IInteractable
{
    Transform InteractionTransform { get; }
    string InteractionPrompt { get; }
    void Interact(PlayerManager player);
}
