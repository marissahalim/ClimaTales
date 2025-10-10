using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.MUIP;

public class FutureDataController : MonoBehaviour
{

    public WindowManager windowManager;

    public LoadTable futureMapLoader;

    public FCPButtonManager futureBtns;                 // scenario buttons

    public HistDataTypeSelector dataTypeSelector;       // rainfall/temperature buttons

    private int prevSelectedButtonIndex = -1;
    private string prevSelectedMode = "";

    [Header("Future Path Info")]
    public string futureDataType;
    public string time;
    public string downscale;
    public string content;
    public string scenario;

    // Update is called once per frame
    void Update()
    {
        if (windowManager.currentWindowIndex == 1)
        {
            int currentIndex = futureBtns.selectedButtonIndex;
            string currentMode = dataTypeSelector.selectedMode;

            if (currentIndex != prevSelectedButtonIndex)
            {
                // Debug.Log("Selected Button Index changed: " + currentIndex);
                prevSelectedButtonIndex = currentIndex;
            }

            if (currentMode != prevSelectedMode)
            {
                // Debug.Log("Selected Mode changed: " + currentMode);
                prevSelectedMode = currentMode;
            }

            SetFCPPathInfo(currentIndex);
            SetFutureDataType(currentMode);
        }
    }

    public void SetFCPPathInfo(int scenarioNumber)
    {
        switch (scenarioNumber)
        {
            case 0:
                // Debug.Log("Present Dynamical");
                // fullPath = $"{basePath}/{dataType}/{"Dyn"}/{"Present"}";
                time = "Present";
                downscale = "Dyn";
                content = "";
                scenario = "";
                break;
            case 1:
                // Debug.Log("Future Dynamical Best case Value");
                // fullPath = $"{basePath}/{dataType}/{"Dyn"}/{"End"}_{"V"}_{"4.5"}";
                time = "End";
                downscale = "Dyn";
                content = "V";
                scenario = "4.5";
                break;
            case 2:
                // Debug.Log("Future Dynamical Worst case Value");
                // fullPath = $"{basePath}/{dataType}/{"Dyn"}/{"End"}_{"V"}_{"8.5"}";
                time = "End";
                downscale = "Dyn";
                content = "V";
                scenario = "8.5";
                break;
            case 3:
                // Debug.Log("Future Dynamical Best case Abs Change");
                // fullPath = $"{basePath}/{dataType}/{"Dyn"}/{"End"}_{"AC"}_{"4.5"}";
                time = "End";
                downscale = "Dyn";
                content = "AC";
                scenario = "4.5";
                break;
            case 4:
                // Debug.Log("Future Dynamical Worst case Abs Change");
                // fullPath = $"{basePath}/{dataType}/{"Dyn"}/{"End"}_{"AC"}_{"8.5"}";
                time = "End";
                downscale = "Dyn";
                content = "AC";
                scenario = "8.5";
                break;
            case 5:
                // Debug.Log("Present Statistical");
                // fullPath = $"{basePath}/{dataType}/{"Stat"}/{"Present"}";
                time = "Present";
                downscale = "Stat";
                content = "";
                scenario = "";
                break;
            case 6:
                // Debug.Log("Future Statistical Best case Value");
                // fullPath = $"{basePath}/{dataType}/{"Stat"}/{"End"}_{"V"}_{"4.5"}";
                time = "End";
                downscale = "Stat";
                content = "V";
                scenario = "4.5";
                break;
            case 7:
                // Debug.Log("Future Statistical Worst case Value");
                // fullPath = $"{basePath}/{dataType}/{"Stat"}/{"End"}_{"V"}_{"8.5"}";
                time = "End";
                downscale = "Stat";
                content = "V";
                scenario = "8.5";
                break;
            case 8:
                // Debug.Log("Future Statistical Best case Abs Change");
                // fullPath = $"{basePath}/{dataType}/{"Stat"}/{"End"}_{"AC"}_{"4.5"}";
                time = "End";
                downscale = "Stat";
                content = "AC";
                scenario = "4.5";
                break;
            case 9:
                // Debug.Log("Future Statistical Worst case Abs Change");
                // fullPath = $"{basePath}/{dataType}/{"Stat"}/{"End"}_{"AC"}_{"8.5"}";
                time = "End";
                downscale = "Stat";
                content = "AC";
                scenario = "8.5";
                break;
            default:
                break;
        }
    }

    public void SetFutureDataType(string selectedDataType)
    {
        switch (selectedDataType)
        {
            case "Rain":
                futureDataType = "FCP_Rain";
                // currentDataTypeName = "Annual Rainfall (in)";
                break;
            case "Temp":
                futureDataType = "FCP_Temp";
                // currentDataTypeName = "Annual Mean Temperature (°F)";

                break;
            case "No Data":
                futureDataType = "No Data";
                // currentDataTypeName = "No data showing";
                break;
            default:
                break;
        }
    }

    public void ResetUI()
    {
        // data type
        dataTypeSelector.SetStoryDataType("No Data");
        futureDataType = "No Data";

        // Clear path state
        time = "";
        downscale = "";
        content = "";
        scenario = "";

        // Buttons
        futureBtns.ResetButtons();

        // scales
        dataTypeSelector.scalesManager.ResetScales();

        futureMapLoader?.ResetLabelsAndMap();

        Debug.Log("Future is reset");
    }
}
