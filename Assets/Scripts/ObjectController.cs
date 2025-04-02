using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectController : MonoBehaviour
{
    // [SerializeField]
    // private GameObject mainTransform;
    
    // [SerializeField]
    private Validator validator;
    
    [SerializeField] private TextMeshProUGUI textPrefab; // Reference to text prefab
    private TextMeshProUGUI collisionText; // Instance of text for this object
    
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

        if (textPrefab == null)
        {
            Debug.LogError($"Please assign TextMeshProUGUI prefab in Inspector for {gameObject.name}");
        }
        else
        {
            // Find the UI Canvas
            GameObject uiCanvas = GameObject.Find("UI");
            if (uiCanvas != null)
            {
                collisionText = Instantiate(textPrefab, uiCanvas.transform);
                collisionText.gameObject.SetActive(false);
                Debug.Log($"Created text instance for object {gameObject.name}");
            }
            else
            {
                Debug.LogError("Could not find UI Canvas in scene! Make sure it's named 'UI'");
            }
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

            // Show collision text
            // if (collisionText != null)
            // {
            //     collisionText.gameObject.SetActive(true);
            //     Debug.Log($"Showing collision text for {gameObject.name}");
            // }
        }
        else // i.e. colliding with another object, NOT the plane
        {   
            if (!other.gameObject.CompareTag("ARPlane"))
            {
                // Show collision text
                if (collisionText != null)
                {
                    collisionText.gameObject.SetActive(true);
                    Debug.Log($"Showing collision text for {gameObject.name}");
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
            if (collisionText != null)
            {
                Destroy(collisionText.gameObject);
            }
            Destroy(transform.parent.gameObject);
        }
        else // leaves collision with some other object
        {
            // Hide collision text
            if (collisionText != null)
            {
                collisionText.gameObject.SetActive(false);
                Debug.Log($"Hiding collision text for {gameObject.name}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
