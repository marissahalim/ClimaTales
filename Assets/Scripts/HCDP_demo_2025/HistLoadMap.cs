using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Michsky.MUIP;

// IGNORE THIS SCRIPT, ITS NOT BEING USED
public class HistLoadMap : MonoBehaviour
{
    public WindowManager windowManager;
    public HistStateManager histStateManager;

    [SerializeField] private string basePath = "Textures"; // Base path for your textures

    public HistDataController histDataTypeController;

    public RawImage interactiveTexture;

    public RawImage storyTexture;

    [Header("Labels")]
    public TMP_Text mapLabel_dataType;
    public TMP_Text mapLabel_time;

    public Image icon_ENSO;
    public Sprite elNino;
    public Sprite laNina;

    //SCALES
    public ScalesManager scalesManager;
    private int currentScaleIndex;

    private string currentMonthName;
    private string[] monthNames = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

    void Update()
    {
        if (windowManager.currentWindowIndex == 0)
        {
            // currentScaleIndex = histDataTypeController.dataTypeSelector.scaleIndex;

            if (histDataTypeController.histDataType.Equals("No Data"))
            {
                interactiveTexture.SetActive(false);
                // mapLabel_dataType.text = "";
                mapLabel_time.text = "";
                // currentScaleIndex = -1;
            }
            else
            {
                if (!histDataTypeController.histTimeType.Equals(""))
                {
                    interactiveTexture.SetActive(true);
                    // LoadHistoricalTexture();
                    // scalesManager.SetActiveScale(currentScaleIndex);
                }
            }
        }
    }

    // public void LoadInteractiveTexture(){

    // }

    public void LoadTextureFromData(string dataType, string timeType, int year, int month, string label = "")
    {
        string fileName = "";
        string fullPath = "";

        SetMonthName(month - 1); // Assuming month is 1-based

        if (timeType == "hist")
        {
            fileName = $"{year}_{month:D2}";
            fullPath = $"{basePath}/{dataType}/{timeType}/{year}/{fileName}";
        }
        else if (timeType == "contemp")
        {
            fileName = $"{month:D2}";
            fullPath = $"{basePath}/{dataType}/{timeType}/{fileName}";
        }
        else if (timeType == "monthlyAnomaly")
        {
            fileName = $"{year}_{month:D2}";
            fullPath = $"{basePath}/{dataType}/{timeType}/{year}/{fileName}";
        }

        Debug.Log($"Trying to load from path: {fullPath}");

        Texture2D mapTexture = Resources.Load<Texture2D>(fullPath);

        if (mapTexture != null)
        {
            storyTexture.texture = mapTexture;
            SetDataTypeLabel(dataType);
            SetTimeLabel(timeType, year);
            if (!string.IsNullOrEmpty(label)) mapLabel_dataType.text = label;
        }
        else
        {
            Debug.LogError($"Failed to load texture: {fullPath}");
        }
    }


    public void LoadHistoricalTexture()
    {
        string fileName = "";
        string fullPath = "";

        SetMonthName(histDataTypeController.month - 1);

        if (histDataTypeController.histTimeType.Equals("hist"))
        {
            fileName = $"{histDataTypeController.year}_{histDataTypeController.month:D2}";
            fullPath = $"{basePath}/{histDataTypeController.histDataType}/{histDataTypeController.histTimeType}/{histDataTypeController.year}/{fileName}";
            // Debug.Log(fullPath);
        }
        else if (histDataTypeController.histTimeType.Equals("contemp"))
        {
            fileName = $"{histDataTypeController.month:D2}";
            fullPath = $"{basePath}/{histDataTypeController.histDataType}/{histDataTypeController.histTimeType}/{fileName}";
        }
        // else if time period = anomalies, use this full path

        // Load the texture from the Resources folder
        Texture2D mapTexture = Resources.Load<Texture2D>(fullPath);

        if (mapTexture != null)
        {
            interactiveTexture.texture = mapTexture;

            // SetDataTypeLabel();
            // SetTimeLabel();
            // SetENSOIcon();
        }
        else
        {
            Debug.LogError($"Failed to load texture: {fullPath}");
        }
    }


    public void SetMonthName(int curMonth)
    {
        currentMonthName = monthNames[curMonth];
    }

    public void SetDataTypeLabel(string dataType)
    {
        mapLabel_dataType.text = dataType == "Rain" ? "Rainfall" :
                                 dataType == "Temp" ? "Temperature" : "";
    }

    public void SetTimeLabel(string timeType, int year)
    {
        if (timeType == "hist")
            mapLabel_time.text = $"{currentMonthName} {year}";
        else if (timeType == "contemp")
            mapLabel_time.text = $"{currentMonthName} Monthly Mean";
        else if (timeType == "monthlyAnomaly")
            mapLabel_time.text = $"{currentMonthName} {year} Anomaly";
    }

    // public void SetENSOIcon()
    // {
    //     if (histDataTypeController.ENSO.Equals("elnino"))
    //     {
    //         icon_ENSO.SetActive(true);
    //         icon_ENSO.sprite = elNino;
    //     }
    //     else if (histDataTypeController.ENSO.Equals("lanina"))
    //     {
    //         icon_ENSO.SetActive(true);
    //         icon_ENSO.sprite = laNina;
    //     }
    //     else
    //     {
    //         icon_ENSO.SetActive(false);
    //     }
    // }

    public void SwitchWindows()
    {
        // data
        // histDataType = "No Data";
        // fcpDataType = "No Data";
        histDataTypeController.histDataType = "No Data";
        // labels
        mapLabel_dataType.text = "";
        mapLabel_time.text = "";
        // mapLabel_scen_cont.text = "";
        // mapLabel_downscale.text = "";
        icon_ENSO.SetActive(false);
        // Sliders
        // ResetCircSlider(years);
        // ResetCircSlider(months);
        // CheckENSO(-1);
        // scales and buttons
        scalesManager.ResetScales();
        // fcpMap.ResetButtons();
    }

}
