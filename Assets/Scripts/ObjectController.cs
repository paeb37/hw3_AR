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
        // Debug.Log($"Trigger detected between {gameObject.name} and {other.gameObject.name}");
        
        // Check if the triggering object is a floor
        if (other.gameObject.CompareTag("Floor"))
        {
            Debug.Log("Object landed on floor - keeping object");
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
