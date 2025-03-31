using UnityEngine;

/// <summary>
/// Temporary component that stores the initial transform state of an object being grabbed.
/// This component is added when grabbing starts and removed when grabbing ends.
/// </summary>
public class TransformState : MonoBehaviour
{   
    // have to store the values, not the transform itself
    // because that's attached to gameobject which could be destroyed temporarily
    
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    /// <summary>
    /// Stores the initial transform state of the object
    /// </summary>
    public void StoreInitialState(Transform transform)
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    /// <summary>
    /// Gets the stored initial transform values
    /// </summary>
    public (Vector3 position, Quaternion rotation, Vector3 scale) GetInitialTransform()
    {
        return (initialPosition, initialRotation, initialScale);
    }

    /// <summary>
    /// Checks if the position has changed from the initial state
    /// </summary>
    public bool HasPositionChanged()
    {
        return transform.position != initialPosition;
    }

    /// <summary>
    /// Checks if the rotation has changed from the initial state
    /// </summary>
    public bool HasRotationChanged()
    {
        return Quaternion.Angle(transform.rotation, initialRotation) > 0.01f;
    }

    /// <summary>
    /// Checks if the scale has changed from the initial state
    /// </summary>
    public bool HasScaleChanged()
    {
        return transform.localScale != initialScale;
    }
}