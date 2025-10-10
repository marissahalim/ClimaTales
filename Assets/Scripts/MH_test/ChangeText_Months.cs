using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeText_Months : MonoBehaviour
{
    [SerializeField] private Slider mainSlider;
    [SerializeField] private TMP_Text currentMonth;

    public YearsDropdown yearsDropdown;

    private string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

    private void Start()
    {
        
    }

    public void MonthChangeCheck()
    {
        Debug.Log(mainSlider.value);
        currentMonth.SetText(months[(int)mainSlider.value]);
        //Debug.Log(yearsDropdown.GetRainObj());

        for (int i = 0; i < months.Length; i++)
        {
            if (i == mainSlider.value)
            {
                yearsDropdown.GetRainObj().transform.GetChild(i).gameObject.SetActive(true);
                yearsDropdown.GetTempObj().transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                yearsDropdown.GetRainObj().transform.GetChild(i).gameObject.SetActive(false);
                yearsDropdown.GetTempObj().transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public float GetCurrentSliderValue()
    {
        return mainSlider.value;
    }

}
