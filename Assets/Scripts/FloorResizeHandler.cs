using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FloorResizeHandler : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private bool hasObjectsOnTop = false;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            // Subscribe to the select events to check for objects on top
            grabInteractable.selectEntered.AddListener(OnSelectEntered);
            grabInteractable.selectExited.AddListener(OnSelectExited);
        }
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Check if there are any objects on top of this floor
        CheckForObjectsOnTop();
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        // Reset the flag when the floor is released
        hasObjectsOnTop = false;
    }

    void CheckForObjectsOnTop()
    {
        // Cast a ray upward from the floor's position
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.up);
        RaycastHit[] hits = Physics.RaycastAll(ray, 2f); // Check up to 2 units above the floor

        foreach (RaycastHit hit in hits)
        {
            // Skip if the hit object is this floor or its children
            if (hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform))
                continue;

            // If we find any other object, mark this floor as having objects on top
            hasObjectsOnTop = true;
            break;
        }
    }

    void Update()
    {
        if (hasObjectsOnTop && grabInteractable != null)
        {
            // If there are objects on top, prevent scaling
            grabInteractable.trackScale = false;
        }
        else if (grabInteractable != null)
        {
            // Otherwise, allow scaling
            grabInteractable.trackScale = true;
        }
    }
} 