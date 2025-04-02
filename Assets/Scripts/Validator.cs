using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

// checks if requirements have been met
// if so, then it will show the Export Button in the UI
public class Validator : MonoBehaviour
{
    [SerializeField]
    private GameObject mainTransform;
    
    [SerializeField]
    private Button exportButton;
    
    private const int REQUIRED_FLOOR_COUNT = 3;

    private void Start()
    {
        // Find MainTransform if not assigned
        if (mainTransform == null)
        {
            mainTransform = GameObject.Find("MainTransform");
            if (mainTransform == null)
            {
                Debug.LogError("MainTransform not found in scene!");
                return;
            }
        }

        // Initially hide the export button
        if (exportButton != null)
        {
            exportButton.gameObject.SetActive(false);
        }
    }

    public void ValidateObjects()
    {
        if (mainTransform == null) return;

        int floorCount = 0;
        bool allRequirementsMet = true;
        
        // Iterate through all children of MainTransform
        foreach (Transform child in mainTransform.transform)
        {
            if (child.CompareTag("Floor")) // checking number of floors
            {
                floorCount++;
                
                // check requirements for Floor 1
                if (child.name == "Floor1")
                {
                    bool hasDoor = false;
                    bool hasMonster = false;
                    
                    // Check children of Floor 1
                    foreach (Transform floorChild in child)
                    {
                        Debug.Log($"Checking child of Floor 1: {floorChild.name} with tag: {floorChild.tag}");
                        if (floorChild.CompareTag("Door"))
                        {
                            hasDoor = true;
                            Debug.Log("Found Door in Floor 1!");
                        }
                        else if (floorChild.CompareTag("Monster"))
                        {
                            hasMonster = true;
                            Debug.Log("Found Monster in Floor 1!");
                        }
                    }
                    
                    // If Floor 1 doesn't meet requirements, set allRequirementsMet to false
                    if (!hasDoor || !hasMonster)
                    {
                        allRequirementsMet = false;
                        Debug.LogWarning("Floor 1 is missing required objects: " + 
                            (!hasDoor ? "Door " : "") + 
                            (!hasMonster ? "Monster" : ""));
                    }
                }
                // check requirements for Floor 2
                else if (child.name == "Floor2")
                {
                    bool hasDoor = false;
                    int chestCount = 0;
                    int monsterCount = 0;
                    
                    // Check children of Floor 2
                    foreach (Transform floorChild in child)
                    {
                        // Debug.Log($"Checking child of Floor 2: {floorChild.name} with tag: {floorChild.tag}");
                        if (floorChild.CompareTag("Door"))
                        {
                            hasDoor = true;
                            Debug.Log("Found Door in Floor 2!");
                        }
                        else if (floorChild.CompareTag("Chest"))
                        {
                            chestCount++;
                            Debug.Log($"Found Chest in Floor 2! Total chests: {chestCount}");
                        }
                        else if (floorChild.CompareTag("Monster"))
                        {
                            monsterCount++;
                            Debug.Log($"Found Monster in Floor 2! Total monsters: {monsterCount}");
                        }
                    }
                    
                    // If Floor 2 doesn't meet requirements, set allRequirementsMet to false
                    if (!hasDoor || chestCount != 2 || monsterCount < 2)
                    {
                        allRequirementsMet = false;
                        Debug.LogWarning("Floor 2 is missing required objects: " + 
                            (!hasDoor ? "Door " : "") + 
                            (chestCount != 2 ? $"Chests (found {chestCount}, need 2) " : "") + 
                            (monsterCount < 2 ? $"Monsters (found {monsterCount}, need at least 2)" : ""));
                    }
                }
            }
        }

        // Show export button if we have exactly 3 floor objects and all requirements are met
        if (exportButton != null)
        {
            exportButton.gameObject.SetActive(floorCount == REQUIRED_FLOOR_COUNT && allRequirementsMet);
        }
    }
}