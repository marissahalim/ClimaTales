using System.Collections;
using System.Collections.Generic;
using Michsky.MUIP;
using UnityEngine;
using UnityEngine.UI;

public class StoryListManager : MonoBehaviour
{
    public StoryListLoader storyListLoader;
    public StoryLoader storyLoader;
    public StoryManager storyManager;

    [Header("Story List UI")]
    public List<GameObject> storyButtons;
    public GameObject storyListUI;
    public Transform gridContent;
    public GameObject storyButtonPrefab;

    public GameObject backButton;

    private bool storyReadyToPlay = false;

    // Start is called before the first frame update
    void Start()
    {
        CreateStoryButtons();
    }

    void CreateStoryButtons()
    {
        List<SingleStoryPaths> stories = storyListLoader.GetAllStoryPaths();

        // Clear any existing buttons before repopulating
        ClearStoryList();

        for (int i = 0; i < stories.Count; i++)
        {
            GameObject buttonObj = Instantiate(storyButtonPrefab, gridContent);
            buttonObj.name = $"StoryButton_{i}";

            StoryButtonUI buttonUI = buttonObj.GetComponent<StoryButtonUI>();
            if (buttonUI != null)
            {
                buttonUI.Setup(stories[i], i, this);
            }
            else
            {
                Debug.LogWarning($"[StoryListManager] StoryButtonPrefab is missing a StoryButtonUI component on button {i}.");
            }

            storyButtons.Add(buttonObj);
        }
    }

    public void OnStoryButtonClicked(SingleStoryPaths paths)
    {
        storyManager.ResetStory();

        storyLoader.doneLoadingAudios = false;

        HistStateManager.Instance.NotifyStorySelected();

        StartCoroutine(storyLoader.LoadStoryData(paths.storyPath, paths.audioPath));

        // storyLoader.LoadStoryData(paths.storyPath, paths.audioPath);

        // DisableStoryList();

        // storyManager.SetStoryInfo();

        // if (storyLoader.doneLoadingAudios)
        // {
        //     HistStateManager.Instance.EnterStorytellingState();
        // }
    }

    void ClearStoryList()
    {
        foreach (GameObject button in storyButtons)
        {
            if (button != null)
                Destroy(button);
        }

        storyButtons.Clear();
    }

    public void BackButtonPressed()
    {
        // storyManager.PauseStory();

        storyLoader.UnloadCurrentStory();

        // storyManager.ResetStory();

        storyListUI.SetActive(true);
        backButton.SetActive(false);
    }

    public void DisableStoryList()
    {
        backButton.SetActive(true);
        storyListUI.SetActive(false);
    }
}
