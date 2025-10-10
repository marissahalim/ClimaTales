using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class TouchManager : MonoBehaviour
{
    [SerializeField]
    private GameObject handle;
    [SerializeField]
    private GameObject fill;
    [SerializeField]
    private GameObject sliderBG;
    [SerializeField]
    private RectTransform sliderRect;

    private PlayerInput playerInput;

    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    public RectTransform canvasRectTransform; // The RectTransform of the Canvas

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPressAction = playerInput.actions["TouchPress"];
        touchPositionAction = playerInput.actions["TouchPosition"];
    }

    private void OnEnable()
    {
        touchPressAction.performed += TouchPressed;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressed;
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        Vector2 screenPoint = touchPositionAction.ReadValue<Vector2>();

        // Test pressure
        // Result: Different touch pressures on the screen all return 1.0f
        var touch = Touchscreen.current.primaryTouch.pressure.ReadValue();
        //Debug.Log($"Touch Pressure: {touch}");

        (float minX, float maxX) = GetRectTransformXBounds(sliderRect);

        float xAxis = touchPositionAction.ReadValue<Vector2>().x;

        Vector3 sp;

        if (xAxis < minX)
        {
            sp = new Vector3(minX, sliderBG.transform.position.y, handle.transform.position.z);
        } else if (xAxis > maxX)
        {
            sp = new Vector3(maxX, sliderBG.transform.position.y, handle.transform.position.z);
        } else
        {
            sp = new Vector3(touchPositionAction.ReadValue<Vector2>().x, sliderBG.transform.position.y, handle.transform.position.z);
        }

        // Set the position of the handle relative to the Canvas
        RectTransform handleRectTransform = handle.GetComponent<RectTransform>();
        handleRectTransform.position = sp;

        //Fill functionality
        // Convert screen position to local position relative to the canvas
        //Vector2 localPoint;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPoint, null, out localPoint);

        // Calculate the fill amount based on handle's position
        float fillWidth = Mathf.Clamp(screenPoint.x - minX, 0, sliderRect.rect.width);
        //float fillWidth = Mathf.Clamp(screenPoint.x, 0, sliderRect.rect.width);

        // Update the fill's width and position
        RectTransform fillRectTransform = fill.GetComponent<RectTransform>();
        fillRectTransform.sizeDelta = new Vector2(fillWidth, fillRectTransform.sizeDelta.y);
        fillRectTransform.anchoredPosition = new Vector2(minX, fillRectTransform.anchoredPosition.y);

        Debug.Log("Touch position set: " + screenPoint);

    }

    // Function to get the min and max X values of a RectTransform
    public static (float minX, float maxX) GetRectTransformXBounds(RectTransform rectTransform)
    {
        // Array to hold the corners of the RectTransform
        Vector3[] corners = new Vector3[4];

        // Get the world corners of the RectTransform
        rectTransform.GetWorldCorners(corners);

        // Initialize minX and maxX with the X value of the first corner
        float minX = corners[0].x;
        float maxX = corners[0].x;

        // Iterate through the corners to find the min and max X values
        foreach (Vector3 corner in corners)
        {
            if (corner.x < minX) minX = corner.x;
            if (corner.x > maxX) maxX = corner.x;
        }

        // Return the results as a tuple
        return (minX, maxX);
    }
}
