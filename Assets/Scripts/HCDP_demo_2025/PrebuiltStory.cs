using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Michsky.MUIP;
using MzDemo;

[Serializable]
public class Story
{
    public string name;
    public string description;

    public string[] mapDesc;
    public float[] mapTransitionTimes;

    public MapData leftMap;
    public MapData rightMap;
}

[Serializable]
public class MapData
{
    public string[] dataType;
    public string[] timeType;
    public string[] mapPaths;
    public string[] mapLabels;
}

public class PrebuiltStory : MonoBehaviour
{
    [Header("Story Data")]
    public TextAsset storyJSON;
    public Story myStory;
    public bool storyShouldLoop = false;

    public bool IsPlaying { get; private set; } = false;

    // Parsed date data
    public int[] leftMapYears;
    public int[] leftMapMonths;
    public int[] rightMapYears;
    public int[] rightMapMonths;

    [Header("Map Controllers")]
    public LoadTable leftMapLoader;
    public HistDataTypeSelector leftData;
    public HistDataController leftTime;
    public KnobController leftYears;
    public KnobController leftMonths;
    public LoadTable rightMapLoader;
    public HistDataTypeSelector rightData;
    public HistDataController rightTime;
    public KnobController rightYears;
    public KnobController rightMonths;

    [Header("Touchscreen Elements")]
    public TMP_Text storyTitle;
    public TMP_Text storyDesc;

    public TMP_Text mapDescHolder;

    [Header("Table Elements")]
    public TMP_Text leftStoryLabel;
    public TMP_Text rightStoryLabel;
    public Button playStoryButton;

    [Header("Audio")]
    public AudioClip[] pageAudios;
    private AudioSource audioSource;
    private int currentAudioIndex = 0;
    [Tooltip("Buffer time in seconds between audio clips")]
    public float audioBufferTime = 2f;

    [Header("Play/Pause Sprites")]
    public Sprite playSprite;
    public Sprite pauseSprite;
    public Image playButtonImage; // This should reference the Image component on your button

    public ProgressBar storyProgressBar;

    // Coroutines
    private Coroutine leftStoryCoroutine;
    private Coroutine rightStoryCoroutine;
    private Coroutine audioCoroutine;

    public int leftStoryIndex = 0;
    public int rightStoryIndex = 0;

    private void Start()
    {
        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        LoadStoryData();

        storyTitle.text = myStory.name;
        storyDesc.text = myStory.description;

        storyProgressBar.isOn = false;

        // mapDescHolder.text = myStory.mapDesc[0];

    }

    private void LoadStoryData()
    {
        if (storyJSON == null)
        {
            Debug.LogError("Story JSON file not assigned!");
            return;
        }

        myStory = JsonUtility.FromJson<Story>(storyJSON.text);

        if (myStory == null)
        {
            Debug.LogError("Failed to parse JSON.");
        }
        else
        {
            ParseMapPaths();
        }
    }

    private void ParseMapPaths()
    {
        //TODO: ALLOW FOR BLANK PATHS TO SHOW BASE MAP

        leftMapYears = new int[myStory.leftMap.mapPaths.Length];
        leftMapMonths = new int[myStory.leftMap.mapPaths.Length];

        for (int i = 0; i < myStory.leftMap.mapPaths.Length; i++)
        {
            string[] parts = myStory.leftMap.mapPaths[i].Split('_');
            if (parts.Length == 2 && int.TryParse(parts[0], out int year) && int.TryParse(parts[1], out int month))
            {
                leftMapYears[i] = year;
                leftMapMonths[i] = month;
            }
            else
            {
                Debug.LogWarning($"Invalid leftMap path format: {myStory.leftMap.mapPaths[i]}");
            }
        }

        rightMapYears = new int[myStory.rightMap.mapPaths.Length];
        rightMapMonths = new int[myStory.rightMap.mapPaths.Length];

        for (int i = 0; i < myStory.rightMap.mapPaths.Length; i++)
        {
            string[] parts = myStory.rightMap.mapPaths[i].Split('_');
            if (parts.Length == 2 && int.TryParse(parts[0], out int year) && int.TryParse(parts[1], out int month))
            {
                rightMapYears[i] = year;
                rightMapMonths[i] = month;
            }
            else
            {
                Debug.LogWarning($"Invalid rightMap path format: {myStory.rightMap.mapPaths[i]}");
            }
        }
    }

    private IEnumerator LoadLeftMapStory()
    {
        while (IsPlaying)
        {
            if (leftStoryIndex >= leftMapYears.Length)
            {
                if (storyShouldLoop)
                    leftStoryIndex = 0;
                else
                    yield break;
            }

            string enso = ENSOHelper.GetENSOPhase(leftMapYears[leftStoryIndex], leftMapMonths[leftStoryIndex]);

            leftData.SetStoryDataType(myStory.leftMap.dataType[leftStoryIndex]);
            leftTime.SetToggle(myStory.leftMap.timeType[leftStoryIndex]);

            leftYears.SetValue(leftMapYears[leftStoryIndex]);
            leftMonths.SetValue(leftMapMonths[leftStoryIndex] - 1);
            leftStoryLabel.text = myStory.leftMap.mapLabels[leftStoryIndex];

            leftMapLoader.LoadMapTexture(
                myStory.leftMap.dataType[leftStoryIndex],
                myStory.leftMap.timeType[leftStoryIndex],
                leftMapYears[leftStoryIndex],
                leftMapMonths[leftStoryIndex],
                enso
            );

            mapDescHolder.text = myStory.mapDesc[leftStoryIndex];

            // storyProgressBar.isOn = true;
            // double percentage = (double)leftStoryIndex / leftMapYears.Length * 100;
            // storyProgressBar.SetValue((float) percentage);
            // Debug.Log((float) percentage);

            // float waitTime = myStory.mapTransitionTimes[leftStoryIndex];
            // Determine wait time: use audio clip length if available, otherwise use mapTransitionTimes
            float waitTime;
            if (pageAudios != null && leftStoryIndex < pageAudios.Length && pageAudios[leftStoryIndex] != null)
            {
                waitTime = pageAudios[leftStoryIndex].length + audioBufferTime;
            }
            else
            {
                waitTime = myStory.mapTransitionTimes[leftStoryIndex];
            }

            // leftStoryIndex++;

            storyProgressBar.isOn = true;
            double percentage = (double)leftStoryIndex / leftMapYears.Length * 100;
            storyProgressBar.SetValue((float)percentage);
            Debug.Log((float)percentage);

            yield return new WaitForSeconds(waitTime);

            leftStoryIndex++;
            // yield return new WaitForSeconds(myStory.leftMap.mapTransitionTimes[leftStoryIndex]);
        }
    }

    private IEnumerator LoadRightMapStory()
    {
        while (IsPlaying)
        {
            if (rightStoryIndex >= rightMapYears.Length)
            {
                if (storyShouldLoop)
                    rightStoryIndex = 0;
                else
                    yield break;
            }

            string enso = ENSOHelper.GetENSOPhase(rightMapYears[rightStoryIndex], rightMapMonths[rightStoryIndex]);


            rightData.SetStoryDataType(myStory.rightMap.dataType[rightStoryIndex]);
            rightTime.SetToggle(myStory.rightMap.timeType[rightStoryIndex]);
            rightYears.SetValue(rightMapYears[rightStoryIndex]);
            rightMonths.SetValue(rightMapMonths[rightStoryIndex] - 1);
            rightStoryLabel.text = myStory.rightMap.mapLabels[rightStoryIndex];

            rightMapLoader.LoadMapTexture(
                myStory.rightMap.dataType[rightStoryIndex],
                myStory.rightMap.timeType[rightStoryIndex],
                rightMapYears[rightStoryIndex],
                rightMapMonths[rightStoryIndex],
                enso
            );

            // float waitTime = myStory.mapTransitionTimes[rightStoryIndex];
            // Determine wait time: use audio clip length if available, otherwise use mapTransitionTimes
            float waitTime;
            if (pageAudios != null && rightStoryIndex < pageAudios.Length && pageAudios[rightStoryIndex] != null)
            {
                waitTime = pageAudios[rightStoryIndex].length + audioBufferTime;
            }
            else
            {
                waitTime = myStory.mapTransitionTimes[rightStoryIndex];
            }

            // rightStoryIndex++;
            yield return new WaitForSeconds(waitTime);
            rightStoryIndex++;

            // yield return new WaitForSeconds(myStory.rightMap.mapTransitionTimes[rightStoryIndex]);
        }
    }


    public void PlayStory()
    {
        if (IsPlaying) return;

        leftMapLoader?.ResetLabelsAndMap();
        rightMapLoader?.ResetLabelsAndMap();

        IsPlaying = true;

        leftMapLoader?.SetStoryMode(true);
        rightMapLoader?.SetStoryMode(true);

        leftStoryCoroutine = StartCoroutine(LoadLeftMapStory());
        rightStoryCoroutine = StartCoroutine(LoadRightMapStory());

        // Play or resume audio
        PlayAudio();

        UpdatePlayButtonImage();
    }

    public void PauseStory()
    {
        if (!IsPlaying) return;

        IsPlaying = false;

        leftMapLoader?.SetStoryMode(false);
        rightMapLoader?.SetStoryMode(false);

        if (leftStoryCoroutine != null)
        {
            StopCoroutine(leftStoryCoroutine);
            leftStoryCoroutine = null;
        }

        if (rightStoryCoroutine != null)
        {
            StopCoroutine(rightStoryCoroutine);
            rightStoryCoroutine = null;
        }

        // Stop audio coroutine
        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
            audioCoroutine = null;
        }

        // Pause audio
        PauseAudio();

        UpdatePlayButtonImage();
    }

    public void ToggleStoryPlayback()
    {
        if (IsPlaying)
            PauseStory();
        else
            PlayStory();
    }



    private void UpdatePlayButtonImage()
    {
        if (playButtonImage == null || playSprite == null || pauseSprite == null)
            return;

        playButtonImage.sprite = IsPlaying ? pauseSprite : playSprite;
    }

    /// <summary>
    /// Audio controls
    /// </summary>
    private void PlayAudio()
    {
        if (pageAudios == null || pageAudios.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned to pageAudios array!");
            return;
        }

        // If audio is paused (clip exists and has played some amount), resume it
        if (audioSource.clip != null && audioSource.time > 0 && !audioSource.isPlaying)
        {
            audioSource.UnPause();
            // Restart the coroutine to continue after this clip finishes
            if (audioCoroutine == null)
            {
                audioCoroutine = StartCoroutine(AudioPlaybackCoroutine());
            }
        }
        else
        {
            // Start the audio playback coroutine from current index
            if (audioCoroutine == null)
            {
                audioCoroutine = StartCoroutine(AudioPlaybackCoroutine());
            }
        }
    }

    private void PauseAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    private IEnumerator AudioPlaybackCoroutine()
    {
        while (IsPlaying && currentAudioIndex < pageAudios.Length)
        {
            if (pageAudios[currentAudioIndex] != null)
            {
                audioSource.clip = pageAudios[currentAudioIndex];
                audioSource.Play();

                // Wait for the clip to finish
                yield return new WaitWhile(() => audioSource.isPlaying);

                // Wait for buffer time before next audio
                yield return new WaitForSeconds(audioBufferTime);
            }

            // Only increment after audio and buffer time complete
            currentAudioIndex++;

            // Loop back if needed
            if (currentAudioIndex >= pageAudios.Length && storyShouldLoop)
            {
                currentAudioIndex = 0;
            }
        }
    }

    //TODO: Skip forward 1 desc
    //TODO: Go backwards 1 desc

    public void OnStorySelected()
    {
        HistStateManager.Instance.HandleStorySelection(this);
    }

    public void RestartStory()
    {
        PauseStory();
        leftStoryIndex = 0;
        rightStoryIndex = 0;

        currentAudioIndex = 0;
        audioSource.Stop();
        audioSource.time = 0;

        PlayStory();
    }

    public void ResetStoryUI()
    {
        leftStoryIndex = 0;
        rightStoryIndex = 0;
        leftStoryLabel.text = "";
        rightStoryLabel.text = "";
    }

    public void ResetStory()
    {
        // Stop any ongoing story playback
        PauseStory();

        // Reset indices
        leftStoryIndex = 0;
        rightStoryIndex = 0;

        // Stop and reset audio
        audioSource.Stop();
        audioSource.clip = null;
        audioSource.time = 0;

        // Clear labels
        leftStoryLabel.text = "";
        rightStoryLabel.text = "";

        mapDescHolder.text = "";

        // Clear map visuals and turn off story mode
        leftMapLoader?.ResetLabelsAndMap();
        rightMapLoader?.ResetLabelsAndMap();
        leftMapLoader?.SetStoryMode(false);
        rightMapLoader?.SetStoryMode(false);

        // Reset data type and toggle selection
        leftData?.ToggleMode("No Data");
        rightData?.ToggleMode("No Data");

        leftTime?.ResetUI();
        rightTime?.ResetUI();

        // Reset knob values (e.g., to the first valid year/month)
        if (leftMapYears.Length > 0) leftYears?.SetValue(leftMapYears[0]);
        if (leftMapMonths.Length > 0) leftMonths?.SetValue(leftMapMonths[0] - 1);
        if (rightMapYears.Length > 0) rightYears?.SetValue(rightMapYears[0]);
        if (rightMapMonths.Length > 0) rightMonths?.SetValue(rightMapMonths[0] - 1);

        // Reset play/pause icon
        UpdatePlayButtonImage();

        // Reset progress bar
        storyProgressBar.SetValue(0);
        storyProgressBar.isOn = false;
    }


}