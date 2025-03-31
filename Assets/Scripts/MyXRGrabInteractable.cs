// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit.Interactables;
// using UnityEngine.XR.Interaction.Toolkit.Transformers;

// namespace XRInteraction
// {
//     public class MyXRGrabInteractable : XRGrabInteractable
//     {
//         private ActionTracker actionTracker;

//         protected override void Awake()
//         {
//             base.Awake(); // keep functionality from original XRGrabInteractable
//             actionTracker = FindObjectOfType<ActionTracker>();
//         }

//         protected override void InvokeGrabTransformersProcess(XRInteractionUpdateOrder.UpdatePhase updatePhase, ref Pose targetPose, ref Vector3 localScale)
//         {
//             // Store initial transform state when grab starts
//             if (actionTracker != null)
//             {
//                 actionTracker.OnGrabStart(this);
//             }

//             base.InvokeGrabTransformersProcess(updatePhase, ref targetPose, ref localScale);
//         }

//         protected override void InvokeGrabTransformersOnDrop(DropEventArgs args)
//         {
//             // Record final transform state when dropping
//             if (actionTracker != null)
//             {
//                 actionTracker.OnGrabEnd(this);
//             }

//             base.InvokeGrabTransformersOnDrop(args);
//         }
//     }
// } 