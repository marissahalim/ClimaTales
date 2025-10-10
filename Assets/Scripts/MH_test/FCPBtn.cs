using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Michsky.MUIP;
using UnityEngine.UI;
using TMPro;

public class FCPBtn : MonoBehaviour, IPointerClickHandler
{
    // FOR MAP LOADER
    public string buttonValue;
    public FCPBtnOptionLog clickedBtn;

    public FCPBtn otherBtn;         // to check other button toggle state

    // BUTTON DESCRIPTIONS
    public TMP_Text btnDesc;
    public TMP_Text otherBtnDesc;

    // color variables
    public GameObject btnBG;

    public Color normalCol;
    public Color toggledColor = Color.green;

    // Start is called before the first frame update
    void Start()
    {
        normalCol = btnBG.GetComponent<Image>().color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickedBtn.currentOption = buttonValue;     // log the buttonValue to send to the Map Loader

        btnBG.GetComponent<Image>().color = toggledColor;   // Change button color to toggledColor (Green)

        ShowDescription();                          // Show this button's description and hide the other one

        otherBtn.Deselect();                        // Deselect the other button
    }

    // Function to deselect this button
    public void Deselect()
    {
        // buttonDesc.SetActive(false);
        btnBG.GetComponent<Image>().color = normalCol; // Reset color
        Debug.Log(buttonValue + " is deselected.");
    }

    // Function to show this button's description
    private void ShowDescription()
    {
        btnDesc.SetActive(true);       // Show this button's description
        otherBtnDesc.SetActive(false);   // Hide the other button's description
    }
}
