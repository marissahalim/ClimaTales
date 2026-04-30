using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ScienceStory
{
    public string name;
    public string description;

    public string[] mapDesc;
    public float[] mapTransitionTimes; // Kind of dont want to use this, instead i want to use the audio clips time + couple secs
    public PageData leftMap;
    public PageData rightMap;
}

[Serializable]
public class PageData
{
    public string[] dataType;
    public string[] timeType;
    public string[] mapPaths;
    public string[] mapLabels;
}

public class StoryLoader : MonoBehaviour
{
    public StoryListLoader storyListLoader;

    // public TextAsset currentStoryJson;
    public ScienceStory currentStory;

    // Parsed date data
    public int[] leftMapYears;
    public int[] leftMapMonths;
    public int[] rightMapYears;
    public int[] rightMapMonths;

    public AudioClip[] pageAudios;
    public AudioSource audioSource;
    public float audioBufferTime = 1f;
    public bool doneLoadingAudios = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public ScienceStory LoadStoryMetadata(string storyPath)
    {
        string fullStoryPath = Path.Combine(Application.streamingAssetsPath, "JSONs", storyPath);

        if (!File.Exists(fullStoryPath))
        {
            Debug.LogWarning($"[StoryLoader] Metadata file not found: {fullStoryPath}");
            return null;
        }

        string json = File.ReadAllText(fullStoryPath);
        ScienceStory story = JsonUtility.FromJson<ScienceStory>(json);

        if (story == null)
            Debug.LogWarning($"[StoryLoader] Failed to parse metadata JSON at: {fullStoryPath}");

        return story;
    }

    public IEnumerator LoadStoryData(string storyPath, string audioPath)
    {
        string fullStoryPath = Path.Combine(Application.streamingAssetsPath, "JSONs", storyPath);

        if (!File.Exists(fullStoryPath))
        {
            Debug.LogError($"[StoryLoader] Story file not found: {fullStoryPath}");
            yield break;
        }

        string json = File.ReadAllText(fullStoryPath);
        currentStory = JsonUtility.FromJson<ScienceStory>(json);

        if (currentStory == null)
        {
            Debug.LogError($"[StoryLoader] Failed to parse story JSON at: {fullStoryPath}");
            yield break;
        }

        ParseMapPaths();
        Debug.Log($"[StoryLoader] Loaded story: \"{currentStory.name}\"");

        yield return StartCoroutine(LoadStoryAudio(audioPath));
    }

    public IEnumerator LoadStoryAudio(string audioFolderPath)
    {
        if (currentStory == null)
        {
            Debug.LogError("[StoryLoader] LoadStoryAudio called before LoadStoryData.");
            yield break;
        }

        doneLoadingAudios = false;

        string fullFolderPath = Path.Combine(Application.streamingAssetsPath, "Audios", audioFolderPath);
        string[] wavFiles = Directory.GetFiles(fullFolderPath, "*.wav");
        Array.Sort(wavFiles);

        int frameCount = currentStory.mapDesc.Length;
        pageAudios = new AudioClip[frameCount];

        for (int i = 0; i < wavFiles.Length && i < frameCount; i++)
        {
            using UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + wavFiles[i], AudioType.WAV);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                pageAudios[i] = DownloadHandlerAudioClip.GetContent(request);
                pageAudios[i].name = Path.GetFileNameWithoutExtension(wavFiles[i]);
            }
            else
            {
                Debug.LogWarning($"[StoryLoader] Failed to load audio file {wavFiles[i]}: {request.error}");
                pageAudios[i] = null;
            }
        }

        doneLoadingAudios = true;

        Debug.Log("[StoryLoader] Finished loading audio.");
    }

    private void ParseMapPaths()
    {
        //TODO: ALLOW FOR BLANK PATHS TO SHOW BASE MAP

        leftMapYears = new int[currentStory.leftMap.mapPaths.Length];
        leftMapMonths = new int[currentStory.leftMap.mapPaths.Length];

        for (int i = 0; i < currentStory.leftMap.mapPaths.Length; i++)
        {
            string[] parts = currentStory.leftMap.mapPaths[i].Split('_');
            if (parts.Length == 2 && int.TryParse(parts[0], out int year) && int.TryParse(parts[1], out int month))
            {
                leftMapYears[i] = year;
                leftMapMonths[i] = month;
            }
            else
            {
                // Debug.LogWarning($"Invalid leftMap path format: {myStory.leftMap.mapPaths[i]}");
            }
        }

        rightMapYears = new int[currentStory.rightMap.mapPaths.Length];
        rightMapMonths = new int[currentStory.rightMap.mapPaths.Length];

        for (int i = 0; i < currentStory.rightMap.mapPaths.Length; i++)
        {
            string[] parts = currentStory.rightMap.mapPaths[i].Split('_');
            if (parts.Length == 2 && int.TryParse(parts[0], out int year) && int.TryParse(parts[1], out int month))
            {
                rightMapYears[i] = year;
                rightMapMonths[i] = month;
            }
            else
            {
                // Debug.LogWarning($"Invalid rightMap path format: {myStory.rightMap.mapPaths[i]}");
            }
        }
    }

    public void UnloadCurrentStory()
    {
        currentStory = null;
        leftMapYears = null;
        leftMapMonths = null;
        rightMapYears = null;
        rightMapMonths = null;

        if (pageAudios != null)
        {
            foreach (AudioClip clip in pageAudios)
            {
                if (clip != null)
                    Destroy(clip);
            }
            pageAudios = null;
        }

        Debug.Log("[StoryLoader] Unloaded current story and cleared resources.");
    }
}
