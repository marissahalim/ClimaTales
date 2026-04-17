using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StoryButtonUI : MonoBehaviour
{

    [SerializeField] private TMP_Text storyPathLabel;
    [SerializeField] private TMP_Text audioPathLabel;

    [SerializeField] private Button button;

    public static event Action<string, string> OnStorySelected;

    public StoryListLoader storyListLoader;
    public StoryListManager storyListManager;
    public StoryLoader storyLoader;
    public StoryManager storyManager;

    private SingleStoryPaths storyPaths;
    private int storyIndex;

    public void Setup(SingleStoryPaths paths, int index, StoryLoader loader, StoryListManager listManager, StoryManager manager)
    // public void Setup(SingleStoryPaths paths, int index)
    {
        storyPaths = paths;
        storyIndex = index;
        storyLoader = loader;
        storyListManager = listManager;
        storyManager = manager;

        if (storyPathLabel != null)
            storyPathLabel.text = paths.storyPath;

        if (audioPathLabel != null)
            audioPathLabel.text = paths.audioPath;

        if (button != null)
            button.onClick.AddListener(OnButtonClicked);
        else
            Debug.LogWarning($"[StoryButtonUI] No Button component assigned on story button {index}.");
    }

    void OnButtonClicked()
    {
        Debug.Log($"[StoryButtonUI] Selected story {storyIndex}: " +
                  $"'{storyPaths.storyPath}' / '{storyPaths.audioPath}'");

        // Extend here: broadcast the selection to your story loading system
        // HistStateManager.Instance.HandleStorySelection();
        
        storyManager.ResetStory();

        storyLoader.LoadStoryData(storyPaths.storyPath, storyPaths.audioPath);

        storyListManager.DisableStoryList();

        storyManager.SetStoryInfo();

        // if (storyLoader.doneLoadingAudios)
        // {
        //     HistStateManager.Instance.EnterStorytellingState();
        // }
    }

    void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(OnButtonClicked);
    }

    // Read-only accessors in case other systems need the data
    public SingleStoryPaths StoryPaths => storyPaths;
    public int StoryIndex => storyIndex;
}
