using UnityEngine;
using UnityEngine.UI;

public class SliderTouchController : MonoBehaviour
{
    public Slider slider; // Reference to the UI Slider component

    void Update()
    {
        if (Input.touchCount > 0) // Check if there's at least one touch on the screen
        {
            Touch touch = Input.GetTouch(0); // Get the first touch (assuming single touch control)
            Debug.Log("Tee hee");

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                // Convert the touch position to the corresponding position on the slider
                Vector2 touchPosition = touch.position;
                RectTransform sliderRect = slider.GetComponent<RectTransform>();

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(sliderRect, touchPosition, null, out Vector2 localPoint))
                {
                    // Normalize the localPoint to the slider's range (0 to 1)
                    float normalizedValue = Mathf.InverseLerp(sliderRect.rect.xMin, sliderRect.rect.xMax, localPoint.x);

                    // Set the slider's value based on the normalized touch position
                    slider.value = normalizedValue * (slider.maxValue - slider.minValue) + slider.minValue;
                }
            }
        }
    }
}
