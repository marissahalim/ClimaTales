using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum HistState
{
    Interactive,
    Storytelling
}

public class HistStateManager : MonoBehaviour
{

    public static HistStateManager Instance;

    public HistState currentState = HistState.Interactive;

    public float idleTimeThreshold = 60f; // Time in seconds
    private float idleTimer = 0f;

    private bool hasSelectedStory = false;

    [Header("UI Controllers")]
    public HistDataController leftInteractiveUI;
    public HistDataController rightInteractiveUI;

    private PrebuiltStory selectedStory;

    [Header("Default story to auto-play after idle")]
    public PrebuiltStory defaultStory;

    [Header("UI elements to ignore (e.g. PrebuiltStory buttons)")]
    public List<GameObject> uiWhitelist;

    [Header("Interactive UI elements (Buttons, Toggles, Knobs)")]
    public List<GameObject> interactiveElements;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (currentState == HistState.Interactive)
        {
            if (IsUserInteractingWithListedUI())
            {
                // Debug.Log($"[IdleTimer] Interaction detected → Reset to 0");
                idleTimer = 0f;
            }
            else
            {
                idleTimer += Time.deltaTime;
                // Debug.Log($"[IdleTimer] No interaction → Incremented to {idleTimer:F2}");
            }

            if (idleTimer >= idleTimeThreshold && (hasSelectedStory || defaultStory != null))
            {
                // Debug.Log("Idle threshold reached — switching to Storytelling");

                if (!hasSelectedStory && defaultStory != null)
                {
                    selectedStory = defaultStory;
                    hasSelectedStory = true;
                }

                EnterStorytellingState();
            }
        }
        else if (currentState == HistState.Storytelling)
        {
            if (IsUserInteractingWithListedUI())
            {
                // Debug.Log("[State] User interacted during storytelling → returning to Interactive");
                OnAnyInteraction(); // Return to Interactive mode
            }
        }
    }

    public void OnAnyInteraction()
    {
        idleTimer = 0f;

        // Go back to Interactive if in Storytelling and interaction wasn't a story button
        if (currentState == HistState.Storytelling)
        {
            EnterInteractiveState();
        }
    }

    private void EnterInteractiveState()
    {
        Debug.Log("[StateManager] Entering Interactive state");
        currentState = HistState.Interactive;
        idleTimer = 0f;

        if (selectedStory != null)
        {
            // selectedStory.PauseStory();
            selectedStory.ResetStory();
            defaultStory.ResetStory();
        }

        // Reset UI when entering Interactive
        leftInteractiveUI?.ResetUI();
        rightInteractiveUI?.ResetUI();

        selectedStory.leftStoryLabel.text = "";
        selectedStory.rightStoryLabel.text = "";

        selectedStory.leftMapLoader?.SetStoryMode(false);
        selectedStory.rightMapLoader?.SetStoryMode(false);

        // selectedStory.RestartStory();
        // selectedStory.ResetStory();
    }

    public void HandleStorySelection(PrebuiltStory story)
    {
        Debug.Log("Story selected via button: " + story.name);
        idleTimer = 0f;

        // If the same story is already selected and we're in Storytelling, toggle it
        if (currentState == HistState.Storytelling && selectedStory == story)
        {
            story.ToggleStoryPlayback();
            return;
        }

        // If a different story is playing, pause the current one
        if (selectedStory != null && selectedStory != story)
        {
            selectedStory.PauseStory();
        }

        selectedStory = story;
        hasSelectedStory = true;

        // Only enter storytelling if not already in it or switching stories
        if (currentState != HistState.Storytelling)
        {
            EnterStorytellingState();
        }
        else
        {
            // If in Storytelling but switching stories, play the new one
            selectedStory.PlayStory();
        }
    }

    private void EnterStorytellingState()
    {

        if (currentState != HistState.Storytelling)
        {
            currentState = HistState.Storytelling;
            Debug.Log("State changed to Storytelling (idle or button)");

            // // ✅ Reset UI when entering Interactive
            // leftInteractiveUI?.ResetUI();
            // rightInteractiveUI?.ResetUI();
        }

        if (selectedStory != null)
        {
            Debug.Log("Starting story playback: " + selectedStory.name);
            selectedStory.PlayStory();
        }
        else
        {
            Debug.LogWarning("No selected story found during idle transition.");
        }
    }


    private bool IsUserInteractingWithListedUI()
    {
        // Track KnobController activity (uses global timestamp)
        if (Time.time - KnobController.LastInteractionTime < 0.1f)
        {
            // Debug.Log("[UI Interaction] KnobController recently interacted with");
            return true;
        }

        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                GameObject hit = result.gameObject;

                foreach (GameObject uiObject in interactiveElements)
                {
                    if (hit == uiObject || hit.transform.IsChildOf(uiObject.transform))
                    {
                        // Debug.Log($"[UI Interaction] Matched interactive element: {uiObject.name}");
                        return true;
                    }
                }
            }
        }

        return false;
    }

}
