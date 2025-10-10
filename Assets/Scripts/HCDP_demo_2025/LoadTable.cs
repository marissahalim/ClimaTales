using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Michsky.MUIP;

public class LoadTable : MonoBehaviour
{

    public WindowManager windowManager;
    public HistStateManager histStateManager;

    [Header("Map Texture")]
    public RawImage mapTexture;
    public RawImage futureMapTexture;
    [SerializeField] private string basePath = "Textures"; // Base path for your textures

    [Header("Interactive Data Controller")]
    public HistDataController dataController;

    [Header("Future Data Controller")]
    public FutureDataController futureDataController;

    [Header("Scale")]
    public ScalesManager scalesManager;

    [Header("ENSO icon")]
    public Image iconENSO;
    public Sprite elNino;
    public Sprite laNina;

    [Header("Historical Labels - Interactive")]
    public TMP_Text dataTypeLabel;
    public TMP_Text timeTypeLabel;

    [Header("FCP Labels")]
    public TMP_Text futureDataTypeLabel;
    public TMP_Text futureTimeTypeLabel;
    public TMP_Text scenarioContentLabel;
    public TMP_Text downscaleLabel;

    private string currentMonthName;
    private string[] monthNames = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    private bool isStoryMode = false;


    void Update()
    {
        if (isStoryMode)
        {
            // Let PrebuiltStory control the visuals
            return;
        }

        if (windowManager.currentWindowIndex == 0)
        {
            // reset Future stuff
            futureMapTexture.SetActive(false);
            scenarioContentLabel.text = "";
            downscaleLabel.text = "";

            if (histStateManager.currentState == HistState.Interactive)
            {
                if (dataController.histDataType.Equals("No Data"))
                {
                    // only Base Map should show 
                    mapTexture.SetActive(false);
                    scalesManager.ResetScales();
                    iconENSO.SetActive(false);
                    dataTypeLabel.text = "";
                    timeTypeLabel.text = "";
                }
                else    // histDataType either equals Rain or Temp
                {
                    if (!dataController.histTimeType.Equals(""))
                    {
                        mapTexture.SetActive(true);
                        LoadMapTexture(dataController.histDataType,
                                       dataController.histTimeType,
                                       dataController.year,
                                       dataController.month,
                                       dataController.ENSO);
                    }
                }
            }
            else if (histStateManager.currentState == HistState.Storytelling)
            {
                // scalesManager.ResetScales();
                dataTypeLabel.text = "";
                timeTypeLabel.text = "";
            }
        }
        else // window is now future
        {
            histStateManager.currentState = HistState.Interactive;

            // dataController.ResetUI();
            if (futureDataController.futureDataType.Equals("No Data"))
            {
                // only Base Map should show 
                futureMapTexture.SetActive(false);
                scalesManager.ResetScales();
                iconENSO.SetActive(false);
            }
            else
            {
                if (futureDataController.time.Equals("Present") || futureDataController.time.Equals("End"))
                {
                    futureMapTexture.SetActive(true);
                    LoadFCPTexture(futureDataController.futureDataType,
                                    futureDataController.time,
                                    futureDataController.downscale,
                                    futureDataController.content,
                                    futureDataController.scenario);
                }
            }
        }
    }

    // for Historical window
    public void LoadMapTexture(string dataType, string timeType, int year, int month, string enso)
    {

        string fileName = "";
        string fullPath = "";

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

        Texture2D mt = Resources.Load<Texture2D>(fullPath);

        // Debug.Log($"[Story Mode] Loading texture: {dataType}, {timeType}, {year}_{month:D2}, ENSO: {enso}");

        if (mt != null)
        {
            mapTexture.texture = mt;
            mapTexture.gameObject.SetActive(true);
            // Debug.Log("[LoadTable] Texture loaded successfully.");

            if (histStateManager.currentState == HistState.Interactive)
            {
                // dataTypeLabel
                SetDataTypeLabel(dataType);

                // timeTypeLabel
                SetTimeTypeLabel(timeType, year, month);
            }

            // ENSO icon
            SetENSOIcon(enso, timeType);

            // scale manager
            SetScale(dataType);
        }
        else
        {
            Debug.LogError($"Failed to load texture: {fullPath}");
        }
    }

    // for Future window
    public void LoadFCPTexture(string dataType, string time, string ds, string cont, string sce)
    {
        string fullPath = "";

        if (time == "Present")
        {
            fullPath = $"{basePath}/{dataType}/{ds}/{time}"; ;
        }
        else if (time == "End")
        {
            fullPath = $"{basePath}/{dataType}/{ds}/{time}_{cont}_{sce}";
        }

        Texture2D mt = Resources.Load<Texture2D>(fullPath);

        if (mt != null)
        {
            futureMapTexture.texture = mt;
            futureMapTexture.gameObject.SetActive(true);
            // Debug.Log("[LoadTable] Texture loaded successfully.");

            // datatype label
            SetFutureDataTypeLabel(dataType);

            // time label
            SetFutureTimeTypeLabel(time);

            // downscaling label
            SetDownscalingMethod(ds);

            // scenario content label
            SetScenarioContent(time, sce, cont);

            // scales
            SetFutureScale(dataType, time, cont);
        }
        else
        {
            Debug.LogError($"Failed to load texture: {fullPath}");
        }

    }

    public void SetDataTypeLabel(string dataType)
    {
        dataTypeLabel.text = dataType == "Rain" ? "Rainfall" :
                             dataType == "Temp" ? "Temperature" : "";
    }

    public void SetFutureDataTypeLabel(string dataType)
    {
        futureDataTypeLabel.text = dataType == "FCP_Rain" ? "Annual Rainfall" :
                             dataType == "FCP_Temp" ? "Annual Mean Temperature" : "";
    }

    public void SetTimeTypeLabel(string timeType, int year, int month)
    {
        if (timeType.Equals("hist"))
        {
            SetMonthName(month);
            timeTypeLabel.text = currentMonthName + $" {year}";
        }
        else if (timeType.Equals("contemp"))
        {
            SetMonthName(month);
            timeTypeLabel.text = $"{currentMonthName} Monthly Mean";
        }
    }

    public void SetFutureTimeTypeLabel(string time)
    {
        futureTimeTypeLabel.text = time == "Present" ? "Present Day" :
                             time == "End" ? "Late-century (2070-2099)" : "";
    }

    public void SetScenarioContent(string time, string scenario, string content)
    {
        string currentScenario = "";
        string currentContent = "";

        if (scenario.Equals("4.5"))
        {
            currentScenario = "Major Emission Reduction (RCP 4.5)";
        }
        else if (scenario.Equals("8.5"))
        {
            currentScenario = "Modest Emission Reduction (RCP 8.5)";
        }

        if (content.Equals("V"))
        {
            currentContent = "Value";
        }
        else if (content.Equals("AC"))
        {
            currentContent = "Absolute change";
        }

        if (time.Equals("End"))
        {
            scenarioContentLabel.text = currentContent + $" of {currentScenario}";
        }
        else
        {
            scenarioContentLabel.text = "";
        }
    }

    public void SetDownscalingMethod(string downscale)
    {
        downscaleLabel.text = downscale == "Dyn" ? "Dynamic Downscaling" :
                     downscale == "Stat" ? "Statistical Downscaling" : "";
    }

    public void SetENSOIcon(string enso, string timeType)
    {
        if (enso == "elnino" && timeType.Equals("hist"))
        {
            iconENSO.SetActive(true);
            iconENSO.sprite = elNino;
        }
        else if (enso == "lanina" && timeType.Equals("hist"))
        {
            iconENSO.SetActive(true);
            iconENSO.sprite = laNina;
        }
        else
        {
            iconENSO.SetActive(false);
            iconENSO.sprite = null;
        }
    }

    public void SetScale(string dataType)
    {
        if (dataType.Equals("No Data"))
        {
            scalesManager.ResetScales();
        }
        else if (dataType.Equals("Rain"))
        {
            scalesManager.SetActiveScale(2);
        }
        else if (dataType.Equals("Temp"))
        {
            scalesManager.SetActiveScale(0);
        }
    }

    public void SetFutureScale(string dataType, string time, string cont)
    {
        if (dataType.Equals("No Data"))
        {
            scalesManager.ResetScales();
        }
        else if (dataType.Equals("FCP_Rain"))
        {
            if (time.Equals("Present"))
            {
                scalesManager.SetActiveScale(3);
            }
            else if (time.Equals("End"))
            {
                if (cont.Equals("V"))
                {
                    scalesManager.SetActiveScale(3);
                }
                else if (cont.Equals("AC"))
                {
                    scalesManager.SetActiveScale(4);
                }
            }
        }
        else if (dataType.Equals("FCP_Temp"))
        {
            if (time.Equals("Present"))
            {
                scalesManager.SetActiveScale(0);
            }
            else if (time.Equals("End"))
            {
                if (cont.Equals("V"))
                {
                    scalesManager.SetActiveScale(0);
                }
                else if (cont.Equals("AC"))
                {
                    scalesManager.SetActiveScale(1);
                }
            }
        }
    }

    public void SetMonthName(int curMonth)
    {
        currentMonthName = monthNames[curMonth - 1];
    }

    public void SetStoryMode(bool isStory)
    {
        isStoryMode = isStory;
    }

    public void ResetLabelsAndMap()
    {
        mapTexture.gameObject.SetActive(false);
        futureMapTexture.gameObject.SetActive(false);
        iconENSO.SetActive(false);
        dataTypeLabel.text = "";
        timeTypeLabel.text = "";
        scenarioContentLabel.text = "";
        downscaleLabel.text = "";
        scalesManager.ResetScales();
    }

}