using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SceneExporter : MonoBehaviour
{
    public string outputFileName = "scene_data.json";

    public void ExportSceneData()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<ObjectTransformData> objectList = new List<ObjectTransformData>();

        // First, create a dictionary to map floors to their types
        Dictionary<GameObject, string> floorTypes = new Dictionary<GameObject, string>();
        foreach (GameObject obj in allObjects)
        {
            // this is how we detect if it is a floor or not
            if (obj.name.StartsWith("Floor"))
            {
                // Extract the floor number from the name (e.g., "Floor1" -> "1")
                string floorNumber = obj.name.Substring(5);
                floorTypes[obj] = "Room" + floorNumber;
            }
        }

        foreach (GameObject obj in allObjects)
        {
            // Skip objects not in the hierarchy or with HideFlags
            if (!obj.activeInHierarchy || obj.hideFlags != HideFlags.None)
                continue;

            // Find which floor this object belongs to
            string exportName = obj.name;
            Transform parent = obj.transform.parent;
            while (parent != null)
            {
                if (floorTypes.TryGetValue(parent.gameObject, out string roomType))
                {
                    exportName = $"{obj.name} ({roomType})";
                    break;
                }
                parent = parent.parent;
            }

            ObjectTransformData data = new ObjectTransformData
            {
                name = exportName,
                position = obj.transform.position,
                rotation = obj.transform.rotation.eulerAngles,
                scale = obj.transform.localScale
            };
            objectList.Add(data);
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