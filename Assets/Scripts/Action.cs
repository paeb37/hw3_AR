using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

/// <summary>
/// Defines the different types of actions that can be performed on objects in the scene.
/// Each type corresponds to a specific transformation or state change.
/// </summary>
public enum ActionType
{
    Spawn,      // Creating a new object
    Despawn,    // Removing an existing object
    Translate,  // Moving an object
    Scale,      // Resizing an object
    Rotate      // Rotating an object
}

/// <summary>
/// Represents a single action that can be undone or redone.
/// Stores the type of action, the target object, and the previous state of the object.
/// </summary>
public class Action
{
    /// <summary>
    /// The type of action performed (spawn, despawn, translate, scale, or rotate)
    /// </summary>
    public ActionType Type { get; private set; }

    /// <summary>
    /// The GameObject that was affected by this action
    /// </summary>
    public GameObject TargetObject { get; private set; }

    /// <summary>
    /// Stores the complete Transform state of the object before the action was performed.
    /// This includes position, rotation, and scale values.
    /// </summary>
    private (Vector3 position, Quaternion rotation, Vector3 scale) previousTransform;

    /// <summary>
    /// Stores the complete Transform state of the object after the action was performed.
    /// This includes position, rotation, and scale values.
    /// </summary>
    private (Vector3 position, Quaternion rotation, Vector3 scale) currentTransform;

    /// <summary>
    /// Creates a new Action with the specified type and target object.
    /// </summary>
    /// <param name="type">The type of action being performed</param>
    /// <param name="targetObject">The GameObject being affected</param>
    /// <param name="previousTransform">Transform values before the action</param>
    public Action(ActionType type, GameObject targetObject, (Vector3 position, Quaternion rotation, Vector3 scale) previousTransform)
    {
        Type = type;
        TargetObject = targetObject;
        this.previousTransform = previousTransform;
        
        // Store the current state after the action
        this.currentTransform = (
            targetObject.transform.position,
            targetObject.transform.rotation,
            targetObject.transform.localScale
        );
    }

    /// <summary>
    /// Gets the previous transform state of the object
    /// </summary>
    public (Vector3 position, Quaternion rotation, Vector3 scale) GetPreviousTransform()
    {
        return previousTransform;
    }

    /// <summary>
    /// Gets the current transform state of the object
    /// </summary>
    public (Vector3 position, Quaternion rotation, Vector3 scale) GetCurrentTransform()
    {
        return currentTransform;
    }
}