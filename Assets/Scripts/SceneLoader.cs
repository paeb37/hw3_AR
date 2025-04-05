using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

// Add SceneObjectTag class definition
public class SceneObjectTag : MonoBehaviour
{
    public string originalName;
}

public class SceneLoader : MonoBehaviour
{
    [Header("Scene JSON File (from StreamingAssets)")]
    public string jsonFileName = "scene_data.json";

    [Header("Parent for All Loaded Objects")]
    public Transform sceneRoot;

    private string FullPath => Path.Combine(Application.streamingAssetsPath, jsonFileName);

    [System.Serializable]
    public class ObjectTransformData
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public Vector3 position;
        [SerializeField]
        public Vector3 rotation;
        [SerializeField]
        public Vector3 scale;
    }

    [System.Serializable]
    public class SceneData
    {
        [SerializeField]
        public List<ObjectTransformData> objects;  // Changed from array to List
    }

    public void LoadScene()
    {
        if (!File.Exists(FullPath))
        {
            Debug.LogError("❌ Scene JSON file not found: " + FullPath);
            return;
        }

        string json = File.ReadAllText(FullPath);
        SceneData sceneData = JsonUtility.FromJson<SceneData>(json);

        if (sceneData == null || sceneData.objects == null)
        {
            Debug.LogError("❌ Invalid scene JSON format.");
            return;
        }

        int loadedCount = 0;
        Dictionary<string, GameObject> roomParents = new Dictionary<string, GameObject>();

        // First pass: instantiate floor objects first
        foreach (var objData in sceneData.objects)
        {
            string objName = objData.name.Trim();

            // Extract room info using regex
            Match roomMatch = Regex.Match(objName, @"\(Room\d+\)");
            string roomLabel = roomMatch.Success ? roomMatch.Value : null;

            // Extract base name
            string baseName = objName.Split('(')[0].Trim();

            // Skip non-floor objects
            if (roomLabel == null || !objName.Contains(roomLabel))
            {
                continue;
            }

            // Load floor objects first
            if (roomLabel != null)
            {
                string cleanName = Regex.Replace(objName, @"\(Room\d+\)", "").Trim();
                GameObject prefab = Resources.Load<GameObject>("Prefabs/" + cleanName);
                if (prefab == null)
                {
                    Debug.LogWarning($"⚠️ Prefab not found: {cleanName}");
                    continue;
                }
                GameObject roomParent = Instantiate(prefab, objData.position, Quaternion.Euler(objData.rotation), sceneRoot);
                roomParent.name = baseName + " " + roomLabel;
                roomParents[roomLabel] = roomParent;
                roomParent.AddComponent<SceneObjectTag>().originalName = cleanName;
                Debug.Log($"🏠 Created room: {roomParent.name}");
            }
        }

        // Second pass: instantiate all other objects under their respective floors
        foreach (var objData in sceneData.objects)
        {
            string objName = objData.name.Trim();
            Match roomMatch = Regex.Match(objName, @"\(Room\d+\)");
            string roomLabel = roomMatch.Success ? roomMatch.Value : null;
            string baseName = objName.Split('(')[0].Trim();

            // Skip floor objects in second pass
            if (roomLabel != null && baseName.StartsWith("Floor"))
                continue;

            string cleanName = Regex.Replace(objName, @"\(Room\d+\)", "").Trim();
            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + cleanName);
            if (prefab == null)
            {
                Debug.LogWarning($"⚠️ Prefab not found: {cleanName}");
                continue;
            }

            Transform parent = roomLabel != null && roomParents.ContainsKey(roomLabel) ?
                roomParents[roomLabel].transform : sceneRoot;

            // Create the instance and set its parent
            GameObject instance = Instantiate(prefab);
            instance.transform.parent = parent;

            // Set position based on whether it's a floor object or child
            if (roomLabel != null && baseName.StartsWith("Floor"))
            {
                // For floor objects, set world position
                instance.transform.position = objData.position;
            }
            else
            {
                // For children, set local position
                instance.transform.localPosition = objData.position;
            }

            // Set rotation from JSON
            instance.transform.rotation = Quaternion.Euler(objData.rotation);
            // Scale is inherited from the prefab, no need to set it

            instance.AddComponent<SceneObjectTag>().originalName = cleanName;
            Debug.Log($"✅ Loaded prefab: {cleanName} in {roomLabel} at {objData.position}");
            loadedCount++;
        }

        Debug.Log($"✅ Scene load complete. Total objects loaded: {loadedCount}");
    }

    void Start()
    {
        Debug.Log("🟡 SceneLoader Start() called");
        LoadScene();
    }
}


