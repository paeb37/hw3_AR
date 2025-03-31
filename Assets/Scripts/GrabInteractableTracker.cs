using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GrabInteractableTracker : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private ActionTracker actionTracker;
    private bool isFirstEnable = true;
    private bool isBeingDestroyed = false;

    void Awake()
    {
        // Get the XRGrabInteractable component on this object
        grabInteractable = GetComponent<XRGrabInteractable>();
        
        // Find the ActionTracker in the scene
        actionTracker = FindObjectOfType<ActionTracker>();
        
        if (grabInteractable != null && actionTracker != null)
        {
            // Subscribe to the events
            grabInteractable.selectEntered.AddListener(OnSelectEntered);
            grabInteractable.selectExited.AddListener(OnSelectExited);
        }
    }

    void OnEnable()
    {
        // Only track spawn if this is the first time the object is enabled
        // AND it's not being re-enabled from an undo/redo operation
        if (isFirstEnable && !isBeingDestroyed && actionTracker != null)
        {
            actionTracker.OnObjectSpawned(gameObject);
            isFirstEnable = false;
        }
        else if (isBeingDestroyed)
        {
            // If we're being re-enabled from an undo operation, reset the flags
            isBeingDestroyed = false;
            isFirstEnable = false;
        }
    }

    void OnDisable()
    {
        // Only track despawn if the object is actually being destroyed
        // AND it's not being disabled from an undo/redo operation
        if (gameObject != null && !isBeingDestroyed && actionTracker != null)
        {
            actionTracker.OnObjectDespawned(gameObject);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (actionTracker != null)
        {
            actionTracker.OnGrabStart(grabInteractable);
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (actionTracker != null)
        {
            actionTracker.OnGrabEnd(grabInteractable);
        }
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            // Unsubscribe from events
            grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            grabInteractable.selectExited.RemoveListener(OnSelectExited);
        }
    }

    // Called before the object is destroyed
    public void MarkForDestruction()
    {
        isBeingDestroyed = true;
    }
} 