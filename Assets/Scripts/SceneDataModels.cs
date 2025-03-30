using System;
using UnityEngine;

[Serializable]
public class ObjectTransformData
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;
}

[Serializable]
public class SceneData
{
    public ObjectTransformData[] objects;
}
