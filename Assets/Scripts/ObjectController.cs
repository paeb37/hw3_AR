using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Handle collision when another object enters this floor's collider
    // private void OnTriggerEnter(Collider other)
    // {
    //     // Debug statement to verify trigger detection
    //     Debug.Log($"Trigger detected between {gameObject.name} and {other.gameObject.name}");
        
    //     // Check if the triggering object is also a floor
    //     if (other.gameObject.CompareTag("Floor"))
    //     {
    //         // Destroy both parent Floor objects
    //         Destroy(other.gameObject.transform.parent.gameObject);
    //         Destroy(transform.parent.gameObject);
    //     }
    // }

    // Check if another object is placed on here using a LAYER
    // or can check the specific tag attached to the other objects
    /**
    
    */



    // Update is called once per frame
    void Update()
    {
        
    }
}
