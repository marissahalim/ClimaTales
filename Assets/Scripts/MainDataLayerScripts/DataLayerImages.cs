using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.Assertions.Must;
using System.Diagnostics.Tracing;
using System.Linq;

public class DataLayerImages : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public string dataLayerTag; // tag name for each main data layer
    public string dataTableItems; 
    public string colorString;
    public Transform subButtonsParent; // parent of multiple sub buttons
    public Transform mediaParent; // media panel game objects
    public GameObject miniTableObjects;
    public GameObject singleDataLayer;
    public GameObject singleIndicator;
    public GameObject tableDataLayerParent;
    public List<GameObject> multipleButtons;
    public List<GameObject> mediaList; // touchscreen media list (right panel)
    public int centerInfoDisplayed;
    public bool heldDownAlready;
    public int numbersActive;
    private Slider transparencySlider;
    public Slider yearSlider;
    public List<GameObject> allDataImages = new List<GameObject>(); // used to check if category images are on
    public TMP_Text yearText;
    private GameObject chartReference;
    private GameObject touchscreenManage;
    private Image miniMainBorder;
    private Image miniTableBorder;
    private List<GameObject> dataLayerItems = new List<GameObject>(); // touchscreen info items (left panel)
    private GameObject[] dataLayerObjects;
    private float holdStartTime;
    private float elapsedTime;
    public List<GameObject> tableItems = new List<GameObject>(); // table icon and legend
    private GameObject[] dataTableObjects;
    // private GameObject chartReference;
    bool isFilling;
    bool isDefilling;
    bool isHeldDown;
    float imageFillTime = 0.50f;
    private GameObject[] syncedColors;

    void Start()
    {
        touchscreenManage = GameObject.Find("DataLayerButtons");
        chartReference = GameObject.Find("ChartsDataLoader");

        GetRelatedTag();

        // Data Layer Sub Buttons
        if (subButtonsParent != null)
        {
            foreach(Transform imageChild in subButtonsParent)
            {
                multipleButtons.Add(imageChild.gameObject);
            }
        }

        // Data Layer Media Panel (right)
        if (mediaParent.childCount > 0)
        {
            GetMediaChildren(mediaParent);

            foreach(GameObject media in mediaList)
            {
                media.SetActive(false);
            }
        }

        foreach(Transform childImage in tableDataLayerParent.transform)
        {
            allDataImages.Add(childImage.gameObject);
            childImage.gameObject.SetActive(false);
        }

        // Colored Border
        miniMainBorder = this.gameObject.transform.GetChild(1).GetComponent<Image>();

        // Table Border
        miniTableBorder = miniTableObjects.transform.GetChild(1).GetComponent<Image>();
        
        centerInfoDisplayed = 0;

        // Everything associated with the main data layer is inactive
        // If List is not empty, we have multiple buttons to click from
        if (multipleButtons.Count > 0)
        {
            foreach(GameObject button in multipleButtons)
            {
                // initalize all sub buttons to inactive
                button.SetActive(false);
            }
        }

        if (dataLayerItems.Count > 0)
        {
            foreach(GameObject info in dataLayerItems)
            {
                FindTransparency(info.transform);
                info.SetActive(false);
            }
        }

        if (tableItems.Count > 0)
        {
            foreach(GameObject item in tableItems)
            {
                item.SetActive(false);
            }
        }

        ActivateAgricultureOnStart();
    }

    private void FindTransparency(Transform parent)
    {
        foreach (Transform child in parent)
        {
            GameObject item = child.gameObject;

            if (item.name == "TransparencySliderObject")
            {
                transparencySlider = item.GetComponent<Slider>();

                if (transparencySlider != null)
                {
                    transparencySlider.minValue = 0.0f;
                    transparencySlider.maxValue = 1.0f;
                    transparencySlider.wholeNumbers = false;
                    transparencySlider.onValueChanged.AddListener(ChangeTransparencyOnValueChange);
                }
            }

            GetMediaChildren(child);
        }
    }

    private void GetMediaChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            GameObject media = child.gameObject;

            if (media.name == "TransparencySliderObject")
            {
                transparencySlider = media.GetComponent<Slider>();

                if (transparencySlider != null)
                {
                    transparencySlider.minValue = 0.0f;
                    transparencySlider.maxValue = 1.0f;
                    transparencySlider.wholeNumbers = false;
                    transparencySlider.onValueChanged.AddListener(ChangeTransparencyOnValueChange);
                }
            }

            mediaList.Add(media);
            GetMediaChildren(child);
        }
    }

    public void ActivateAgricultureOnStart()
    {
        if (dataLayerTag == "agriculture")
        {
            mainButtonMultiLayers();
        }
    }

    public void GetRelatedTag()
    {
        if (string.IsNullOrEmpty(dataLayerTag))
        {
            Debug.LogError("dataLayerTag field is empty. Please set a tag for the data layer for: " + gameObject.name);
            return;
        }

        dataLayerObjects = GameObject.FindGameObjectsWithTag(dataLayerTag);

        if (dataLayerObjects.Length == 0)
        {
            Debug.Log("No GameObjects are tagged with" + dataLayerTag);
            return;
        }

        for(int i = 0; i < dataLayerObjects.Length; i++)
        {
            dataLayerItems.Add(dataLayerObjects[i]);
        }

        dataTableObjects = GameObject.FindGameObjectsWithTag(dataTableItems);
        if (dataLayerObjects.Length == 0)
        {
            Debug.Log("No GameObjects are tagged with" + dataTableItems);
            return;
        }
        
        for(int i = 0; i < dataTableObjects.Length; i++)
        {
            tableItems.Add(dataTableObjects[i]);
        }

        syncedColors = GameObject.FindGameObjectsWithTag("color");
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // Button is tapped
        mainButtonMultiLayers();
        holdStartTime = Time.time;
        isHeldDown = true;
        
        if (!heldDownAlready)
        {
            if(!isFilling)
            {
                StartCoroutine(FillImageWhileHeldDown());
                isFilling = true;
            }
        }
        else
        {
            if (!isDefilling)
            {
                StartCoroutine(DefillImageWhileHeldDown());
                isDefilling = true;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        float holdDuration = Time.time - holdStartTime;

        if (holdDuration < 0.2f)
        {
            Debug.Log("It was not held for 2 seconds:");
            Debug.Log(holdDuration);
            // mainButtonMultiLayers();
            gameObject.GetComponent<DataButtonsAudioManager>().playPop();
        } 
        
        if (holdDuration >= 0.5f)
        {
            Debug.Log("Time held");
            Debug.Log(holdDuration);
            // mainButtonMultiLayers();
            gameObject.GetComponent<TurnOnSubButtons>().AllButtonClicked();
            gameObject.GetComponent<DataButtonsAudioManager>().playDone();
            heldDownAlready = !heldDownAlready;
        } 
        else if (miniMainBorder.fillAmount != 0.1f || miniMainBorder.fillAmount != 1f)
        {
            if (isFilling)
            {
                miniMainBorder.fillAmount = 0.1f;
                miniTableBorder.fillAmount = 0.1f;
                isFilling = false;
            }
            else if (isDefilling)
            {
                miniMainBorder.fillAmount = 1f;
                miniTableBorder.fillAmount = 1f;
                isDefilling = false;
            }
        }
    
        isHeldDown = false;
    }

    public IEnumerator FillImageWhileHeldDown()
    {

        while (isHeldDown)
        {
            elapsedTime = Time.time - holdStartTime;

            if (elapsedTime >= imageFillTime)
            {
                miniMainBorder.fillAmount = 1f;
                miniTableBorder.fillAmount = 1f;
                isFilling = false;
            }
            else
            {
                miniMainBorder.fillAmount = elapsedTime / imageFillTime;
                miniTableBorder.fillAmount = elapsedTime / imageFillTime;
            }

            yield return null;
        }
    }

    IEnumerator DefillImageWhileHeldDown()
    {
        float startFillAmount = miniMainBorder.fillAmount;

        while (isHeldDown)
        {
            elapsedTime = Time.time - holdStartTime;

            if (elapsedTime >= imageFillTime)
            {
                miniMainBorder.fillAmount = 0.1f;
                miniTableBorder.fillAmount = 0.1f;
                isDefilling = false;
            }
            else
            {
                float newFillAmount = startFillAmount - (elapsedTime / imageFillTime) * (startFillAmount - 0.1f);
                miniMainBorder.fillAmount = newFillAmount;
                miniTableBorder.fillAmount = newFillAmount;
            }

            yield return null;
        }

    }

    IEnumerator FillWithoutHold()
    {
        float timer = 0;
        float duration = 1f;
        float startAmount = miniMainBorder.fillAmount;
        float targetAmt = 1f;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            miniMainBorder.fillAmount = Mathf.Lerp(startAmount, targetAmt, timer / duration);
            yield return null;
        }

        miniMainBorder.fillAmount = 1f;
    }

    public IEnumerator DefillWithoutHold()
    {
        float timer = 0;
        float duration = 1f;
        float startAmount = miniMainBorder.fillAmount;
        float targetAmt = 0.1f;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            miniMainBorder.fillAmount = Mathf.Lerp(startAmount, targetAmt, timer / duration);
            yield return null;
        }

        miniMainBorder.fillAmount = 0.1f;
    }

    public void mainButtonMultiLayers()
    {
            // Check if other data layers are on, then turn it off -------------
            GameObject currentActive = touchscreenManage.GetComponent<TouchscreenCenter>().checkIfCenterDisplayed();

            if (currentActive)
            {
                List<GameObject> subButtons = currentActive.GetComponent<DataLayerImages>().multipleButtons;
                List<GameObject> medias = currentActive.GetComponent<DataLayerImages>().mediaList;
                List<GameObject> infos = currentActive.GetComponent<DataLayerImages>().dataLayerItems;
                List<GameObject> table = currentActive.GetComponent<DataLayerImages>().tableItems;

                if (subButtons.Count > 0)
                {
                    foreach(GameObject button in subButtons)
                    {
                        button.SetActive(false);
                    }
                }

                if (medias.Count > 0)
                {
                    foreach(GameObject media in medias)
                    {
                        media.SetActive(false);
                    }
                }

                foreach(GameObject info in infos)
                {
                    info.SetActive(false);
                }

                currentActive.GetComponent<DataLayerImages>().centerInfoDisplayed = 0;

                int currOn = currentActive.GetComponent<DataLayerImages>().checkIfAnyDataImagesOn();

                if(table.Count > 0 && currOn > 0)
                {
                    foreach(GameObject tableItem in table)
                    {
                        tableItem.SetActive(false);
                    }
                }
            }

            ChangeBarColor(colorString);
            
            // Then active current data layer sub buttons
            if (multipleButtons.Count > 0)
            {
                foreach(GameObject button in multipleButtons)
                {
                    button.SetActive(true);
                }
            }

            if (mediaList.Count > 0)
            {
                foreach(GameObject media in mediaList)
                {
                    media.SetActive(true);
                }
            }

            foreach(GameObject infoCurrent in dataLayerItems)
            {
                infoCurrent.SetActive(true);
            }

            
            int anyOn = checkIfAnyDataImagesOn();

            if(anyOn > 0)
            {
                foreach(GameObject tableItem in tableItems)
                {
                    tableItem.SetActive(true);
                }
            }

            centerInfoDisplayed = 1;
    }

    public void ChangeBarColor(string tag)
    {
        Color newColor;
        if (UnityEngine.ColorUtility.TryParseHtmlString(tag, out newColor))
        {
            foreach(var item in syncedColors)
            {
                item.GetComponent<Image>().color = newColor;
            }

            chartReference.GetComponent<ChartsGraphGenerator>().HighlightScenarioButton();
        }
        else
        {
            Debug.Log("couldn't change color");
        }
    }

    public int checkIfAnyDataImagesOn()
    {
        int activeCount = allDataImages.Count(obj => obj.activeSelf);

        return activeCount;
    }

    public void GetNumbersActive(int numActive)
    {
        if (numActive == 1)
        {
            StartCoroutine(FillWithoutHold());
        }
        else if (numActive == 0)
        {
            StartCoroutine(DefillWithoutHold());

            if (heldDownAlready)
            {
                heldDownAlready = !heldDownAlready;
                gameObject.GetComponent<TurnOnSubButtons>().allSubLayersOn = 0;
            }
        }
        else if (numActive == allDataImages.Count)
        {
            if (!heldDownAlready)
            {
                heldDownAlready = !heldDownAlready;
                gameObject.GetComponent<TurnOnSubButtons>().allSubLayersOn = 1;
            }
        }
    }

    public void ChangeTransparencyOnValueChange(float value)
    {
        foreach(var image in allDataImages)
        {
            float smoothAlpha = Mathf.Lerp(0.0f, 1.0f, value);
            ChangeTransparency(1.0f - smoothAlpha, image);
        }
    }

    public void ChangeTransparency(float alpha, GameObject image)
    {
        alpha = Mathf.Clamp01(alpha);
        Color currentColor = image.GetComponent<RawImage>().color;
        currentColor.a = alpha;
        image.GetComponent<RawImage>().color = currentColor;
    }
}
