using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GrabInteractableTracker : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private ActionTracker actionTracker;

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