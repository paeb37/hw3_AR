using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // should hit the floor
    private void OnTriggerEnter(Collider other)
    {
        // Debug statement to verify trigger detection
        Debug.Log($"Trigger detected between {gameObject.name} and {other.gameObject.name}");
        
        // Check if the triggering object is a floor
        if (other.gameObject.CompareTag("Floor"))
        {
            Debug.Log("Object landed on floor - keeping object");

            // Get the floor number from the FloorController component on the Visuals child
            FloorController floorController = other.gameObject.GetComponent<FloorController>();
            if (floorController != null)
            {
                Debug.Log($"Object landed on Floor {floorController.floorNumber}");
            }

            // now group with the floor it is intersecting with
            
            // make this object a child of the main floor object (parent of the Visuals child)
            GameObject mainFloorObject = other.transform.parent.gameObject;
            GameObject objectToParent = transform.parent.gameObject;
            
            Debug.Log($"Before parenting - Object: {objectToParent.name}, Current parent: {objectToParent.transform.parent?.name ?? "none"}");
            Debug.Log($"Attempting to parent to: {mainFloorObject.name}");
            
            // Store original transform values
            // Vector3 originalPosition = objectToParent.transform.position;
            // Quaternion originalRotation = objectToParent.transform.rotation;
            
            // Parent the object to the floor
            objectToParent.transform.SetParent(mainFloorObject.transform, true);
            
            Debug.Log($"After parenting - Object: {objectToParent.name}, New parent: {objectToParent.transform.parent?.name ?? "none"}");
            Debug.Log($"Object position: {objectToParent.transform.position}, Parent position: {mainFloorObject.transform.position}");
            
            // objectToParent.transform.position = originalPosition;
            // objectToParent.transform.rotation = originalRotation;
            
            Debug.Log($"Parented {objectToParent.name} to {mainFloorObject.name}");
        }
        else // i.e. colliding with another object, NOT the plane
        {   
            if (!other.gameObject.CompareTag("ARPlane"))
            {
                Debug.Log($"Object {transform.parent.gameObject.name} collided with {other.gameObject.name} - destroying");
                Destroy(transform.parent.gameObject);  // Destroy the parent instead of this object
            }
        }
    }

    // Add a new method to check when trigger exits
    private void OnTriggerExit(Collider other)
    {
        // Only check for floor collisions, ignore everything else
        if (other.gameObject.CompareTag("Floor"))
        {
            Debug.Log($"Object {transform.parent.gameObject.name} left floor - destroying");
            Destroy(transform.parent.gameObject);
        }
    }

    



    // Update is called once per frame
    void Update()
    {
        
    }
}
