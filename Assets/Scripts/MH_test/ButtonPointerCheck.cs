using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonPointerCheck : MonoBehaviour, IPointerClickHandler
{
    // Reference to the other button
    public ButtonPointerCheck otherButton;

    // Reference to the buttons' desc
    public TMP_Text buttonDesc;
    public TMP_Text otherButtonDesc;

    [SerializeField] private bool firstDesc;

    // public DownscaleManager downscaleMngr;
    public string option;

    private Button button;
    private Color originalColor;
    [SerializeField] private Color selectedColor = Color.green;

    void Start()
    {
        // Get the Button component and store the original color
        button = GetComponent<Button>();
        originalColor = button.image.color;

        if (firstDesc)
        {
            buttonDesc.SetActive(true);
        }
        otherButtonDesc.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // make the button option avail to MapLoader
        // downscaleMngr.currentOption = option;

        // Deselect the other button
        otherButton.Deselect();

        // Show this button's description and hide the other one
        ShowDescription();

        // keep selected button color
        button.image.color = selectedColor;

        Debug.Log(option + " clicked via pointer.");
    }

    // Function to deselect this button
    public void Deselect()
    {
        buttonDesc.SetActive(false);
        button.image.color = originalColor; // Reset color
        Debug.Log(option + " is deselected.");
    }

    // Function to show this button's description
    private void ShowDescription()
    {
        buttonDesc.SetActive(true);       // Show this button's description
        otherButtonDesc.SetActive(false);   // Hide the other button's description
    }
}
