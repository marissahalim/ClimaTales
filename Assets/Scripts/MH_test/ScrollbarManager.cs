using UnityEngine;
using UnityEngine.Events;

public class ScrollbarManager : MonoBehaviour
{
    // Define the UnityEvent
    [SerializeField] private UnityEvent<int> onPanelSelected;

    // Public property to access the event
    public UnityEvent<int> OnPanelSelected => onPanelSelected;

    // Example method to invoke the event
    public void SelectPanel(int panelIndex)
    {
        // Invokes the event and passes the panelIndex as a parameter
        onPanelSelected?.Invoke(panelIndex);
    }
}
