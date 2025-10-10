using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSlider : MonoBehaviour
{
    public Slider monthSlider;
    public RectTransform background;
    public RectTransform fill;
    public Transform fillObject;
    public float numTicks = 1.0f;

    public GameObject tick;

    private RectTransform sliderRect;

    // Start is called before the first frame update
    void Start()
    {
        sliderRect = monthSlider.GetComponent<RectTransform>();
        Debug.Log(sliderRect.transform.position);

        monthSlider.minValue = 1;
        monthSlider.maxValue = numTicks;

        //get the origin 
        GameObject temp = Instantiate(tick, new Vector3(background.transform.position.x - (background.sizeDelta.x/2) + 20, fillObject.position.y, fillObject.position.z), Quaternion.identity, this.transform);
        Debug.Log(temp.transform.position);
        GameObject temp2 = Instantiate(tick, new Vector3(background.transform.position.x + (background.sizeDelta.x/2) - 20, fillObject.position.y, fillObject.position.z), Quaternion.identity, this.transform);
        Debug.Log(temp2.transform.position);
    }
}
