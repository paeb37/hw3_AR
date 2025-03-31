using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ActionTracker : MonoBehaviour
{   
    // the moment an action is completed, the action is classified as either spawn, translate, scale, rotate etc and added to this action stack
    private Stack<Action> undoStack = new Stack<Action>();

    // when action is undone, it moves here
    // have to still keep track of these actions in case we want to redo the undo
    private Stack<Action> redoStack = new Stack<Action>();

    // linked list doubly
    // 

    private const int MAX_UNDO_LEVELS = 10;

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

    public void AddAction(Action action)
    {
        undoStack.Push(action);

        // clear()
        // add new action to undo stack NOT from redo
        // clear redo stack
        // how it matches ctrl z in README
        // 

        // throw away more than 10!!!!
        
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
        }
    }

    public void Undo()
    {
        if (undoStack.Count == 0) return;

        Action action = undoStack.Pop();

        // add debug log to see how big the stack is
        // 
        action.Reverse();
        redoStack.Push(action);
    }

    public void Redo()
    {
        if (redoStack.Count == 0) return;

        Action action = redoStack.Pop();
        action.Redo();
        undoStack.Push(action);
    }

    // not used currently
    public void Clear()
    {
        undoStack.Clear();
        redoStack.Clear();
    }
} 