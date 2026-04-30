using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StoryButtonUI : MonoBehaviour
{

    [SerializeField] private TMP_Text storyName;
    [SerializeField] private TMP_Text storyDesc;

    [SerializeField] private Button button;

    public static event Action<string, string> OnStorySelected;

    public StoryListLoader storyListLoader;
    public StoryListManager storyListManager;

    private SingleStoryPaths storyPaths;
    private int storyIndex;

    public void Setup(SingleStoryPaths paths, int index, StoryListManager listManager, ScienceStory story)
    {
        storyPaths = paths;
        storyIndex = index;
        storyListManager = listManager;

        if (storyName != null)
            storyName.text = story != null ? story.name : paths.storyPath;

        if (storyDesc != null)
            storyDesc.text = story != null ? story.description : paths.audioPath;

        if (button != null)
            button.onClick.AddListener(OnButtonClicked);
        else
            Debug.LogWarning($"[StoryButtonUI] No Button component assigned on story button {index}.");
    }

    void OnButtonClicked()
    {
        Debug.Log($"[StoryButtonUI] Selected story {storyIndex}: " +
                  $"'{storyPaths.storyPath}' / '{storyPaths.audioPath}'");

        storyListManager.OnStoryButtonClicked(storyPaths);
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
