using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SceneExporter : MonoBehaviour
{
    public string outputFileName = "";

    public void ExportSceneData()
    {
        // Find the MainTransform GameObject
        GameObject mainTransform = GameObject.Find("MainTransform");
        if (mainTransform == null)
        {
            Debug.LogError("MainTransform GameObject not found in the scene!");
            return;
        }

        List<ObjectTransformData> objectList = new List<ObjectTransformData>();
        
        // Process only the floor children of MainTransform
        foreach (Transform child in mainTransform.transform)
        {
            // Check if this child is a floor
            if (child.name.StartsWith("Floor"))
            {
                // Skip if not active in hierarchy
                if (!child.gameObject.activeInHierarchy)
                    continue;
                    
                // Add the floor itself
                string floorNumber = child.name.Substring(5); // Extract the floor number
                string roomType = "Room" + floorNumber;
                
                ObjectTransformData floorData = new ObjectTransformData
                {
                    name = $"{child.name} ({roomType})",
                    position = child.localPosition,
                    rotation = child.localRotation.eulerAngles,
                    scale = child.localScale
                };
                objectList.Add(floorData);
                
                // Add all children of the floor, except for specific objects to ignore
                foreach (Transform floorChild in child)
                {
                    // Skip if not active in hierarchy or has HideFlags
                    if (!floorChild.gameObject.activeInHierarchy || floorChild.gameObject.hideFlags != HideFlags.None)
                        continue;
                        
                    // Skip specific objects we want to ignore based on name prefixes
                    if (floorChild.name.StartsWith("Interaction Affordance") || 
                        floorChild.name.StartsWith("AttachTransform") || 
                        floorChild.name.StartsWith("Visuals"))
                        continue;
                        
                    ObjectTransformData childData = new ObjectTransformData
                    {
                        name = $"{floorChild.name} ({roomType})",
                        position = floorChild.localPosition,
                        rotation = floorChild.localRotation.eulerAngles,
                        scale = floorChild.localScale
                    };
                    objectList.Add(childData);
                }
            }
        }
        
        // Convert to JSON
        string json = JsonUtility.ToJson(new SceneData { objects = objectList }, true);

        try
        {
            // Use persistentDataPath instead of dataPath
            string path = Path.Combine(Application.persistentDataPath, outputFileName);
            File.WriteAllText(path, json);
            Debug.Log($"Scene data exported successfully to: {path}");
            
            // Optional: Show the file path in a more readable format
            #if UNITY_IOS
            string readablePath = path.Replace(Application.persistentDataPath, "~/Documents");
            Debug.Log($"File location (iOS): {readablePath}");
            #endif
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to export scene data: {e.Message}");
            throw; // Re-throw the exception if you want to handle it at a higher level
        }
    }
}

[System.Serializable]
public class SceneData
{
    public List<ObjectTransformData> objects;
}

[System.Serializable]
public class ObjectTransformData
{
    public string name;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
} 