using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
// using System.Linq;

// keeps track of all the stuff you do to objects so you can undo/redo them
public class ActionTracker : MonoBehaviour
{
    [SerializeField]
    private ObjectSpawner objectSpawner; // don't forget to set this in unity!

    // when you do something, it goes here
    private Stack<Action> undoStack = new Stack<Action>();

    // when you undo something, it goes here
    // gotta keep these around in case you wanna redo them
    private Stack<Action> redoStack = new Stack<Action>();

    private const int MAX_UNDO_LEVELS = 10;

    void Awake()
    {
        if (objectSpawner == null)
        {
            Debug.LogError("ObjectSpawner reference not set in ActionTracker!");
        }
    }

    // called when you spawn something
    public void OnObjectSpawned(GameObject obj)
    {
        if (obj != null)
        {
            AddAction(new Action(ActionType.Spawn, obj, (obj.transform.position, obj.transform.rotation, obj.transform.localScale)));
        }
    }

    // called when you delete something
    public void OnObjectDespawned(GameObject obj)
    {
        if (obj != null)
        {
            AddAction(new Action(ActionType.Despawn, obj, (obj.transform.position, obj.transform.rotation, obj.transform.localScale)));
        }
    }

    // called when you start grabbing stuff
    public void OnGrabStart(XRGrabInteractable grabInteractable)
    {
        if (grabInteractable != null)
        {
            Transform initialTransform = new GameObject("InitialTransform").transform;
            initialTransform.position = grabInteractable.transform.position;
            initialTransform.rotation = grabInteractable.transform.rotation;
            initialTransform.localScale = grabInteractable.transform.localScale;
            
            grabInteractable.gameObject.AddComponent<TransformState>().StoreInitialState(initialTransform);
            
            Destroy(initialTransform.gameObject);
        }
    }

    // called when you let go of stuff
    public void OnGrabEnd(XRGrabInteractable grabInteractable)
    {
        if (grabInteractable != null)
        {
            TransformState transformState = grabInteractable.GetComponent<TransformState>();
            if (transformState != null)
            {
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

                Destroy(transformState);
            }
        }
    }

    private void PrintStackContents(string stackName, Stack<Action> stack)
    {
        string timestamp = System.DateTime.Now.ToString("HH:mm:ss.fff");
        if (stack.Count == 0)
        {
            return;
        }

        var tempStack = new Stack<Action>(new Stack<Action>(stack));
        while (tempStack.Count > 0)
        {
            var action = tempStack.Pop();
            string objectName = action.TargetObject != null ? action.TargetObject.name : "null object";
        }
    }

    // adds new stuff to undo stack and clears redo stack
    public void AddAction(Action action)
    {
        string timestamp = System.DateTime.Now.ToString("HH:mm:ss.fff");
        undoStack.Push(action);
        redoStack.Clear();

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
            PrintStackContents("Trimmed Undo Stack", undoStack);
        }
    }

    // makes the last thing you did not happen
    public void Undo()
    {
        if (undoStack.Count == 0) return;

        string timestamp = System.DateTime.Now.ToString("HH:mm:ss.fff");
        
        Action action = undoStack.Pop();
        PrintStackContents("Undo Stack After Undo", undoStack);
        
        ReverseAction(action);
        redoStack.Push(action);
        
        PrintStackContents("Redo Stack After Undo", redoStack);
    }

    // makes the last thing you undid happen again
    public void Redo()
    {
        if (redoStack.Count == 0) return;
        
        Action action = redoStack.Pop();
        
        RedoAction(action);
        undoStack.Push(action);
        
        PrintStackContents("Undo Stack After Redo", undoStack);
        PrintStackContents("Redo Stack After Redo", redoStack);
    }

    // makes an action not happen
    private void ReverseAction(Action action)
    {
        if (action.TargetObject == null) return;

        var previousTransform = action.GetPreviousTransform();

        switch (action.Type)
        {
            case ActionType.Spawn:
                if (action.TargetObject != null)
                {
                    var tracker = action.TargetObject.GetComponent<GrabInteractableTracker>();
                    if (tracker != null)
                    {
                        tracker.SetBeingUndone(true);
                    }
                    action.TargetObject.SetActive(false);
                }
                break;
            case ActionType.Despawn:
                if (action.TargetObject != null)
                {
                    action.TargetObject.SetActive(true);
                    action.TargetObject.transform.position = previousTransform.position;
                    action.TargetObject.transform.rotation = previousTransform.rotation;
                    action.TargetObject.transform.localScale = previousTransform.scale;
                }
                break;
            case ActionType.Translate:
                action.TargetObject.transform.position = previousTransform.position;
                break;
            case ActionType.Rotate:
                action.TargetObject.transform.rotation = previousTransform.rotation;
                break;
            case ActionType.Scale:
                action.TargetObject.transform.localScale = previousTransform.scale;
                break;
        }
    }

    // makes an undone action happen again
    private void RedoAction(Action action)
    {
        var currentTransform = action.GetCurrentTransform();

        switch (action.Type)
        {
            case ActionType.Spawn:
                if (action.TargetObject != null)
                {
                    action.TargetObject.SetActive(true);
                    action.TargetObject.transform.position = currentTransform.position;
                    action.TargetObject.transform.rotation = currentTransform.rotation;
                    action.TargetObject.transform.localScale = currentTransform.scale;
                }
                break;
            case ActionType.Despawn:
                if (action.TargetObject != null)
                {
                    action.TargetObject.SetActive(false);
                }
                break;
            case ActionType.Translate:
                if (action.TargetObject != null)
                {
                    action.TargetObject.transform.position = currentTransform.position;
                }
                break;
            case ActionType.Rotate:
                if (action.TargetObject != null)
                {
                    action.TargetObject.transform.rotation = currentTransform.rotation;
                }
                break;
            case ActionType.Scale:
                if (action.TargetObject != null)
                {
                    action.TargetObject.transform.localScale = currentTransform.scale;
                }
                break;
        }
    }

    // how many things can you undo?
    public int GetUndoStackSize()
    {
        return undoStack.Count;
    }

    // how many things can you redo?
    public int GetRedoStackSize()
    {
        return redoStack.Count;
    }

    // clears everything
    public void Clear()
    {
        string timestamp = System.DateTime.Now.ToString("HH:mm:ss.fff");
        undoStack.Clear();
        redoStack.Clear();
    }
} 