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

        foreach (GameObject obj in allObjects)
        {
            // Skip objects not in the hierarchy or with HideFlags
            if (!obj.activeInHierarchy || obj.hideFlags != HideFlags.None)
                continue;

            ObjectTransformData data = new ObjectTransformData
            {
                name = obj.name,
                position = obj.transform.position,
                rotation = obj.transform.rotation
            };

            objectList.Add(data);
        }

        SceneData sceneData = new SceneData
        {
            objects = objectList.ToArray()
        };

        string json = JsonUtility.ToJson(sceneData, true);
        string filePath = Path.Combine(Application.persistentDataPath, outputFileName);

        File.WriteAllText(filePath, json);
        Debug.Log("Scene exported to: " + filePath);
    }
}