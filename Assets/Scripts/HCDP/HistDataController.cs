using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// This script is to log the values of the Interactive UI elements for the interactable map section (not stories)
// and store the data to set the path name to load a map texture
public class HistDataController : MonoBehaviour
{

    [Header("UI elements")]
    public HistDataTypeSelector dataTypeSelector;
    public HistTimeTypeSelector timeTypeSelector;
    public KnobController years;
    public KnobController months;
    public Toggle monthly;
    public Toggle monthlyAvg;
    public Toggle monthlyAnomaly;

    public LoadTable mapLoader;

    private enum DataType { Rainfall, Temperature }
    private DataType selectedType;

    [Header("Path Info")]
    public string histDataType;
    public string histTimeType;
    public int month;
    public int year;

    [Header("El Nino vs La Nina")]
    public string ENSO;

    [Header("Knobs")]
    private int prevMonth = 0;
    private Vector3 originalMonthScale;
    private Vector3 originalMonthPosition;

    void Start()
    {
        // Data type
        histDataType = dataTypeSelector.selectedMode;

        // Sliders
        month = (int)months.currentValue + 1;
        originalMonthScale = months.transform.localScale;
        originalMonthPosition = months.transform.localPosition;
        year = (int)years.currentValue;

    }

    void Update()
    {
        histDataType = dataTypeSelector.selectedMode;
        // histTimeType = timeTypeSelector.selectedTimeType;

        SetTimeType(timeTypeSelector.selectedTimeType);
    }

    public void SetMonth()
    {
        int currentMonth = (int)months.currentValue;
        int currentYear = (int)years.currentValue;

        // Wrap forward: Dec → Jan
        if (prevMonth == 11 && currentMonth == 0 && currentYear < 2024)
        {
            currentYear += 1;
            years.SetValue(currentYear);
        }
        // Wrap backward: Jan → Dec
        else if (prevMonth == 0 && currentMonth == 11 && currentYear > 1990)
        {
            currentYear -= 1;
            years.SetValue(currentYear);
        }

        // Update month and year values
        prevMonth = currentMonth;
        month = currentMonth + 1; // Store as 1-based (Jan = 1)
        year = currentYear;

        SetENSO();

    }

    public void SetYear()
    {
        year = (int)years.currentValue;
        SetENSO();
    }

    public void SetENSO()
    {
        ENSO = ENSOHelper.GetENSOPhase(year, month);
    }

    public void SetTimeType(string timeType)
    {
        histTimeType = timeType;

        if (histTimeType == "hist")
        {
            monthly.isOn = true;
            monthlyAvg.isOn = false;
            years.SetActive(true);
            months.SetActive(true);
            months.transform.localScale = originalMonthScale;
            months.transform.localPosition = originalMonthPosition;
        }
        else if (histTimeType == "contemp")
        {
            monthlyAvg.isOn = true;
            monthly.isOn = false;
            years.SetActive(false);
            months.SetActive(true);
            months.transform.localScale = originalMonthScale * 1.5f;
            months.transform.localPosition = originalMonthPosition + new Vector3(0.0f, 70f, 0.0f);
        }
    }

    public void ResetUI()
    {
        // data type
        dataTypeSelector.SetStoryDataType("No Data");
        dataTypeSelector.scaleIndex = -1;
        // time type
        SetTimeType("hist");
        // knobs
        years.SetActive(true);
        months.transform.localScale = originalMonthScale;
        months.transform.localPosition = originalMonthPosition;
        years.SetValue(years.minValue);
        months.SetValue(months.minValue);
        // ENSO
        ENSO = "";
        // scales
        dataTypeSelector.scalesManager.ResetScales();

        mapLoader?.ResetLabelsAndMap();

        // Debug.Log("I am reset");
    }

}
