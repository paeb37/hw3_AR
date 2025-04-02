using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    // [SerializeField]
    // private GameObject mainTransform;
    
    // [SerializeField]
    private Validator validator;
    
    [SerializeField] private Material redMaterial; // Assign in Inspector
    private Material originalMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        // Find MainTransform if not assigned
        // if (mainTransform == null)
        // {
        //     mainTransform = GameObject.Find("MainTransform");
        //     if (mainTransform == null)
        //     {
        //         // Debug.LogWarning("MainTransform not found in scene. Objects will not be properly grouped.");
        //     }
        // }

        // Find the validator on the SceneExporter GameObject
        GameObject sceneExporter = GameObject.Find("Scene Exporter"); // exact name matters
        if (sceneExporter != null)
        {
            validator = sceneExporter.GetComponent<Validator>();
        }

        // Store the original material
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.material;
            Debug.Log($"Object {gameObject.name} original material: {originalMaterial.name}");
        }
        else
        {
            Debug.LogError($"No Renderer found on {gameObject.name}");
        }

        if (redMaterial == null)
        {
            Debug.LogError($"Red material not assigned in inspector for {gameObject.name}");
        }
    }

    // should hit the floor
    private void OnTriggerEnter(Collider other)
    {
        // Debug statement to verify trigger detection
        // Debug.Log($"Trigger detected between {gameObject.name} and {other.gameObject.name}");
        
        // Check if the triggering object is a floor
        if (other.gameObject.CompareTag("Floor"))
        {
            // Debug.Log("Object landed on floor - keeping object");

            // Get the floor number from the FloorController component on the Visuals child
            FloorController floorController = other.gameObject.GetComponent<FloorController>();
            if (floorController != null)
            {
                // Debug.Log($"Object landed on Floor {floorController.floorNumber}");
            }

            // now group with the floor it is intersecting with
            
            // make this object a child of the main floor object (parent of the Visuals child)
            GameObject mainFloorObject = other.transform.parent.gameObject;
            GameObject objectToParent = transform.parent.gameObject;
            
            // Debug.Log($"Before parenting - Object: {objectToParent.name}, Current parent: {objectToParent.transform.parent?.name ?? "none"}");
            // Debug.Log($"Attempting to parent to: {mainFloorObject.name}");
            
            // Store original transform values
            // Vector3 originalPosition = objectToParent.transform.position;
            // Quaternion originalRotation = objectToParent.transform.rotation;
            
            // Parent the object to the floor
            objectToParent.transform.SetParent(mainFloorObject.transform, true);
            
            // Debug.Log($"After parenting - Object: {objectToParent.name}, New parent: {objectToParent.transform.parent?.name ?? "none"}");
            // Debug.Log($"Object position: {objectToParent.transform.position}, Parent position: {mainFloorObject.transform.position}");
            
            // objectToParent.transform.position = originalPosition;
            // objectToParent.transform.rotation = originalRotation;
            
            // Debug.Log($"Parented {objectToParent.name} to {mainFloorObject.name}");

            // Validate after parenting the object
            if (validator != null)
            {
                Debug.Log("Validating after parenting object to floor");
                validator.ValidateObjects();
            }
        }
        else // i.e. colliding with another object, NOT the plane
        {   
            if (!other.gameObject.CompareTag("ARPlane"))
            {
                // Change the material of both objects to red
                Renderer thisRenderer = GetComponent<Renderer>();
                Renderer otherRenderer = other.gameObject.GetComponent<Renderer>();
                
                if (thisRenderer != null && redMaterial != null)
                {
                    Debug.Log($"Changing {gameObject.name} to red material");
                    thisRenderer.material = redMaterial;
                }
                else
                {
                    Debug.LogError($"Cannot change material - Renderer or red material missing on {gameObject.name}");
                }

                if (otherRenderer != null)
                {
                    ObjectController otherObject = other.GetComponent<ObjectController>();
                    if (otherObject != null && otherObject.redMaterial != null)
                    {
                        Debug.Log($"Changing {other.gameObject.name} to red material");
                        otherRenderer.material = otherObject.redMaterial;
                    }
                    else
                    {
                        Debug.LogError($"Cannot change material - Renderer or red material missing on {other.gameObject.name}");
                    }
                }
            }
        }
    }

    // If it leaves floor, get rid of it
    private void OnTriggerExit(Collider other)
    {
        // Only check for floor collisions, ignore everything else
        if (other.gameObject.CompareTag("Floor"))
        {
            // Debug.Log($"Object {transform.parent.gameObject.name} left floor - destroying");
            Destroy(transform.parent.gameObject);
        }
        else // leaves collision with some other object
        {
            Debug.Log($"Object {gameObject.name} leaving collision with {other.gameObject.name} - resetting to original material");
            Renderer thisRenderer = GetComponent<Renderer>();
            if (thisRenderer != null && originalMaterial != null)
            {
                thisRenderer.material = originalMaterial;
            }
            else
            {
                Debug.LogError($"Cannot reset material - Renderer or original material missing on {gameObject.name}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
