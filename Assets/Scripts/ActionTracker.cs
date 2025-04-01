using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
// using System.Linq;

/// <summary>
/// Manages the undo/redo system for object transformations in the scene.
/// Tracks all actions performed on objects and allows reversing or reapplying them.
/// </summary>
public class ActionTracker : MonoBehaviour
{
    [SerializeField]
    private ObjectSpawner objectSpawner; // ASSIGN IN THE INSPECTOR

    // the moment an action is completed, the action is classified as either spawn, translate, scale, rotate etc and added to this action stack
    private Stack<Action> undoStack = new Stack<Action>();

    // when action is undone, it moves here
    // have to still keep track of these actions in case we want to redo the undo
    private Stack<Action> redoStack = new Stack<Action>();

    // linked list doubly
    // 

    private const int MAX_UNDO_LEVELS = 10;

    void Awake()
    {
        if (objectSpawner == null)
        {
            Debug.LogError("ObjectSpawner reference not set in ActionTracker!");
        }
    }

    /// Called when an object is spawned
    public void OnObjectSpawned(GameObject obj)
    {
        if (obj != null)
        {
            AddAction(new Action(ActionType.Spawn, obj, (obj.transform.position, obj.transform.rotation, obj.transform.localScale)));
        }
    }

    /// Called when an object is despawned
    public void OnObjectDespawned(GameObject obj)
    {
        if (obj != null)
        {
            AddAction(new Action(ActionType.Despawn, obj, (obj.transform.position, obj.transform.rotation, obj.transform.localScale)));
        }
    }

    /// Called when an object starts being grabbed/manipulated
    /// This is called within the XRGrabInteractable script
    public void OnGrabStart(XRGrabInteractable grabInteractable)
    {
        // Store the initial transform state when grabbing starts
        if (grabInteractable != null)
        {
            // Create a new Transform to store the initial state
            Transform initialTransform = new GameObject("InitialTransform").transform;
            initialTransform.position = grabInteractable.transform.position;
            initialTransform.rotation = grabInteractable.transform.rotation;
            initialTransform.localScale = grabInteractable.transform.localScale;
            
            // store it as a component on the grabbed object
            // we're storing the initial transform in the game object itself, rather than a global dict
            grabInteractable.gameObject.AddComponent<TransformState>().StoreInitialState(initialTransform);
            
            // Clean up the temporary GameObject
            Destroy(initialTransform.gameObject);
        }
    }

    /// Called when an object is released after being grabbed/manipulated
    /// This is called within the XRGrabInteractable script
    public void OnGrabEnd(XRGrabInteractable grabInteractable)
    {
        if (grabInteractable != null)
        {
            TransformState transformState = grabInteractable.GetComponent<TransformState>();
            if (transformState != null)
            {
                // Check what changed during the grab
                if (transformState.HasPositionChanged())
                {
                    AddAction(new Action(ActionType.Translate, grabInteractable.gameObject, transformState.GetInitialTransform()));
                }
                else if (transformState.HasRotationChanged())
                {
                    AddAction(new Action(ActionType.Rotate, grabInteractable.gameObject, transformState.GetInitialTransform()));
                }
                else if (transformState.HasScaleChanged())
                {
                    AddAction(new Action(ActionType.Scale, grabInteractable.gameObject, transformState.GetInitialTransform()));
                }

                // Clean up the temporary component
                Destroy(transformState);
            }
        }
    }

    private void PrintStackContents(string stackName, Stack<Action> stack)
    {
        string timestamp = System.DateTime.Now.ToString("HH:mm:ss.fff");
        Debug.Log($"[{timestamp}] ===== {stackName} Contents ======");
        if (stack.Count == 0)
        {
            Debug.Log($"[{timestamp}] Stack is empty");
            Debug.Log($"[{timestamp}] ================================");
            return;
        }

        // Create a temporary stack to preserve the original
        var tempStack = new Stack<Action>(new Stack<Action>(stack));
        int index = 0;
        while (tempStack.Count > 0)
        {
            var action = tempStack.Pop();
            string objectName = action.TargetObject != null ? action.TargetObject.name : "null object";
            Debug.Log($"[{timestamp}] Stack Entry {index++}: {action.Type} action on {objectName}");
        }
        Debug.Log($"[{timestamp}] ================================");
    }

    /// <summary>
    /// Adds a new action to the undo stack and clears the redo stack.
    /// </summary>
    /// <param name="action">The action to add</param>
    public void AddAction(Action action)
    {
        string timestamp = System.DateTime.Now.ToString("HH:mm:ss.fff");
        undoStack.Push(action);
        Debug.Log($"[{timestamp}] Added {action.Type} action. Undo stack size: {undoStack.Count}");
        // PrintStackContents("Undo Stack", undoStack);

        redoStack.Clear(); // clear redo stack when adding a new action
        Debug.Log($"[{timestamp}] Cleared redo stack");
        // PrintStackContents("Redo Stack", redoStack);

        // Maintain maximum undo levels
        if (undoStack.Count > MAX_UNDO_LEVELS)
        {
            var tempStack = new Stack<Action>();
            for (int i = 0; i < MAX_UNDO_LEVELS - 1; i++)
            {
                tempStack.Push(undoStack.Pop());
            }
            undoStack.Clear();
            while (tempStack.Count > 0)
            {
                undoStack.Push(tempStack.Pop());
            }
            Debug.Log($"[{timestamp}] Trimmed undo stack to {MAX_UNDO_LEVELS} actions");
            PrintStackContents("Trimmed Undo Stack", undoStack);
        }
    }

    /// <summary>
    /// Reverses the most recent action by restoring the object to its previous state.
    /// </summary>
    public void Undo()
    {
        if (undoStack.Count == 0) return; // nothing to undo

        string timestamp = System.DateTime.Now.ToString("HH:mm:ss.fff");
        
        Action action = undoStack.Pop();
        
        // Debug.Log($"[{timestamp}] Undoing {action.Type} action. Remaining undo stack size: {undoStack.Count}");
        PrintStackContents("Undo Stack After Undo", undoStack);
        
        ReverseAction(action);
        redoStack.Push(action);
        
        // Debug.Log($"[{timestamp}] Added to redo stack. Redo stack size: {redoStack.Count}");
        PrintStackContents("Redo Stack After Undo", redoStack);
    }

    /// <summary>
    /// Reapplies the most recently undone action.
    /// </summary>
    public void Redo()
    {
        if (redoStack.Count == 0) return;

        // string timestamp = System.DateTime.Now.ToString("HH:mm:ss.fff");
        
        Action action = redoStack.Pop();
        
        // Debug.Log($"[{timestamp}] Redoing {action.Type} action. Remaining redo stack size: {redoStack.Count}");
        
        RedoAction(action);
        undoStack.Push(action);
        
        //Debug.Log($"[{timestamp}] Added back to undo stack. Undo stack size: {undoStack.Count}");
        
        PrintStackContents("Undo Stack After Redo", undoStack);
        PrintStackContents("Redo Stack After Redo", redoStack);
    }

    /// <summary>
    /// Reverses a specific action by restoring the object to its previous state.
    /// </summary>
    private void ReverseAction(Action action)
    {
        // Safety check - don't proceed if the target object no longer exists
        if (action.TargetObject == null) return;

        var previousTransform = action.GetPreviousTransform();

        switch (action.Type)
        {
            case ActionType.Spawn:
                // If we spawned an object, reverse means destroying it
                if (action.TargetObject != null)
                {
                    var tracker = action.TargetObject.GetComponent<GrabInteractableTracker>();
                    if (tracker != null)
                    {
                        tracker.SetBeingUndone(true); // to make sure it doesn't treat it as a destroy
                    }
                    Destroy(action.TargetObject);
                }
                break;
            case ActionType.Despawn:
                // If we despawned an object, reverse means recreating it
                if (objectSpawner != null)
                {
                    objectSpawner.TrySpawnObject(previousTransform.position, Vector3.up);
                }
                break;
            case ActionType.Translate:
                // Restore the object's position to where it was before the move
                action.TargetObject.transform.position = previousTransform.position;
                break;
            case ActionType.Rotate:
                // Restore the object's rotation to what it was before the rotation
                action.TargetObject.transform.rotation = previousTransform.rotation;
                break;
            case ActionType.Scale:
                // Restore the object's scale to what it was before the scaling
                action.TargetObject.transform.localScale = previousTransform.scale;
                break;
        }
    }

    /// <summary>
    /// Reapplies a specific action by restoring the object to its state after the action.
    /// </summary>
    private void RedoAction(Action action)
    {
        // Safety check - don't proceed if the target object no longer exists

        // NO NULL CHECK!
        // let's say the undo action (i.e. spawn) came from the redo stack
        // it means that the object was deleted from the scene
        // so it will be null ...

        // if (action.TargetObject == null) return;

        var currentTransform = action.GetCurrentTransform();

        switch (action.Type)
        {
            case ActionType.Spawn:
                // If we undid a spawn, redo means recreating the object
                if (objectSpawner != null)
                {
                    objectSpawner.TrySpawnObject(currentTransform.position, Vector3.up);
                }
                break;
            case ActionType.Despawn:
                // If we undid a despawn, redo means destroying it again
                Destroy(action.TargetObject);
                break;
            case ActionType.Translate:
                // Restore the object's position to where it was after the move
                action.TargetObject.transform.position = currentTransform.position;
                break;
            case ActionType.Rotate:
                // Restore the object's rotation to what it was after the rotation
                action.TargetObject.transform.rotation = currentTransform.rotation;
                break;
            case ActionType.Scale:
                // Restore the object's scale to what it was after the scaling
                action.TargetObject.transform.localScale = currentTransform.scale;
                break;
        }
    }

    /// <summary>
    /// Returns the current size of the undo stack.
    /// </summary>
    public int GetUndoStackSize()
    {
        return undoStack.Count;
    }

    /// <summary>
    /// Returns the current size of the redo stack.
    /// </summary>
    public int GetRedoStackSize()
    {
        return redoStack.Count;
    }

    public void Clear()
    {
        string timestamp = System.DateTime.Now.ToString("HH:mm:ss.fff");
        undoStack.Clear();
        redoStack.Clear();
        Debug.Log($"[{timestamp}] Cleared both undo and redo stacks");
    }
} 