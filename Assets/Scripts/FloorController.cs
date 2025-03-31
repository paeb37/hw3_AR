using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public int floorNumber; // 1, 2, or 3

    // Start is called before the first frame update
    void Start()
    {
        floorNumber = ++GameData.numFloors;

        // Set the name of the parent GameObject
        transform.parent.gameObject.name = $"Floor{floorNumber}";

        Debug.Log($"Floor {floorNumber} initialized");
    }

    // Handle collision when another object enters this floor's collider
    private void OnTriggerEnter(Collider other)
    {
        // Debug statement to verify trigger detection
        Debug.Log($"Trigger detected between Floor {floorNumber} and {other.gameObject.name}");
        
        // Check if the triggering object is also a floor
        if (other.gameObject.CompareTag("Floor"))
        {   
            Debug.Log("Triggered!!");

            // Destroy both parent Floor objects
            Destroy(other.gameObject.transform.parent.gameObject);
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Debug.Log($"Object {other.gameObject.name} was spawned on Floor {floorNumber}!");
            // Add your custom logic here for when objects are spawned on the floor
            // For example:
            // - Track the object
            // - Trigger events
            // - Apply effects
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
