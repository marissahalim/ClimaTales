using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
Class to detect which data layer is currently displayed on the touchscreen
***/
public class TouchscreenCenter : MonoBehaviour
{
    public List<GameObject> dataLayerButtons;
    private GameObject currentActive;
    // Start is called before the first frame update

    public GameObject checkIfCenterDisplayed()
    {
        foreach(GameObject mainlayerButton in dataLayerButtons)
        {
            if (mainlayerButton.GetComponent<DataLayerImages>().centerInfoDisplayed == 1)
            {
                currentActive = mainlayerButton;
            }
        }

        return currentActive;
    }
}
