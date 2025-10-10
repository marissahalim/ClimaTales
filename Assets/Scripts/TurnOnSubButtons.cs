using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/***
Class used to turn on all buttons when the main button is held down
***/
public class TurnOnSubButtons : MonoBehaviour
{
    public GameObject ChartsGeneratorReference;
    public int allSubLayersOn;
    private List<GameObject> childrenButtons;
    private List<GameObject> childrenMedia;
    private List<GameObject> tableItems;
    private List<GameObject> multiImages;
    private Slider yearSlide;
    private Slider solarSlide;
    private bool mainBool;

    // Start is called before the first frame update
    void Awake()
    {
        childrenButtons = this.gameObject.GetComponent<DataLayerImages>().multipleButtons;
        childrenMedia = this.gameObject.GetComponent<DataLayerImages>().mediaList;
        tableItems = this.gameObject.GetComponent<DataLayerImages>().tableItems;
        multiImages = this.gameObject.GetComponent<DataLayerImages>().allDataImages;
        mainBool = this.gameObject.GetComponent<DataLayerImages>().heldDownAlready;
        yearSlide = this.gameObject.GetComponent<DataLayerImages>().yearSlider;
        solarSlide = ChartsGeneratorReference.GetComponent<ChartsGraphGenerator>().solarSlider;
        allSubLayersOn = 0;
    }

    public void AllButtonClicked()
    {
        if (allSubLayersOn == 0)
        {
            allSubLayersOn = 1;
            if (childrenButtons.Count > 0)
            {
                foreach(GameObject subLayer in childrenButtons)
                {
                    subLayer.GetComponent<ManageMultiButton>().imageState = 1;
                    subLayer.GetComponent<ManageMultiButton>().dataImage.SetActive(true);
                    subLayer.GetComponent<ManageMultiButton>().dataImageIndicator.SetActive(true);

                    if (subLayer.GetComponent<ManageMultiButton>().CheckIfSubButtonDisplayed())
                    {
                        StartCoroutine(subLayer.GetComponent<ManageMultiButton>().FillAndDefillImage(subLayer.GetComponent<ManageMultiButton>().imageState));
                    }
                    else
                    {
                        // Fill image while not displayed on screen
                        subLayer.GetComponent<ManageMultiButton>().image.fillAmount = 1f;
                    }
                }
            }
            else if (multiImages.Count > 1 && childrenButtons.Count == 0)
            {
                this.gameObject.GetComponent<DataLayerImages>().singleDataLayer.SetActive(true);
                this.gameObject.GetComponent<DataLayerImages>().singleIndicator.SetActive(true);

                if (gameObject.name == "SolarButton")
                {
                    // multiImages[1].SetActive(true);
                    ChartsGeneratorReference.GetComponent<ChartsGraphGenerator>().UpdateYearWithSlider();
                }
            }
            else
            {
                this.gameObject.GetComponent<DataLayerImages>().singleDataLayer.SetActive(true);
                this.gameObject.GetComponent<DataLayerImages>().singleIndicator.SetActive(true);
            }
            
            if (this.gameObject.GetComponent<DataLayerImages>().centerInfoDisplayed == 1)
            {
                foreach(GameObject item in tableItems)
                {
                    item.SetActive(true);
                }
            }
        }
        else if (allSubLayersOn == 1)
        {
            allSubLayersOn = 0;
            if (mainBool != false)
            {
                mainBool = false;
            }
            if (childrenButtons.Count > 0)
            {
                foreach(GameObject subLayer in childrenButtons)
                {
                    subLayer.GetComponent<ManageMultiButton>().imageState = 0;
                    subLayer.GetComponent<ManageMultiButton>().dataImage.SetActive(false);
                    subLayer.GetComponent<ManageMultiButton>().dataImageIndicator.SetActive(false);

                    if (subLayer.GetComponent<ManageMultiButton>().CheckIfSubButtonDisplayed())
                    {
                        StartCoroutine(subLayer.GetComponent<ManageMultiButton>().FillAndDefillImage(subLayer.GetComponent<ManageMultiButton>().imageState));
                    }
                    else
                    {
                        // Fill image while not displayed on screen
                        subLayer.GetComponent<ManageMultiButton>().image.fillAmount = 0.15f;
                    }
                }
            }
            else if (multiImages.Count > 1 && childrenButtons.Count == 0)
            {
                this.gameObject.GetComponent<DataLayerImages>().singleIndicator.SetActive(false);

                if (gameObject.name == "SolarButton")
                {
                    this.gameObject.GetComponent<DataLayerImages>().singleDataLayer.SetActive(false);
                    // solarSlide.value = 0f;
                }
                else
                {
                    this.gameObject.GetComponent<DataLayerImages>().yearSlider.value = 0f;
                }

                foreach(GameObject img in multiImages)
                {
                    img.SetActive(false);
                }
            }
            else
            {
                this.gameObject.GetComponent<DataLayerImages>().singleDataLayer.SetActive(false);
                this.gameObject.GetComponent<DataLayerImages>().singleIndicator.SetActive(false);
            }

            if (this.gameObject.GetComponent<DataLayerImages>().centerInfoDisplayed == 1)
            {
                foreach(GameObject item in tableItems)
                {
                    item.SetActive(false);
                }
            }
        }
    }
}
