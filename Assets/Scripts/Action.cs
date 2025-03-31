using UnityEngine;

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

    /// Reverses the action by restoring the object to its previous state.
    /// For spawn/despawn: just toggles the object's active state
    /// For transform actions: restores the object's position/rotation/scale to previous values
    public void Reverse()
    {
        // Safety check - don't proceed if the target object no longer exists
        if (TargetObject == null) return;

        switch (Type)
        {
            case ActionType.Spawn:
                // If we spawned an object, reverse means hiding it
                TargetObject.SetActive(false);
                break;
            case ActionType.Despawn:
                // If we despawned an object, reverse means showing it again
                // Don't create a new spawn action, just re-enable the object
                TargetObject.SetActive(true);
                break;
            case ActionType.Translate:
                // Restore the object's position to where it was before the move
                TargetObject.transform.position = previousTransform.position;
                break;
            case ActionType.Rotate:
                // Restore the object's rotation to what it was before the rotation using Quaternion
                // USES QUATERNION TO AVOID GIMBAL LOCK
                TargetObject.transform.rotation = previousTransform.rotation;
                break;
            case ActionType.Scale:
                // Restore the object's scale to what it was before the scaling
                TargetObject.transform.localScale = previousTransform.scale;
                break;
        }
    }

    /// Reapplies the action by restoring the object to its state after the action.
    /// For spawn/despawn: toggles the object's active state
    /// For transform actions: restores the object's position/rotation/scale to the values after the action
    public void Redo()
    {
        // Safety check - don't proceed if the target object no longer exists
        if (TargetObject == null) return;

        switch (Type)
        {
            case ActionType.Spawn:
                // If we undid a spawn, redo means showing the object again
                TargetObject.SetActive(true);
                break;
            case ActionType.Despawn:
                // If we undid a despawn, redo means hiding it again
                TargetObject.SetActive(false);
                break;
            case ActionType.Translate:
                // Restore the object's position to where it was after the move
                TargetObject.transform.position = currentTransform.position;
                break;
            case ActionType.Rotate:
                // Restore the object's rotation to what it was after the rotation using Quaternion
                // USES QUATERNION TO AVOID GIMBAL LOCK
                TargetObject.transform.rotation = currentTransform.rotation;
                break;
            case ActionType.Scale:
                // Restore the object's scale to what it was after the scaling
                TargetObject.transform.localScale = currentTransform.scale;
                break;
        }
    }
}