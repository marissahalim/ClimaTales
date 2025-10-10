using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonthSlider : MonoBehaviour
{

    public Slider monthSlider;
    public float scaleFactor = 1.0f;
    public float widthValue = 160.0f;
    public float numTicks = 1.0f;
    public GameObject startTick;
    public GameObject endTick;
    public GameObject tickMark;
    public GameObject[] tickMarkArray;


    private RectTransform sliderRect;

    // Start is called before the first frame update
    void Start()
    {
        sliderRect = monthSlider.GetComponent<RectTransform>();
        //sets the slider's value to accept whole numbers only.
        monthSlider.wholeNumbers = true;
        ScaleSlider(scaleFactor);
        ChangeWidth(widthValue);

        // set the ticks
        //tickMark.SetActive(true);
        //Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent);
        Instantiate(tickMark, new Vector3(startTick.transform.position.x + 300.0f, startTick.transform.position.y, startTick.transform.position.z), Quaternion.identity, this.transform);
        // Instantiate(tickMark, new Vector3(startTick.transform.position.x * 2.0f, startTick.transform.position.y, startTick.transform.position.z), Quaternion.identity);
        // Debug.Log(startTick.transform.position.y);
        // Debug.Log(startTick.transform.position.z);
        Debug.Log(startTick.transform.position);
        Debug.Log(endTick.transform.position);
        // Debug.Log(tickMark.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ScaleSlider(float scaleNum)
    {
        float newXValue = sliderRect.localScale.x * scaleNum;
        float newYValue = sliderRect.localScale.y * scaleNum;
        float newZValue = sliderRect.localScale.z * scaleNum;
        sliderRect.localScale = new Vector3(newXValue, newYValue, newZValue);
    }

    public void ChangeWidth(float widthNum)
    {
        sliderRect.sizeDelta = new Vector2(widthNum, sliderRect.sizeDelta.y);
    }

    public void AddTicksEvenly(float nt)
    {
        float spaceBetween = widthValue / (nt + 1);
        // Debug.Log("" + spaceBetween);
        // Debug.Log(startTick.transform.position.x + spaceBetween);
        // Instantiate(prefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
        // Instantiate(tickMark, new Vector3(sliderRect.position.x, sliderRect.position.y, sliderRect.position.z), Quaternion.identity);
    }
}