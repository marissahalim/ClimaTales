using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YearsDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private int timeStart;
    [SerializeField] private int timeEnd;
    [SerializeField] private int timeIncrement;

    public List<GameObject> years_rainfall;
    public List<GameObject> years_temp;

    public GameObject currentRain;
    public GameObject currentTemp;

    public ChangeText_Months monthsSlider;

    // Start is called before the first frame update
    void Start()
    {
        Fill();
        // first rain & temp objects are 1990
        currentRain = years_rainfall[0];
        currentTemp = years_temp[0];
    }

    public void GetDropdownValue()
    {
        int currentDropdownIndex = dropdown.value;
        string currentDropdownValue = dropdown.options[currentDropdownIndex].text;
        // Debug.Log(currentDropdownValue);
    }

    private void Fill()
    {
        dropdown.ClearOptions();
        int diff = timeEnd - timeStart;
        for (int i = timeStart; i <= timeEnd; i += timeIncrement)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString(), null));
        }
    }

    public void LinkYears()
    {
        Debug.Log(dropdown.value);
        Debug.Log(years_rainfall[dropdown.value].tag);
        Debug.Log(dropdown.options[dropdown.value].text);
        if (years_rainfall[dropdown.value].tag == dropdown.options[dropdown.value].text)
        {
            //Debug.Log("match");
            for (int i = 0; i < years_rainfall.Count; i++)
            {
                if (i == dropdown.value)
                {
                    years_rainfall[i].SetActive(true);
                    years_temp[i].SetActive(true);
                    // set current rain & temp obj to the selected dropdown year
                    currentRain = years_rainfall[i];
                    currentTemp = years_temp[i];

                    // loop through children in years_rainfall[i] & years_temp[i] to set active the month that matches the dropdown value
                    for (int m = 0; m < 12; m++)
                    {
                        if (m == monthsSlider.GetCurrentSliderValue())
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

    public GameObject GetRainObj()
    {
        return currentRain;
    }

    public GameObject GetTempObj()
    {
        return currentTemp;
    }
}
