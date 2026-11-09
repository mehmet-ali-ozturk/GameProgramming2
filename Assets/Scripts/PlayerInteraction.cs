using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(SphereCollider))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] InputActionReference interactAction;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameObject promptObject;
    [SerializeField] TMP_Text promptText;

    readonly Dictionary<IInteractable, int> nearbyInteractables = new();
    IInteractable closestInteractable;

    void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        SetPromptVisible(false);
    }

    void OnEnable()
    {
        interactAction.action.performed += OnInteract;
        interactAction.action.Enable();
    }

    void OnDisable()
    {
        interactAction.action.performed -= OnInteract;
        interactAction.action.Disable();
        SetPromptVisible(false);
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            closestInteractable = null;
            SetPromptVisible(false);
            return;
        }

        closestInteractable = FindClosestInteractable();

        if (closestInteractable == null)
        {
            SetPromptVisible(false);
            return;
        }

        if (promptText != null)
        {
            promptText.text = closestInteractable.InteractionPrompt;
        }

        SetPromptVisible(true);
    }

    void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable == null)
        {
            return;
        }

        if (nearbyInteractables.ContainsKey(interactable))
        {
            nearbyInteractables[interactable]++;
        }
        else
        {
            nearbyInteractables.Add(interactable, 1);
        }
    }

    void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable == null || !nearbyInteractables.ContainsKey(interactable))
        {
            return;
        }

        nearbyInteractables[interactable]--;

        if (nearbyInteractables[interactable] <= 0)
        {
            nearbyInteractables.Remove(interactable);
        }
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        if (closestInteractable != null)
        {
            closestInteractable.Interact(playerManager);
        }
    }

    IInteractable FindClosestInteractable()
    {
        IInteractable closest = null;
        float closestDistance = float.MaxValue;
        List<IInteractable> missingInteractables = new();

        foreach (IInteractable interactable in nearbyInteractables.Keys)
        {
            if (interactable is not Component component || component == null)
            {
                missingInteractables.Add(interactable);
                continue;
            }

            if (!interactable.CanInteract)
            {
                continue;
            }

            float distance = (interactable.InteractionTransform.position - transform.position).sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = interactable;
            }
        }

        foreach (IInteractable interactable in missingInteractables)
        {
            nearbyInteractables.Remove(interactable);
        }

        return closest;
    }

    void SetPromptVisible(bool visible)
    {
        if (promptObject != null)
        {
            promptObject.SetActive(visible);
        }
    }
}
