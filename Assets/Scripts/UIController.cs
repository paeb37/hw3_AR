using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] private Toggle toggleButton;
    [SerializeField] private ObjectSelector objectSelector;
    
    void Start()
    {
        // Debug.Log("UIController Start - Toggle Button: " + (toggleButton != null) + ", Object Selector: " + (objectSelector != null));
        
        // Set toggle to start unchecked
        toggleButton.isOn = false;
        
        // toggleButton.onValueChanged.AddListener((isOn) => {
        //     Debug.Log("Toggle value changed to: " + isOn);
        //     if (isOn)
        //     {
        //         Debug.Log("Attempting to trigger selection");
        //         objectSelector.TriggerSelection();
        //         // objectSelector.SetScale(2f);
        //     }
        //     else
        //     {
        //         Debug.Log("Attempting to release selection");
        //         objectSelector.ReleaseSelection();
        //     }
        // });

        toggleButton.onValueChanged.AddListener(OnToggleValueChanged);
    }

    void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("Toggle is ON");
            // Debug.Log("Attempting to trigger selection");
            objectSelector.TriggerSelection();
        }
        else
        {
            Debug.Log("Toggle is OFF");
            // Debug.Log("Attempting to release selection");
            objectSelector.ReleaseSelection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the toggle button is being clicked
        // if (toggleButton != null && toggleButton.interactable)
        // {
        //     // If it's clicked, toggle its state
        //     if (Mouse.current.leftButton.wasPressedThisFrame)
        //     {
        //         toggleButton.isOn = !toggleButton.isOn;
        //     }
        // }
    }
}
