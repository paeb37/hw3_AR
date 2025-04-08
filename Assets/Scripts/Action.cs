using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

// different ways to mess with objects in the scene
public enum ActionType
{
    Spawn,      // make new object
    Despawn,    // delete object
    Translate,  // move object
    Scale,      // resize object
    Rotate      // spin object
}

// one action that can be undone/redone
public class Action
{
    // what kind of action it is
    public ActionType Type { get; private set; }

    // the object that got changed
    public GameObject TargetObject { get; private set; }

    // where the object was before
    private (Vector3 position, Quaternion rotation, Vector3 scale) previousTransform;

    // where the object is now
    private (Vector3 position, Quaternion rotation, Vector3 scale) currentTransform;

    // create a new action
    public Action(ActionType type, GameObject targetObject, (Vector3 position, Quaternion rotation, Vector3 scale) previousTransform)
    {
        Type = type;
        TargetObject = targetObject;
        this.previousTransform = previousTransform;
        
        // save where it is now
        this.currentTransform = (
            targetObject.transform.position,
            targetObject.transform.rotation,
            targetObject.transform.localScale
        );
    }

    // get where it was before
    public (Vector3 position, Quaternion rotation, Vector3 scale) GetPreviousTransform()
    {
        return previousTransform;
    }

    // get where it is now
    public (Vector3 position, Quaternion rotation, Vector3 scale) GetCurrentTransform()
    {
        return currentTransform;
    }
}