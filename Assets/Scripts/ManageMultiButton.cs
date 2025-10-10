using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/***
Class used to manage only the data layers with sub categories.
- Activates/Deactivates the image displayed on the table
- Changes fill amount of sub category buttons
***/
public class ManageMultiButton : MonoBehaviour
{
    public GameObject dataImage;
    public GameObject dataImageIndicator;
    public GameObject parentButton;
    public int imageState;
    public Image image;
    private int oldActive;


    void Awake()
    {
        image = gameObject.transform.GetChild(1).GetComponent<Image>();
    }

    public void SubButtonClicked()
    {
        gameObject.GetComponent<DataButtonsAudioManager>().playPop();
        oldActive = parentButton.GetComponent<DataLayerImages>().checkIfAnyDataImagesOn();
        //  bool mainState = parentButton.GetComponent<DataLayerImages>().heldDownAlready;
        int newActive = 0;

        if (imageState == 0)
        {
            imageState = 1;
            dataImage.SetActive(true);
            dataImageIndicator.SetActive(true);

            if (CheckIfSubButtonDisplayed())
            {
                StartCoroutine(FillAndDefillImage(imageState));
            }
            else
            {
                // Fill image while not displayed on screen
                image.fillAmount = 1f;
            }
            
            newActive = parentButton.GetComponent<DataLayerImages>().checkIfAnyDataImagesOn();
        }
        else if(imageState == 1)
        {
            imageState = 0;
            dataImage.SetActive(false);
            dataImageIndicator.SetActive(false);

            if (CheckIfSubButtonDisplayed())
            {
                StartCoroutine(FillAndDefillImage(imageState));
            }
            else
            {
                // Defill image while not displayed on screen
                image.fillAmount = 0.2f;
            }

            newActive = parentButton.GetComponent<DataLayerImages>().checkIfAnyDataImagesOn();
        }

        if (oldActive == 0 && newActive == 1)
        {
            parentButton.GetComponent<DataLayerImages>().GetNumbersActive(newActive);
            foreach(GameObject item in parentButton.GetComponent<DataLayerImages>().tableItems)
            {
                item.SetActive(true);
            }
        }
        else if (oldActive == 1 && newActive == 0)
        {
            parentButton.GetComponent<DataLayerImages>().GetNumbersActive(newActive);

            foreach(GameObject item in parentButton.GetComponent<DataLayerImages>().tableItems)
            {
                item.SetActive(false);
            }
        }
        else if (newActive == parentButton.GetComponent<DataLayerImages>().allDataImages.Count)
        {
            parentButton.GetComponent<DataLayerImages>().GetNumbersActive(newActive);
        }
    }

    public IEnumerator FillAndDefillImage(int state)
    {
        float timer = 0;
        float duration = 1f;
        float startAmount = image.fillAmount;
        float targetAmt;

        if (state == 1)
        {
            targetAmt = 1f;
        }
        else
        {
            targetAmt = 0.2f;
        }

        while(timer < duration)
        {
            timer += Time.deltaTime;
            image.fillAmount = Mathf.Lerp(startAmount, targetAmt, timer / duration);
            yield return null;
        }

        image.fillAmount = targetAmt;
    }

    public bool CheckIfSubButtonDisplayed()
    {
        if (gameObject.activeSelf == true)
        {
            return true;
        }
        
        return false;
    }
}
