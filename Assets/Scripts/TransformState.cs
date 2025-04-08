using UnityEngine;

// stores initial transform state when grabbing starts, removed when grabbing ends
public class TransformState : MonoBehaviour
{   
    // gotta store values not transform itself cuz gameobject might get destroyed
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    // saves initial transform state
    public void StoreInitialState(Transform transform)
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    // gets the saved transform values
    public (Vector3 position, Quaternion rotation, Vector3 scale) GetInitialTransform()
    {
        return (initialPosition, initialRotation, initialScale);
    }

    // checks if position changed
    public bool HasPositionChanged()
    {
        return transform.position != initialPosition;
    }

    // checks if rotation changed
    public bool HasRotationChanged()
    {
        return Quaternion.Angle(transform.rotation, initialRotation) > 0.01f;
    }

    // checks if scale changed
    public bool HasScaleChanged()
    {
        return transform.localScale != initialScale;
    }
}