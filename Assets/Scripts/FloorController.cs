using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TMPro;

public class FloorController : MonoBehaviour
{
    public int floorNumber; // 1, 2, or 3
    [SerializeField] private XRGrabInteractable grabInteractable; // Assign in Inspector
    [SerializeField] private TextMeshProUGUI textPrefab; // Reference to text prefab
    private TextMeshProUGUI collisionText; // Instance of text for this floor

    // Start is called before the first frame update
    void Start()
    {
        floorNumber = ++GameData.numFloors;
        transform.parent.gameObject.name = $"Floor{floorNumber}";

        if (grabInteractable == null)
        {
            Debug.LogError($"Please assign XRGrabInteractable in Inspector for Floor {floorNumber}");
        }

        if (textPrefab == null)
        {
            Debug.LogError($"Please assign TextMeshProUGUI prefab in Inspector for Floor {floorNumber}");
        }
        else
        {
            // Find the UI Canvas
            GameObject uiCanvas = GameObject.Find("UI");
            if (uiCanvas != null)
            {
                collisionText = Instantiate(textPrefab, uiCanvas.transform);
                collisionText.gameObject.SetActive(false);
                Debug.Log($"Created text instance for Floor {floorNumber}");
            }
            else
            {
                Debug.LogError("Could not find UI Canvas in scene! Make sure it's named 'UI'");
            }
        }
    }

    // Handle collision when another object enters this floor's collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {   
            Debug.Log($"Floor {floorNumber} collision detected with {other.gameObject.name}");
            if (collisionText != null)
            {
                collisionText.gameObject.SetActive(true);
                // collisionText.text = $"Objects can't overlap!";
                Debug.Log($"Showing text for Floor {floorNumber}");
            }
            else
            {
                Debug.LogError($"Collision text is null for Floor {floorNumber}");
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

    // Add OnTriggerExit to hide text when floors stop overlapping
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            Debug.Log($"Floor {floorNumber} collision ended with {other.gameObject.name}");
            if (collisionText != null)
            {
                collisionText.gameObject.SetActive(false);
                Debug.Log($"Hiding text for Floor {floorNumber}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}