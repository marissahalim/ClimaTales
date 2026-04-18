using System.Collections;
using System.Collections.Generic;
// using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum HistState
{
    Interactive,
    Storytelling
}

public class HistStateManager : MonoBehaviour
{

    public static HistStateManager Instance;

    public HistState currentState = HistState.Interactive;

    [Tooltip("How long (in seconds) to wait to switch from Interactive mode to Storytelling mode after no interaction")]
    public float idleTimeThreshold = 60f;
    private float idleTimer = 0f;

    private bool hasSelectedStory = false;
    private bool storyReadyToPlay = false;

    [Header("UI Controllers")]
    public HistDataController leftInteractiveUI;
    public HistDataController rightInteractiveUI;

    public StoryListManager storyListManager;
    public StoryLoader storyLoader;
    public StoryManager newStoryManager;


    public PrebuiltStoryManager storyManager;
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

    void Start()
    {
        foreach (GameObject storyBtn in storyListManager.storyButtons)
        {
            uiWhitelist.Add(storyBtn);
        }
    }

    void Update()
    {
        if (currentState == HistState.Interactive)
        {
            if (IsUserInteractingWithListedUI())
            {
                idleTimer = 0f;
            }
            else
            {
                idleTimer += Time.deltaTime;
            }

            if (storyReadyToPlay && storyLoader.doneLoadingAudios && !newStoryManager.IsPlaying)
            {
                storyReadyToPlay = false;
                storyListManager.DisableStoryList();
                newStoryManager.SetStoryInfo();
                EnterStorytellingState();
            }

            // if (idleTimer >= idleTimeThreshold && (hasSelectedStory || defaultStory != null))
            // {
            //     if (!hasSelectedStory && defaultStory != null)
            //     {
            //         selectedStory = defaultStory;
            //         hasSelectedStory = true;
            //     }

            //     EnterStorytellingState();
            // }
        }
        else if (currentState == HistState.Storytelling)
        {
            if (IsUserInteractingWithListedUI())
            {
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
        // Debug.Log("[StateManager] Entering Interactive state");
        currentState = HistState.Interactive;
        idleTimer = 0f;

        if (newStoryManager != null)
        {
            // selectedStory.PauseStory();
            newStoryManager.ResetStory();
            storyListManager.BackButtonPressed();
            // defaultStory.ResetStory();
        }

        // Reset UI when entering Interactive
        leftInteractiveUI?.ResetUI();
        rightInteractiveUI?.ResetUI();

        // selectedStory.leftStoryLabel.text = "";
        // selectedStory.rightStoryLabel.text = "";

        // selectedStory.leftMapLoader?.SetStoryMode(false);
        // selectedStory.rightMapLoader?.SetStoryMode(false);
    }

    public void NotifyStorySelected()
    {
        storyReadyToPlay = true;
    }

    public void HandleStorySelection(PrebuiltStory story)
    {
        Debug.Log(story.storyDonePlaying);

        // Debug.Log("Story selected via button: " + story.name);
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

    public void EnterStorytellingState()
    {

        if (currentState != HistState.Storytelling)
        {
            currentState = HistState.Storytelling;
            Debug.Log("State changed to Storytelling (idle or button)");
        }

        // if (selectedStory != null)
        // {
        //     Debug.Log("Starting story playback: " + selectedStory.name);
        //     selectedStory.PlayStory();
        // }
        if (newStoryManager != null)
        {
            Debug.Log("Starting story playback: " + newStoryManager.name);
            newStoryManager.PlayStory();
        }
        else
        {
            Debug.LogWarning("No selected story found during idle transition.");
        }
    }

    // private void AddListenersToInteractiveBtns()
    // {
    //     foreach (GameObject uiElement in interactiveElements)
    //     {

    //         btn.GetComponent<Button>().onClick.AddListener(() => OnAnyInteraction());
    //     }
    // }


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
