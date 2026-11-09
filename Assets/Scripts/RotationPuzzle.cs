using System.Collections.Generic;
using UnityEngine;

public class RotationPuzzle : MonoBehaviour
{
    [SerializeField] int requiredPoweredObjects = 3;
    [SerializeField] GameObject door;

    readonly HashSet<TestInteractable> interactables = new();

    public bool IsSolved { get; private set; }

    public void Register(TestInteractable interactable)
    {
        interactables.Add(interactable);
        CheckPuzzle();
    }

    public void CheckPuzzle()
    {
        bool allPowered = interactables.Count >= requiredPoweredObjects;

        foreach (TestInteractable interactable in interactables)
        {
            if (interactable == null || !interactable.IsPowered)
            {
                allPowered = false;
                break;
            }
        }

        if (allPowered && !IsSolved)
        {
            IsSolved = true;
            Debug.Log("Rotation puzzle solved");

            if (door != null)
            {
                Destroy(door);
            }

        }
        else if (!allPowered)
        {
            IsSolved = false;
        }
    }
}
