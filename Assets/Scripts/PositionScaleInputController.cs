using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using System.IO;
using TMPro;

public class PositionScaleInputController : MonoBehaviour
{
    public GameObject positionObj;
    public GameObject scaleObj;
    public GameObject positionLabel;
    public GameObject scaleLabel;
    private TMP_InputField positionInput;
    private TMP_InputField scaleInput;
    private TMP_Text currentPositon;
    private TMP_Text currentScale;

    public float positionSet;
    public float scaleSet;

    // Start is called before the first frame update
    void Awake()
    {
        positionSet = 10f;
        scaleSet = 0.1f;
        positionInput = positionObj.GetComponent<TMP_InputField>();
        scaleInput = scaleObj.GetComponent<TMP_InputField>();
        positionInput.text = positionSet.ToString();
        scaleInput.text = scaleSet.ToString();
        positionInput.onEndEdit.AddListener(delegate {ChangePositionInput(positionInput.text); });
        scaleInput.onEndEdit.AddListener(delegate {ChangeScaleInput(scaleInput.text); });

        currentPositon = positionLabel.GetComponent<TMP_Text>();
        currentScale = scaleLabel.GetComponent<TMP_Text>();
        currentPositon.text = "Current Value: " + positionSet.ToString();
        currentScale.text = "Current Value: " + scaleSet.ToString();
    }

    public void ChangePositionInput(string newValue)
    {
        if (newValue != null)
        {
            if (float.TryParse(newValue, out float newPosition))
            {
                positionSet = newPosition;
                currentPositon.text = "Current Value: " + positionSet.ToString();
                gameObject.GetComponent<TableObjectsController>().UpdatePositionValue(positionSet);
            }
        }
        else
        {
            positionSet = 0.0f;
            currentPositon.text = "Current Value: " + positionSet.ToString();
        }
    }

    public void ChangeScaleInput(string newValue)
    {
        if (newValue != null)
        {
            if (float.TryParse(newValue, out float newScale))
            {
                scaleSet = newScale;
                currentScale.text = "Current Value: " + scaleSet.ToString();
                gameObject.GetComponent<TableObjectsController>().UpdateScaleValue(scaleSet);
            }

        }
        else
        {
            scaleSet = 0.0f;
            currentScale.text = "Current Value: " + scaleSet.ToString();
        }
    }
}
