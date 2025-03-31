using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ObjectSelector : MonoBehaviour
{

    public bool IsInteracting => interactor != null && interactor.hasSelection;

    [SerializeField] private XRGrabInteractable targetObject; // Reference to the mainTransform's XRGrabInteractable
    [SerializeField] private XRRayInteractor interactor;   // Reference to an interactor that will do the selecting
    [SerializeField] private InteractionLayerMask interactionLayers;     // Layers that can be interacted with

    private void Start()
    {
        Debug.Log("ObjectSelector Start - Target Object: " + (targetObject != null) + ", Interactor: " + (interactor != null));
        if (targetObject != null)
        {
            Debug.Log("Target Object Layer: " + targetObject.gameObject.layer);
            Debug.Log("Target Object Name: " + targetObject.gameObject.name);
        }
        if (interactor != null)
        {
            Debug.Log("Interactor Interaction Layers: " + interactor.interactionLayers);
        }
        // Set the interaction layers for the interactor
        if (interactor != null)
        {
            interactor.interactionLayers = interactionLayers;
        }
    }

    public void TriggerSelection()
    {
        Debug.Log("TriggerSelection called - Target Object: " + (targetObject != null));
        if (targetObject != null)
        {
            Debug.Log("Starting manual interaction with: " + targetObject.gameObject.name);
            // Force the selection
            interactor.StartManualInteraction(targetObject as IXRSelectInteractable);
            Debug.Log("Started manual interaction");
        }
        else
        {
            Debug.LogError("Target Object is null! Please assign it in the Inspector.");
        }
    }

    public void ReleaseSelection()
    {
        Debug.Log("ReleaseSelection called");
        // End the forced selection
        interactor.EndManualInteraction();
        Debug.Log("Ended manual interaction");
    }

    public void SetScale(float scale)
    {
        if (targetObject != null)
        {
            targetObject.transform.localScale = Vector3.one * scale;
            Debug.Log("Set scale to: " + scale);
        }
    }
}