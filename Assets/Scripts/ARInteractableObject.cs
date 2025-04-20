using System.Collections.Generic;
using UnityEngine;

public abstract class ARInteractableObject : MonoBehaviour
{
    List<ARInteractableObject> interactbles = new();
    protected enum State
    {
        IDLE,
        ACTIVE,
    }
    protected State ARObjectState = State.IDLE;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ARInteractableObject>(out var interactable))
        {
            AddInteractable(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ARInteractableObject>(out var interactable))
        {
            RemoveInteractable(interactable);
        }
    }

    private void AddInteractable(ARInteractableObject interactable)
    {
        interactbles.Add(interactable);
        SetState(State.ACTIVE);
    }

    protected virtual void SetState(State state)
    {
        ARObjectState = state;
    }

    private void RemoveInteractable(ARInteractableObject interactable)
    {
        interactbles.Remove(interactable);
        if (interactbles.Count == 0) SetState(State.IDLE);
        {
            
        }
    }

    private void OnDisable()
    {
        foreach (var interactable in interactbles)
        {
            interactable.RemoveInteractable(this);
        }
        interactbles.Clear();
        SetState(State.IDLE);
    }
}
