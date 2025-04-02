using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FloorController : MonoBehaviour
{
    public int floorNumber; // 1, 2, or 3
    [SerializeField] private XRGrabInteractable grabInteractable; // Assign in Inspector
    private Material originalMaterial;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        floorNumber = ++GameData.numFloors;
        transform.parent.gameObject.name = $"Floor{floorNumber}";

        // Store the original material and color
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.sharedMaterial;
            originalColor = originalMaterial.GetColor("_BaseColor");
            Debug.Log($"Stored original color for Floor {floorNumber}");
        }

        if (grabInteractable == null)
        {
            Debug.LogError($"Please assign XRGrabInteractable in Inspector for Floor {floorNumber}");
        }
    }

    // Handle collision when another object enters this floor's collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {   
            Renderer thisRenderer = GetComponent<Renderer>();
            if (thisRenderer != null)
            {
                Debug.Log($"Setting Floor {floorNumber} color to red");
                thisRenderer.sharedMaterial.SetColor("_BaseColor", Color.red);
            }
        }
        else
        {
            if (grabInteractable != null)
            {
                grabInteractable.trackScale = false;
            }
        }
    }

    // Add OnTriggerExit to reset material when floors stop overlapping
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            Renderer thisRenderer = GetComponent<Renderer>();
            if (thisRenderer != null)
            {
                Debug.Log($"Resetting Floor {floorNumber} to original color");
                thisRenderer.sharedMaterial.SetColor("_BaseColor", originalColor);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
