using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Michsky.MUIP;
using MzDemo;

public class MapLoaderCirc : MonoBehaviour
{

    [Header("Base")]
    public WindowManager windowManager;
    public RawImage histTextureHolder;
    public RawImage futureTextureHolder;

    public PrebuiltStory story;

    // Textures should be in a Resources folder
    [SerializeField] private string basePath = "Textures"; // Base path for your textures

    // OVERALL VARIABLES
    // public string histDataType;
    public string fcpDataType;
    public string dataType;
    private string currentDataTypeName;

    [Header("Labels")]
    public TMP_Text mapLabel_dataType;
    public TMP_Text mapLabel_time;
    public TMP_Text mapLabel_scen_cont;
    public TMP_Text mapLabel_downscale;

    // public TMP_Text mapLabel_ENSO;
    public Image icon_ENSO;
    public Sprite elNino;
    public Sprite laNina;

    [Header("Historical")]
    // HIST DATA CONTROL
    // public Demo_UIScrollCtrl icons;
    public HistDataTypeSelector dataTypeSelector;
    public KnobController years;
    public KnobController months;

    // HISTORICAL VARIABLES
    private int currentYear;
    private int currentMonth;

    // HISTORICAL LABEL CONTENTS
    private string currentMonthName;
    private string[] monthNames = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

    [Header("Future")]
    // FCP DATA CONTROL
    public Demo_UIScrollCtrl fcpIcons;
    public FCPButtonManager fcpMap;
    public TMP_Text mapNotSelected; // for when user hasn't pressed all the buttons

    // FUTURE LABEL CONTENTS
    private string currentTime;
    private string currentDownscale;
    private string currentContent;
    private string currentScenario;

    //SCALES
    public ScalesManager scalesManager;
    private int currentScaleIndex;

    void Start()
    {
        // histDataType = dataTypeSelector.selectedMode;
        dataType = dataTypeSelector.selectedMode;
    }

    // Update is called once per frame
    void Update()
    {
        if (windowManager.currentWindowIndex == 0)
        {
            // set data type
            dataType = dataTypeSelector.selectedMode;
            currentScaleIndex = dataTypeSelector.scaleIndex;
            futureTextureHolder.SetActive(false);

            // if datatype has not been chosen, hide the data layer
            if (dataType.Equals("No Data"))
            {
                histTextureHolder.SetActive(false);
                mapNotSelected.SetActive(true);
                mapNotSelected.text = "";
                mapLabel_dataType.text = "";
                mapLabel_time.text = "";
                mapLabel_scen_cont.text = "";
                scalesManager.ResetScales();
                fcpMap.ResetButtons();
                currentScaleIndex = -1;
            }
            else
            {
                mapNotSelected.SetActive(false);
                histTextureHolder.SetActive(true);
                LoadHistoricalTexture();
                scalesManager.SetActiveScale(currentScaleIndex);
            }
        }
        else if (windowManager.currentWindowIndex == 1)
        {
            dataType = fcpDataType;
            histTextureHolder.SetActive(false);

            // if datatype has not been chosen OR any of the FUTURE BUTTONS (FB) are null, hide the data layer
            if (dataType.Equals("No Data"))
            {
                futureTextureHolder.SetActive(false);         // hide the data layer
                mapNotSelected.SetActive(true);               // show text that tells user to set a value for all FB options
                mapNotSelected.text = "";
                mapLabel_dataType.text = "";
                mapLabel_time.text = "";
                mapLabel_scen_cont.text = "";
                mapLabel_downscale.text = "";
                scalesManager.ResetScales();
                fcpMap.ResetButtons();
            }
            else if (fcpMap.selectedButtonIndex == -1)
            {
                futureTextureHolder.SetActive(false);             // hide the data layer
                mapNotSelected.SetActive(true);             // show text that tells user to set a value for all FB options
                mapNotSelected.text = "";
            }
            else
            {
                mapNotSelected.SetActive(false);
                futureTextureHolder.SetActive(true);
                LoadFCPTexture();
                scalesManager.SetActiveScale(currentScaleIndex);
            }
        }
    }

    // HISTORICAL CODE ////////////////////////////////////////////////////////////////////////////////////////////
    public void SetMonthName(int curMonth)
    {
        currentMonthName = monthNames[curMonth];
    }

    // public void SetHistDataType(int selectedDataType)
    public void SetHistDataType()
    {
        // histDataType = dataTypeSelector.selectedMode;
        // currentScaleIndex = dataTypeSelector.scaleIndex;
    }

    // CHECK EL NINO YEARS
    void CheckENSO(int year)
    {
        if (year == 1992 || year == 1995 || year == 1998 || year == 2003 || year == 2007 || year == 2010 || year == 2016)
        {
            icon_ENSO.SetActive(true);
            icon_ENSO.sprite = elNino;
        }
        else if (year == 1999 || year == 2000 || year == 2008 || year == 2011 || year == 2012 || year == 2021 || year == 2022)
        {
            icon_ENSO.SetActive(true);
            icon_ENSO.sprite = laNina;
        }
        else
        {
            icon_ENSO.SetActive(false);
        }
    }

    // HISTORICAL TEXTURE FUNCTION
    public void LoadHistoricalTexture()
    {
        // SetHistDataType();

        currentYear = (int)years.currentValue;
        currentMonth = (int)months.currentValue + 1;

        SetMonthName(currentMonth - 1);

        // Format the file name based on the given year and month
        string fileName = $"{currentYear}_{currentMonth:D2}";
        string fullPath = $"{basePath}/{dataType}/{"hist"}/{currentYear}/{fileName}";

        // Load the texture from the Resources folder
        Texture2D mapTexture = Resources.Load<Texture2D>(fullPath);

        if (mapTexture != null)
        {
            // Apply the texture to a material or sprite renderer
            histTextureHolder.texture = mapTexture;

            mapLabel_dataType.text = $"{currentDataTypeName}";
            mapLabel_time.text = currentMonthName + $" {currentYear}";
            CheckENSO(currentYear);
        }
        else
        {
            Debug.LogError($"Failed to load texture: {fullPath}");
        }
    }

    // FCP CODE ////////////////////////////////////////////////////////////////////////////////////////////

    // SET FUTURE DATA TYPE
    public void SetFCPDataType(int selectedDataType)
    {
        switch (selectedDataType)
        {
            case 0:
                // Debug.Log("No data shown");
                fcpDataType = "No Data";
                currentDataTypeName = "No data showing";
                break;
            case 1:
                // Debug.Log("I am future rain");
                fcpDataType = "FCP_Rain";
                currentDataTypeName = "Annual Rainfall (in)";
                break;
            case 2:
                // Debug.Log("I am future temp");
                fcpDataType = "FCP_Temp";
                currentDataTypeName = "Annual Mean Temperature (°F)";
                break;
            default:
                break;
        }
    }

    // FUTURE TEXTURE FUNCTION
    public void LoadFCPTexture()
    {
        string fullPath = $"";

        switch (fcpMap.selectedButtonIndex)
        {
            case 0:
                // Debug.Log("Present Dynamical");
                fullPath = $"{basePath}/{dataType}/{"Dyn"}/{"Present"}";
                currentTime = "Present";
                currentDownscale = "Dynamical Downscaling";
                currentContent = "";
                currentScenario = "";
                break;
            case 1:
                // Debug.Log("Future Dynamical Best case Value");
                fullPath = $"{basePath}/{dataType}/{"Dyn"}/{"End"}_{"V"}_{"4.5"}";
                currentTime = "Future";
                currentDownscale = "Dynamical Downscaling";
                currentContent = "Value";
                currentScenario = "Major Emission Reduction (RCP 4.5)";
                break;
            case 2:
                // Debug.Log("Future Dynamical Worst case Value");
                fullPath = $"{basePath}/{dataType}/{"Dyn"}/{"End"}_{"V"}_{"8.5"}";
                currentTime = "Future";
                currentDownscale = "Dynamical Downscaling";
                currentContent = "Value";
                currentScenario = "Modest Emission Reduction (RCP 8.5)";
                break;
            case 3:
                // Debug.Log("Future Dynamical Best case Abs Change");
                fullPath = $"{basePath}/{dataType}/{"Dyn"}/{"End"}_{"AC"}_{"4.5"}";
                currentTime = "Future";
                currentDownscale = "Dynamical Downscaling";
                currentContent = "Absolute Change";
                currentScenario = "Major Emission Reduction (RCP 4.5)";
                break;
            case 4:
                // Debug.Log("Future Dynamical Worst case Abs Change");
                fullPath = $"{basePath}/{dataType}/{"Dyn"}/{"End"}_{"AC"}_{"8.5"}";
                currentTime = "Future";
                currentDownscale = "Dynamical Downscaling";
                currentContent = "Absolute Change";
                currentScenario = "Modest Emission Reduction (RCP 8.5)";
                break;
            case 5:
                // Debug.Log("Present Statistical");
                fullPath = $"{basePath}/{dataType}/{"Stat"}/{"Present"}";
                currentTime = "Present";
                currentDownscale = "Statistical Downscaling";
                currentContent = "";
                currentScenario = "";
                break;
            case 6:
                // Debug.Log("Future Statistical Best case Value");
                fullPath = $"{basePath}/{dataType}/{"Stat"}/{"End"}_{"V"}_{"4.5"}";
                currentTime = "Future";
                currentDownscale = "Statistical Downscaling";
                currentContent = "Value";
                currentScenario = "Major Emission Reduction (RCP 4.5)";
                break;
            case 7:
                // Debug.Log("Future Statistical Worst case Value");
                fullPath = $"{basePath}/{dataType}/{"Stat"}/{"End"}_{"V"}_{"8.5"}";
                currentTime = "Future";
                currentDownscale = "Statistical Downscaling";
                currentContent = "Value";
                currentScenario = "Modest Emission Reduction (RCP 8.5)";
                break;
            case 8:
                // Debug.Log("Future Statistical Best case Abs Change");
                fullPath = $"{basePath}/{dataType}/{"Stat"}/{"End"}_{"AC"}_{"4.5"}";
                currentTime = "Future";
                currentDownscale = "Statistical Downscaling";
                currentContent = "Absolute Change";
                currentScenario = "Major Emission Reduction (RCP 4.5)";
                break;
            case 9:
                // Debug.Log("Future Statistical Worst case Abs Change");
                fullPath = $"{basePath}/{dataType}/{"Stat"}/{"End"}_{"AC"}_{"8.5"}";
                currentTime = "Future";
                currentDownscale = "Statistical Downscaling";
                currentContent = "Absolute Change";
                currentScenario = "Modest Emission Reduction (RCP 8.5)";
                break;
            default:
                break;
        }

        // decide FCP scale
        if (dataType.Equals("FCP_Rain"))
        {
            if (currentContent.Equals("Absolute Change"))
            {
                currentScaleIndex = 4;
            }
            if (currentContent.Equals("Value"))
            {
                currentScaleIndex = 3;
            }
            if (currentTime.Equals("Present"))
            {
                currentScaleIndex = 3;
            }
        }

        if (dataType.Equals("FCP_Temp"))
        {
            if (currentContent.Equals("Absolute Change"))
            {
                currentScaleIndex = 1;
            }
            if (currentContent.Equals("Value"))
            {
                currentScaleIndex = 0;
            }
            if (currentTime.Equals("Present"))
            {
                currentScaleIndex = 0;
            }
        }

        // Load the texture from the Resources folder
        Texture2D mapTexture = Resources.Load<Texture2D>(fullPath);

        if (mapTexture != null)
        {
            // Apply the texture to a material or sprite renderer
            futureTextureHolder.texture = mapTexture;

            //mapLabel.text = $"{currentDataTypeName} - {currentTime} {currentScenario} {currentContent} {currentDownscale}";
            mapLabel_time.text = $"{currentTime}";
            mapLabel_dataType.text = $"{currentDataTypeName}";
            mapLabel_scen_cont.text = $"{currentScenario} - {currentContent}";
            mapLabel_downscale.text = $"{currentDownscale}";
        }
        else
        {
            Debug.LogError($"Failed to load texture: {fullPath}");
        }
    }

    // CHANGING WINDOWS
    //If windows are switched, need to reset everything
    public void SwitchWindows()
    {
        // data
        // histDataType = "No Data";
        fcpDataType = "No Data";
        dataType = "No Data";
        // labels
        mapLabel_dataType.text = "";
        mapLabel_time.text = "";
        mapLabel_scen_cont.text = "";
        mapLabel_downscale.text = "";
        icon_ENSO.SetActive(false);
        // Sliders
        ResetCircSlider(years);
        ResetCircSlider(months);
        // CheckENSO(-1);
        // scales and buttons
        scalesManager.ResetScales();
        fcpMap.ResetButtons();
    }

    public void ResetCircSlider(KnobController circSlider)
    {
        if (circSlider.valueType == KnobController.KnobValueTypes.Ticks && circSlider.currentAngle > circSlider.startAngle) {
            years.SetValue(0);
        }

        if (circSlider.valueType == KnobController.KnobValueTypes.Labels && circSlider.currentAngle > circSlider.startAngle)
        {
            months.SetValue(0);
            circSlider.ResetLabelsUI();
        }
    } 


}