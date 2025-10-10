using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeText_Years : MonoBehaviour
{
    [SerializeField] private Slider mainSlider;
    [SerializeField] private TMP_Text currentYear;

    public List<GameObject> years_rainfall;
    public List<GameObject> years_temp;

    public MonthsDropdown months;

    public GameObject currentRainToDisplay;
    public GameObject currentTempToDisplay;

    private float sum;

    private void Start()
    {
        currentRainToDisplay = years_rainfall[0];
        currentTempToDisplay = years_temp[0];
    }

    public void YearChangeCheck()
    {
        sum = 1990 + mainSlider.value;
        //Debug.Log(sum);
        currentYear.SetText(sum.ToString());
    }

    public void ValueChangeCheck()
    {
        Debug.Log(mainSlider.value);
        Debug.Log(years_rainfall[(int)mainSlider.value].tag);
    }

    public void DisplayYear()
    {
        if (years_rainfall[(int)mainSlider.value].tag == currentYear.text)
        {
            Debug.Log(months.GetCurrentDropdownValue());
            for (int i = 0; i < years_rainfall.Count; i++)
            {
                if (i == mainSlider.value)
                {
                    years_rainfall[i].SetActive(true);
                    currentRainToDisplay = years_rainfall[i];
                    years_temp[i].SetActive(true);
                    currentTempToDisplay = years_temp[i];

                    // loop through children in years_rainfall[i] & years_temp[i] to set active the month that matches the dropdown value
                    for (int m = 0; m < 12; m++)
                    {
                        if (m == months.GetCurrentDropdownValue())
                        {
                            years_rainfall[i].transform.GetChild(m).gameObject.SetActive(true);
                            years_temp[i].transform.GetChild(m).gameObject.SetActive(true);
                        }
                        else
                        {
                            years_rainfall[i].transform.GetChild(m).gameObject.SetActive(false);
                            years_temp[i].transform.GetChild(m).gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    years_rainfall[i].SetActive(false);
                    years_temp[i].SetActive(false);
                }
            }
        }
    }

    public GameObject GetRainYearObj()
    {
        return currentRainToDisplay;
    }

    public GameObject GetTempYearObj()
    {
        return currentTempToDisplay;
    }



}
