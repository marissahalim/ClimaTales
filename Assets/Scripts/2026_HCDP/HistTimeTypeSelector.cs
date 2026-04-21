using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistTimeTypeSelector : MonoBehaviour
{

    public Toggle monthly;
    public Toggle monthlyAvg;

    public string selectedTimeType;

    // Start is called before the first frame update
    void Start()
    {
        selectedTimeType = "hist";
        monthly.onValueChanged.AddListener(OnMonthlyToggled);
        monthlyAvg.onValueChanged.AddListener(OnMonthlyAvgToggled);
    }

    public void OnMonthlyToggled(bool isOn)
    {
        if (isOn)
        {
            selectedTimeType = "hist";
        } else
        {
            selectedTimeType = "";
        }
    }

    public void OnMonthlyAvgToggled(bool isOn)
    {
        if (isOn)
        {
            selectedTimeType = "contemp";
        } else
        {
            selectedTimeType = "";
        }
    }
}
