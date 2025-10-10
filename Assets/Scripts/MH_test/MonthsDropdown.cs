using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonthsDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    public ChangeText_Years yearSlider;

    private string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

    private GameObject currentRainYearObj;
    private GameObject currentTempYearObj;

    // Start is called before the first frame update
    void Start()
    {
        Fill();
    }

    private void Fill()
    {
        dropdown.ClearOptions();
        for (int i = 0; i < months.Length; i++)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(months[i], null));
        }
    }

    public void GetDropdownValue()
    {
        int currentDropdownIndex = dropdown.value;
        //string currentDropdownValue = dropdown.options[currentDropdownIndex].text;
        Debug.Log(currentDropdownIndex);

        currentRainYearObj = yearSlider.GetRainYearObj();
        currentTempYearObj = yearSlider.GetTempYearObj();

        for (int i = 0; i < months.Length; i++)
        {
            if (i == dropdown.value)
            {
                currentRainYearObj.transform.GetChild(i).gameObject.SetActive(true);
                currentTempYearObj.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                currentRainYearObj.transform.GetChild(i).gameObject.SetActive(false);
                currentTempYearObj.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }

    public float GetCurrentDropdownValue()
    {
        return dropdown.value;
    }

}
