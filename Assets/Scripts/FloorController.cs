using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FloorController : MonoBehaviour
{
    public int floorNumber; // 1, 2, or 3
    [SerializeField] private XRGrabInteractable grabInteractable; // Assign in Inspector

    // Start is called before the first frame update
    void Start()
    {
        floorNumber = ++GameData.numFloors;

        // Set the name of the parent GameObject
        transform.parent.gameObject.name = $"Floor{floorNumber}";

        if (grabInteractable != null)
        {
            // Debug.Log($"XRGrabInteractable assigned for Floor {floorNumber}");
        }
        else
        {
            // Debug.LogError($"Please assign XRGrabInteractable in Inspector for Floor {floorNumber}");
        }

        // Debug.Log($"Floor {floorNumber} initialized");
    }

    // Handle collision when another object enters this floor's collider
    private void OnTriggerEnter(Collider other)
    {
        // Debug statement to verify trigger detection
        // Debug.Log($"Trigger detected between Floor {floorNumber} and {other.gameObject.name}");
        
        // Check if the triggering object is also a floor
        if (other.gameObject.CompareTag("Floor"))
        {   
            // Debug.Log("Triggered!!");

            // Destroy both parent Floor objects
            // Destroy(other.gameObject.transform.parent.gameObject);
            // Destroy(transform.parent.gameObject);
        }
        else
        {
            // Debug.Log($"Object {other.gameObject.name} was spawned on Floor {floorNumber}!");
            
            // Disable Track Scale if we have an XRGrabInteractable
            if (grabInteractable != null)
            {
                // Debug.Log($"Disabling Track Scale on Floor {floorNumber}");
                grabInteractable.trackScale = false;
            }
        }
    }

    // Check if another object is placed on here using a LAYER
    // or can check the specific tag attached to the other objects
    /**

    */



    // Update is called once per frame
    void Update()
    {
        
    }
}
