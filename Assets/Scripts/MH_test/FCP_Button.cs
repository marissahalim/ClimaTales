using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FCP_Button : MonoBehaviour
{
    //public FCP_Button_Manager btnManager;
    public Button myButton;

    public MapLoader mapLoader;

    //public string dm = "Stat";
    //public string cont = "Change";
    //public string sce = "4.5";

    void Start()
    {
        // Add a listener to the button click event
        //myButton.onClick.AddListener(OnButtonClick);
    }

    //public void OnButtonClick()
    //{
    //    // Get the TMP_Text component of the button
    //    TMP_Text buttonText = myButton.GetComponentInChildren<TMP_Text>();

    //    if (buttonText != null)
    //    {
    //        // Pass the text to the switch case function
    //        HandleButtonAction(buttonText.text);
    //        //fcpContent = buttonText.text;
    //        //Debug.Log(fcpContent);
    //    }
    //    else
    //    {
    //        Debug.LogError("TMP_Text component not found on button!");
    //    }
    //}

    //public void SetDownscaleMethod()
    //{
    //    // Get the TMP_Text component of the button
    //    TMP_Text buttonText = myButton.GetComponentInChildren<TMP_Text>();

    //    if (buttonText.Equals("Dynamical"))
    //    {
    //        mapLoader.dm = "Dyn";
    //    } else
    //    {
    //        mapLoader.dm = "Stat";
    //    }
    //}

    //public void SetContent()
    //{
    //    // Get the TMP_Text component of the button
    //    TMP_Text buttonText = myButton.GetComponentInChildren<TMP_Text>();

    //    if (buttonText.Equals("Value"))
    //    {
    //        mapLoader.cont = "Value";
    //    } else
    //    {
    //        mapLoader.cont = "Change";
    //    }
    //}

    //public void SetScenario()
    //{
    //    // Get the TMP_Text component of the button
    //    TMP_Text buttonText = myButton.GetComponentInChildren<TMP_Text>();
    //    if (buttonText.Equals("RCP 8.5"))
    //    {
    //        mapLoader.sce = "8.5";
    //    } else
    //    {
    //        mapLoader.sce = "4.5";
    //    }
    //}

    //void HandleButtonAction(string buttonText)
    //{
    //    // Switch case based on the button's text content
    //    switch (buttonText)
    //    {
    //        case "Statistical":
    //            dm = "Stat";
    //            Debug.Log(dm);
    //            break;

    //        case "Dynamical":
    //            dm = "Dyn";
    //            Debug.Log(dm);
    //            break;

    //        case "RCP 4.5":
    //            sce = "4.5";
    //            Debug.Log(sce);
    //            break;

    //        case "RCP 8.5":
    //            sce = "8.5";
    //            Debug.Log(sce);
    //            break;

    //        case "Absolute Change":
    //            cont = "Change";
    //            Debug.Log(cont);
    //            break;

    //        case "Value":
    //            cont = "Value";
    //            Debug.Log(cont);
    //            break;

    //        default:
    //            Debug.LogWarning("Unrecognized button action: " + buttonText);
    //            break;
    //    }
    //}


}
