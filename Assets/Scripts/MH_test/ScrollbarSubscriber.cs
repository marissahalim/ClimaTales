using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollbarSubscriber : MonoBehaviour
{
    [SerializeField] private ScrollbarManager scrollbarManager;

    private void OnEnable()
    {
        // Subscribe to the OnPanelSelected event
        if (scrollbarManager != null)
        {
            scrollbarManager.OnPanelSelected.AddListener(HandlePanelSelected);
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnPanelSelected event
        if (scrollbarManager != null)
        {
            scrollbarManager.OnPanelSelected.RemoveListener(HandlePanelSelected);
        }
    }

    // Method that will be called when the event is invoked
    private void HandlePanelSelected(int panelIndex)
    {
        Debug.Log($"Panel {panelIndex} selected.");
        // Add your logic here for when a panel is selected
    }
}
