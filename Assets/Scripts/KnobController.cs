using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class KnobController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    public enum KnobValueTypes { Ticks, Continuous, Labels };

    // Content

    public KnobValueTypes valueType = KnobValueTypes.Continuous;

    // Resources
    public Image background;
    public Image backgroundImage;
    public Image sliderImage;
    public Transform indicatorPivot;
    public TextMeshProUGUI valueText;

    // Settings
    public float minValue = 0;
    public float maxValue = 100;
    public float tickSize = 1f;
    [Range(0, 8)] public int decimals;
    public float currentValue = 50.0f;

    // MH Test code
    public bool printValue;
    public static bool IsAnyKnobBeingUsed = false;
    public static float LastInteractionTime = -999f; // Global timestamp

    public bool isPercent;
    public string[] stringValues;
    public bool flip = false;

    public float startAngle = 0.25f;
    public float endAngle = 0.75f;
    public bool isInput = true;

    // Events
    [System.Serializable]
    public class SliderEvent : UnityEvent<float> { }
    [SerializeField]
    public SliderEvent onValueChanged = new SliderEvent();
    public UnityEvent onPointerEnter;
    public UnityEvent onPointerExit;

    [Header("Appearance")]
    public Color knobColor = Color.red;

    private GraphicRaycaster graphicRaycaster;
    private RectTransform hitRectTransform;
    private bool isPointerDown;
    public float currentAngle = 0;
    private float currentAngleOnPointerDown;
    private float valueDisplayPrecision;


    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    public float SliderAngle
    {
        get { return currentAngle; }
        set { currentAngle = Mathf.Clamp(value, 0.0f, 360.0f); }
    }

    public void SetColor(Color c)
    {
        knobColor = c;
        sliderImage.color = c;
        indicatorPivot.GetChild(0).gameObject.GetComponent<Image>().color = c;
    }


    public float angleFromValue(float value)
    {
        if (valueType == KnobValueTypes.Labels)
        {
            return map(value, minValue, maxValue + 1, startAngle, endAngle) * 360.0f;

        }
        else
            return map(value, minValue, maxValue, startAngle, endAngle) * 360.0f;
    }

    public float valuefromAngle(float angle)
    {

        if (valueType == KnobValueTypes.Continuous)
        {
            return (long)(map(angle / 360, startAngle, endAngle, minValue, maxValue) * valueDisplayPrecision) / valueDisplayPrecision;
        }
        else if (valueType == KnobValueTypes.Ticks)
        {
            float midValue = map(angle / 360, startAngle, endAngle, minValue, maxValue);
            // round to closest tick value
            return Mathf.Round(midValue / tickSize) * tickSize;

        }
        else if (valueType == KnobValueTypes.Labels)
        {
            float segment = (endAngle - startAngle) * 360 / stringValues.Length;
            int i;
            for (i = stringValues.Length - 1; i >= 0; i--)
            {
                if ((Mathf.Round(angle) >= startAngle * 360 + segment * i) && (Mathf.Round(angle) <= startAngle * 360 + segment * (i + 1)))
                {
                    break;
                }
            }
            return i;
        }
        else return -1;
    }

    private void Awake()
    {
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();

        if (graphicRaycaster == null)
            Debug.LogWarning("<b>[Radial Slider]</b> Could not find GraphicRaycaster component in parent.", this);
    }

    private void Start()
    {
        SetColor(knobColor);
        backgroundImage.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, -(360 * startAngle)));
        backgroundImage.fillAmount = endAngle - startAngle;
        sliderImage.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, -(360 * startAngle)));
        if (valueType == KnobValueTypes.Continuous)
        {
            valueDisplayPrecision = Mathf.Pow(10, decimals);
        }
        if (valueType == KnobValueTypes.Labels)
        {
            if (stringValues.Length == 0) stringValues = new string[] { "No Values" };
            minValue = 0;
            maxValue = stringValues.Length - 1;
        }
        if (currentValue < minValue) currentValue = minValue;
        if (currentValue > maxValue) currentValue = maxValue;

        if (flip) background.rectTransform.localScale = new Vector3(-1, 1, 1);
        else background.rectTransform.localScale = new Vector3(1, 1, 1);

        if (!isInput) indicatorPivot.gameObject.SetActive(false);

        onValueChanged.Invoke(currentValue);
        SliderAngle = angleFromValue(currentValue);
        UpdateUI();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("Im being held down");
        hitRectTransform = eventData.pointerCurrentRaycast.gameObject.GetComponent<RectTransform>();
        isPointerDown = true;
        IsAnyKnobBeingUsed = true;
        LastInteractionTime = Time.time;
        currentAngleOnPointerDown = SliderAngle;
        HandleSliderMouseInput(eventData, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Debug.Log("I've been let go");
        hitRectTransform = null;
        isPointerDown = false;
        IsAnyKnobBeingUsed = false;
        LastInteractionTime = Time.time;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // OLD CODE
        // if (SliderAngle >= 360.0f * startAngle && SliderAngle <= 360.0f * endAngle)
        // {
        //     Debug.Log("You've gone past the threshold");
        //     HandleSliderMouseInput(eventData, false);
        // }

        // Debug.Log("You've gone past the threshold");
        LastInteractionTime = Time.time;
        HandleSliderMouseInput(eventData, false);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit.Invoke();
    }

    public void UpdateUI()
    {
        if (SliderAngle >= 360.0f * startAngle && SliderAngle <= 360.0f * endAngle)
        {
            // MH test code
            if (isPointerDown)
            {
                if (printValue)
                {
                    Debug.Log(SliderAngle);
                }
            }

            float adjustedAngle = SliderAngle;
            currentValue = valuefromAngle(SliderAngle);

            if (valueType == KnobValueTypes.Ticks || valueType == KnobValueTypes.Labels)
            {
                adjustedAngle = angleFromValue(currentValue);
            }

            if (valueType == KnobValueTypes.Labels)
            {
                float offsetAngle = (endAngle - startAngle) * 360 / stringValues.Length / 2;
                if (isInput) indicatorPivot.transform.localEulerAngles = new Vector3(180.0f, 0.0f, adjustedAngle + offsetAngle);
                sliderImage.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, ((flip) ? 1 : -1) * adjustedAngle));
                sliderImage.fillAmount = (endAngle - startAngle) / stringValues.Length;
                valueText.text = stringValues[(int)currentValue];
            }
            else
            {
                if (isInput) indicatorPivot.transform.localEulerAngles = new Vector3(180.0f, 0.0f, adjustedAngle);
                sliderImage.fillAmount = (adjustedAngle / 360.0f) - startAngle;
                valueText.text = string.Format("{0}{1}", currentValue, isPercent ? "%" : "");
            }
        }
    }

    private bool HasValueChanged()
    {
        return SliderAngle != currentAngleOnPointerDown;
    }

    private void HandleSliderMouseInput(PointerEventData eventData, bool allowValueWrap)
    {
        if (!isPointerDown || !isInput)
            return;

        float currentValueBefore = currentValue;

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(hitRectTransform, eventData.position, eventData.pressEventCamera, out localPos);

        float newAngle = Mathf.Atan2(-localPos.y, localPos.x) * Mathf.Rad2Deg + 180f;

        // OLD CODE
        // SliderAngle = newAngle;
        // UpdateUI();

        // // updates the slider text
        // if (currentValue != currentValueBefore)
        //     onValueChanged.Invoke(currentValue);

        // MH test code
        // Map to the valid range of the knob
        float mappedAngle = Mathf.Lerp(startAngle * 360f, endAngle * 360f,
                                       Mathf.Clamp01((newAngle - startAngle * 360f) / ((endAngle - startAngle) * 360f)));

        // Update slider rotation only if within the allowed range
        if (newAngle >= startAngle * 360f && newAngle <= endAngle * 360f)
        {
            SliderAngle = mappedAngle;
            UpdateUI();
            onValueChanged.Invoke(currentValue);
        }

    }

    public void SetValue(float v)
    {
        currentValue = v;
        if (currentValue < minValue) currentValue = minValue;
        if (currentValue > maxValue) currentValue = maxValue;
        SliderAngle = angleFromValue(currentValue);
        UpdateUI();
    }
    public void IncreaseValue()
    {
        if (valueType == KnobValueTypes.Labels)
        {
            currentValue = currentValue + 1;
        }
        if (valueType == KnobValueTypes.Ticks)
        {
            currentValue = currentValue + tickSize;
        }
        if (currentValue < minValue) currentValue = minValue;
        if (currentValue > maxValue) currentValue = maxValue;
        SliderAngle = angleFromValue(currentValue);
        UpdateUI();

    }
    public void DecreaseValue()
    {
        if (valueType == KnobValueTypes.Labels)
        {
            currentValue--;
        }
        if (valueType == KnobValueTypes.Ticks)
        {
            currentValue = currentValue - tickSize;
        }
        if (currentValue < minValue) currentValue = minValue;
        if (currentValue > maxValue) currentValue = maxValue;
        SliderAngle = angleFromValue(currentValue);
        UpdateUI();

    }

    public void IncreaseBy(float inc)
    {
        currentValue = currentValue + inc;
        if (currentValue < minValue) currentValue = minValue;
        if (currentValue > maxValue) currentValue = maxValue;
        SliderAngle = angleFromValue(currentValue);
        UpdateUI();
    }
    public void DecreaseBy(float dec)
    {
        currentValue = currentValue - dec;
        if (currentValue < minValue) currentValue = minValue;
        if (currentValue > maxValue) currentValue = maxValue;
        SliderAngle = angleFromValue(currentValue);
        UpdateUI();

    }

    // MH CODE
    public void ResetLabelsUI()
    {
        float adjustedAngle = SliderAngle;

        if (valueType == KnobValueTypes.Ticks || valueType == KnobValueTypes.Labels)
        {
            adjustedAngle = angleFromValue(0);
        }

        if (valueType == KnobValueTypes.Labels)
        {
            float offsetAngle = (endAngle - startAngle) * 360 / stringValues.Length / 2;
            if (isInput) indicatorPivot.transform.localEulerAngles = new Vector3(180.0f, 0.0f, adjustedAngle + offsetAngle);
            sliderImage.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, ((flip) ? 1 : -1) * adjustedAngle));
            sliderImage.fillAmount = (endAngle - startAngle) / stringValues.Length;
            valueText.text = stringValues[0];
        }
        else
        {
            if (isInput) indicatorPivot.transform.localEulerAngles = new Vector3(180.0f, 0.0f, adjustedAngle);
            sliderImage.fillAmount = (adjustedAngle / 360.0f) - startAngle;
            valueText.text = string.Format("{0}{1}", currentValue, isPercent ? "%" : "");
        }
    }
}

