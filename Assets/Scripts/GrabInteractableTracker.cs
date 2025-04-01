using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GrabInteractableTracker : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private ActionTracker actionTracker;
    private bool isInitialized = false;
    private bool isBeingUndone = false;

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

    void Start()
    {
        // Only track spawn if this is a new object (not from undo/redo)
        if (actionTracker != null && !gameObject.GetComponent<TransformState>() && !isInitialized)
        {
            isInitialized = true;
            actionTracker.OnObjectSpawned(gameObject);
        }
    }

    void OnDisable()
    {
        // Only track despawn if this is a real despawn (not from undo/redo)
        // and if the object still exists (not null)
        if (actionTracker != null && !gameObject.GetComponent<TransformState>() && gameObject != null && isInitialized && !isBeingUndone)
        {
            // ACTUALLY DESTROYED!!!!
            actionTracker.OnObjectDespawned(gameObject);
        }
    }

    public void SetBeingUndone(bool value)
    {
        isBeingUndone = value;
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
} 