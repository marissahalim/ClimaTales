using UnityEngine;
using UnityEngine.UI;


// This script controls the colors of the rainfall and temperature buttons
// And logs the datatype value for the HistDataController
public class HistDataTypeSelector : MonoBehaviour
{
    public Button rainButton;
    public Button tempButton;

    public string selectedMode;
    public int scaleIndex;

    private Color rainSelectedColor = new Color(0.3607843f, 0.7058824f, 0.9725491f);  // Blue-ish
    private Color tempSelectedColor = new Color(0.9450981f, 0.627451f, 0.3137255f);  // Orange-ish
    private Color defaultColor = Color.white;

    public ScalesManager scalesManager;

    private void Start()
    {
        selectedMode = "No Data";
        rainButton.onClick.AddListener(() => ToggleMode("Rain"));
        tempButton.onClick.AddListener(() => ToggleMode("Temp"));

        SetButtonColor(rainButton, false, rainSelectedColor);
        SetButtonColor(tempButton, false, tempSelectedColor);
    }

    public void ToggleMode(string mode)
    {
        if (selectedMode == mode)
        {
            selectedMode = "No Data"; // Deselect
        }
        else
        {
            selectedMode = mode;
        }
        UpdateButtonStates();
        Debug.Log("Selected Mode: " + selectedMode);
    }

    public void SetStoryDataType(string mode)
    {
        selectedMode = mode;

        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        SetButtonColor(rainButton, selectedMode == "Rain", rainSelectedColor);
        SetButtonColor(tempButton, selectedMode == "Temp", tempSelectedColor);
    }

    private void SetButtonColor(Button button, bool isSelected, Color selectedColor)
    {
        var colors = button.colors;
        colors.normalColor = isSelected ? selectedColor : defaultColor;
        colors.selectedColor = isSelected ? selectedColor : defaultColor;
        colors.highlightedColor = isSelected ? selectedColor : defaultColor;
        colors.pressedColor = isSelected ? selectedColor : defaultColor;
        button.colors = colors;
    }


    // Call this function in WindowManager OnWindowChange
    public void ResetSelection()
    {
        selectedMode = "No Data";
        scaleIndex = -1;  // Reset the index

        // Reset button colors back to default
        SetButtonColor(rainButton, false, rainSelectedColor);
        SetButtonColor(tempButton, false, tempSelectedColor);

        // Ensure buttons remain interactable
        rainButton.interactable = true;
        tempButton.interactable = true;
    }
}
