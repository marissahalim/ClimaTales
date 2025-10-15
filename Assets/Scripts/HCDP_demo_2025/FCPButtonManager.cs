using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FCPButtonManager : MonoBehaviour
{

    public Button[] buttons;

    public int selectedButtonIndex = -1;   // Keeps track of the currently selected button

    // Start is called before the first frame update
    void Start()
    {
        if (buttons.Length != 10)
        {
            Debug.LogError("Please assign exactly 10 buttons.");
            return;
        }

        // Add listeners to each button
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Local variable to capture the index for the lambda
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    private void OnButtonClicked(int index)
    {
        // Deselect the currently selected button
        if (selectedButtonIndex != -1 && selectedButtonIndex != index)
        {
            buttons[selectedButtonIndex].interactable = true;
        }

        // Update selected button index
        selectedButtonIndex = index;
        Debug.Log("you have pressed button" + buttons[index]);

        // Disable the selected button to indicate selection
        buttons[index].interactable = false;
    }

    public void ResetButtons()
    {
        //Set all buttons to interactable
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }

        //Reset selected button index
        selectedButtonIndex = -1;
    }
}
