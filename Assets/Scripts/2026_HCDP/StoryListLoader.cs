using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[Serializable]
public class SingleStoryPaths
{
    public string storyPath;
    public string audioPath;
}

[Serializable]
public class StoryList
{
    public SingleStoryPaths[] stories;
    public int defaultStoryIndex;
}

public class StoryListLoader : MonoBehaviour
{

    [SerializeField] private string storyListPath = "PrebuiltStoryManager.txt";

    public StoryList storyList { get; private set; }
    public SingleStoryPaths DefaultStoryPaths { get; private set; }

    void Awake()
    {
        LoadStoryListData();
    }

    void LoadStoryListData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, storyListPath);

        string json;

        try
        {
            json = File.ReadAllText(filePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"[StoryConfigLoader] Failed to read file: {e.Message}");
            return;
        }

        try
        {
            storyList = JsonUtility.FromJson<StoryList>(json);
        }
        catch (Exception e)
        {
            Debug.LogError($"[StoryConfigLoader] Failed to parse JSON: {e.Message}");
            return;
        }

        if (storyList.defaultStoryIndex < 0 || storyList.defaultStoryIndex >= storyList.stories.Length)
        {
            Debug.LogWarning($"[StoryConfigLoader] defaultStoryIndex ({storyList.defaultStoryIndex}) is out of range. " +
                             "Falling back to index 0.");
            storyList.defaultStoryIndex = 0;
        }

        DefaultStoryPaths = storyList.stories[storyList.defaultStoryIndex];

        Debug.Log($"[StoryConfigLoader] Loaded {storyList.stories.Length} story(s). " +
                  $"Default: '{DefaultStoryPaths.storyPath}' / '{DefaultStoryPaths.audioPath}'");

    }

    public SingleStoryPaths GetStoryPath(int index)
    {
        if (index < 0 || index >= storyList.stories.Length)
        {
            Debug.LogError($"[StoryConfigLoader] GetStoryPaths: Index {index} is out of range.");
            return null;
        }

        return storyList.stories[index];
    }

    public List<SingleStoryPaths> GetAllStoryPaths()
    {
        return new List<SingleStoryPaths>(storyList.stories);
    }

}
