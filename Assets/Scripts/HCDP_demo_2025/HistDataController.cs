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

        // Time Type
        monthly.onValueChanged.AddListener((isOn) =>
                {
                    if (isOn) OnToggleSelected(monthly);
                });
        monthlyAvg.onValueChanged.AddListener((isOn) =>
                {
                    if (isOn) OnToggleSelected(monthlyAvg);
                });
        monthlyAnomaly.onValueChanged.AddListener((isOn) =>
                {
                    if (isOn) OnToggleSelected(monthlyAnomaly);
                });

        // Sliders
        month = (int)months.currentValue + 1;
        originalMonthScale = months.transform.localScale;
        originalMonthPosition = months.transform.localPosition;
        year = (int)years.currentValue;

    }

    void Update()
    {
        histDataType = dataTypeSelector.selectedMode;
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

    void OnToggleSelected(Toggle selected)
    {
        if (selected != monthly) monthly.isOn = false;
        if (selected != monthlyAvg) monthlyAvg.isOn = false;
        if (selected != monthlyAnomaly) monthlyAnomaly.isOn = false;

        if (selected == monthly)
        {
            years.SetActive(true);
            months.transform.localScale = originalMonthScale;
            months.transform.localPosition = originalMonthPosition;
            histTimeType = "hist";
        }
        else if (selected == monthlyAvg)
        {
            years.SetActive(false);
            months.transform.localScale = originalMonthScale * 1.5f;
            months.transform.localPosition = originalMonthPosition + new Vector3(0.0f, 70f, 0.0f);
            histTimeType = "contemp";
        }
        else if (selected == monthlyAnomaly)
        {
            years.SetActive(true);
            months.transform.localScale = originalMonthScale;
            months.transform.localPosition = originalMonthPosition;
            histTimeType = "monthlyAnomaly";
        }
    }

    public void SetToggle(string time)
    {
        if (time == "hist")
        {
            monthly.isOn = true;
        }
        else if (time == "contemp")
        {
            monthlyAvg.isOn = true;
        }
    }

    public void ResetUI()
    {
        // data type
        dataTypeSelector.SetStoryDataType("No Data");
        dataTypeSelector.scaleIndex = -1;
        // time type
        monthly.isOn = false;
        monthlyAvg.isOn = false;
        years.SetActive(true);
        months.transform.localScale = originalMonthScale;
        months.transform.localPosition = originalMonthPosition;
        histTimeType = "";
        // knobs
        years.SetValue(years.minValue);
        months.SetValue(months.minValue);
        // ENSO
        ENSO = "";
        // scales
        dataTypeSelector.scalesManager.ResetScales();

        mapLoader?.ResetLabelsAndMap();

        Debug.Log("I am reset");
    }

}
