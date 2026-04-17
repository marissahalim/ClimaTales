using System.Collections;
using System.Collections.Generic;
using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public StoryLoader storyLoader;

    public bool IsPlaying { get; private set; } = false;
    public bool storyDonePlaying;

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
    public Sprite playSprite;
    public Sprite pauseSprite;
    public Image playButtonImage;
    public Button playStoryButton;
    public Button skipForwardButton;
    public Button rewindButton;
    public ProgressBar storyProgressBar;

    [Header("Table Elements")]
    public TMP_Text leftStoryLabel;
    public TMP_Text rightStoryLabel;
    public TMP_Text mapDescHolder;
    public GameObject mapDescBG;

    private Coroutine leftStoryCoroutine;
    private Coroutine rightStoryCoroutine;
    public int leftStoryIndex = 0;
    public int rightStoryIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        storyProgressBar.isOn = false;

        mapDescHolder.text = "";
        mapDescBG.SetActive(false);
    }

    // This function is to set the title and description of the story 
    public void SetStoryInfo()
    {
        if (storyLoader.currentStory != null)
        {
            storyTitle.text = storyLoader.currentStory.name;
            storyDesc.text = storyLoader.currentStory.description;
        }
    }

    // LeftMapCoroutine
    private IEnumerator LoadLeftMapStory()
    {
        mapDescHolder.gameObject.SetActive(true);
        mapDescBG.SetActive(true);
        storyDonePlaying = false;

        while (IsPlaying)
        {
            if (leftStoryIndex >= storyLoader.leftMapYears.Length)
            {
                ResetStory();
                yield break;
            }

            string enso = ENSOHelper.GetENSOPhase(storyLoader.leftMapYears[leftStoryIndex], storyLoader.leftMapMonths[leftStoryIndex]);

            leftData.SetStoryDataType(storyLoader.currentStory.leftMap.dataType[leftStoryIndex]);

            leftTime.SetTimeType(storyLoader.currentStory.leftMap.timeType[leftStoryIndex]);

            leftYears.SetValue(storyLoader.leftMapYears[leftStoryIndex]);
            leftMonths.SetValue(storyLoader.leftMapMonths[leftStoryIndex] - 1);
            leftStoryLabel.text = storyLoader.currentStory.leftMap.mapLabels[leftStoryIndex];

            leftMapLoader.LoadMapTexture(
                storyLoader.currentStory.leftMap.dataType[leftStoryIndex],
                storyLoader.currentStory.leftMap.timeType[leftStoryIndex],
                storyLoader.leftMapYears[leftStoryIndex],
                storyLoader.leftMapMonths[leftStoryIndex],
                enso
            );

            mapDescHolder.text = storyLoader.currentStory.mapDesc[leftStoryIndex];

            // Determine wait time: use audio clip length if available, otherwise use mapTransitionTimes
            // float waitTime = storyLoader.currentStory.mapTransitionTimes[leftStoryIndex];

            float waitTime = storyLoader.pageAudios[leftStoryIndex] != null
    ? storyLoader.pageAudios[leftStoryIndex].length + 2f
    : storyLoader.currentStory.mapTransitionTimes[leftStoryIndex];

            // float waitTime = pageAudios[leftStoryIndex].clip.length + audioBufferTime;
            // float waitTime = (float)(storyLoader.pageAudios?[leftStoryIndex].length + storyLoader.audioBufferTime);

            storyProgressBar.isOn = true;
            double percentage = (double)leftStoryIndex / storyLoader.leftMapYears.Length * 100;
            storyProgressBar.SetValue((float)percentage);
            // Debug.Log((float)percentage);

            PlayAudio();

            yield return new WaitForSeconds(waitTime);

            Debug.Log("Coroutine finished after " + waitTime + " seconds at: " + Time.time);

            leftStoryIndex++;
            // currentAudioIndex++;
            // yield return new WaitForSeconds(myStory.leftMap.mapTransitionTimes[leftStoryIndex]);
        }
    }

    // RightMapCoroutine
    private IEnumerator LoadRightMapStory()
    {
        int lastIndex = -1;

        while (IsPlaying)
        {
            if (rightStoryIndex >= storyLoader.rightMapYears.Length)
            {
                rightStoryIndex = 0;
                yield break;
            }

            // Only update when the left map has moved to a new frame
            if (leftStoryIndex != lastIndex)
            {
                lastIndex = leftStoryIndex;
                rightStoryIndex = leftStoryIndex;

                string enso = ENSOHelper.GetENSOPhase(storyLoader.rightMapYears[rightStoryIndex], storyLoader.rightMapMonths[rightStoryIndex]);

                rightData.SetStoryDataType(storyLoader.currentStory.rightMap.dataType[rightStoryIndex]);
                rightTime.SetTimeType(storyLoader.currentStory.rightMap.timeType[rightStoryIndex]);
                rightYears.SetValue(storyLoader.rightMapYears[rightStoryIndex]);
                rightMonths.SetValue(storyLoader.rightMapMonths[rightStoryIndex] - 1);
                rightStoryLabel.text = storyLoader.currentStory.rightMap.mapLabels[rightStoryIndex];

                rightMapLoader.LoadMapTexture(
                    storyLoader.currentStory.rightMap.dataType[rightStoryIndex],
                    storyLoader.currentStory.rightMap.timeType[rightStoryIndex],
                    storyLoader.rightMapYears[rightStoryIndex],
                    storyLoader.rightMapMonths[rightStoryIndex],
                    enso
                );
            }
            yield return null;
        }
    }

    private void PlayAudio()
    {
        // Debug.Log("Audio coroutine started at: " + Time.time);
        // AudioClip clip = storyLoader.pageAudios?[leftStoryIndex];
        // if (clip == null)
        // {
        //     Debug.LogWarning($"[StoryManager] No audio clip for frame {leftStoryIndex}.");
        //     return;
        // }

        // Debug.Log($"[StoryManager] Clip name: {clip.name}, Length: {clip.length}, Channels: {clip.channels}, Frequency: {clip.frequency}, LoadState: {clip.loadState}");

        storyLoader.audioSource.clip = storyLoader.pageAudios[leftStoryIndex];
        storyLoader.audioSource.Play();

        // Debug.Log($"[StoryManager] AudioSource isPlaying: {storyLoader.audioSource.isPlaying}");
    }

    // Control functionality
    public void PlayStory()
    {
        if (IsPlaying) return;

        leftMapLoader?.ResetLabelsAndMap();
        rightMapLoader?.ResetLabelsAndMap();

        IsPlaying = true;

        leftMapLoader?.SetStoryMode(true);
        rightMapLoader?.SetStoryMode(true);

        Debug.Log("Coroutine started at: " + Time.time);

        leftStoryCoroutine = StartCoroutine(LoadLeftMapStory());
        rightStoryCoroutine = StartCoroutine(LoadRightMapStory());

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

        storyLoader.audioSource.Pause();

        Debug.Log("Coroutine paused at: " + Time.time);

        UpdatePlayButtonImage();
    }

    private void UpdatePlayButtonImage()
    {
        if (playButtonImage == null || playSprite == null || pauseSprite == null)
            return;

        playButtonImage.sprite = IsPlaying ? pauseSprite : playSprite;
    }

    // public void OnStorySelected()
    // {
    //     HistStateManager.Instance.HandleStorySelection(this);
    // }

    public void ToggleStoryPlayback()
    {
        if (IsPlaying)
            PauseStory();
        else
            PlayStory();
    }

    public void SkipForwardOne()
    {
        if (leftStoryIndex < storyLoader.leftMapYears.Length - 1)
        {
            PauseStory();

            leftStoryIndex++;
            // currentAudioIndex++;
            rightStoryIndex++;

            PlayStory();
        }
    }

    public void GoBackwardOne()
    {
        if (leftStoryIndex > 0)
        {
            PauseStory();

            leftStoryIndex--;
            rightStoryIndex--;

            PlayStory();
        }
        else if (leftStoryIndex == 0)
        {
            PauseStory();

            // audioSource.clip = null;

            leftStoryIndex = 0;
            rightStoryIndex = 0;

            PlayStory();
        }
    }

    // RESET TOOL
    public void ResetStory()
    {
        // Stop any ongoing story playback
        PauseStory();

        storyDonePlaying = true;

        // Reset indices
        leftStoryIndex = 0;
        rightStoryIndex = 0;

        // Stop and reset audio
        storyLoader.audioSource?.Stop();
        if (storyLoader.audioSource != null)
        {
            storyLoader.audioSource.clip = null;
            storyLoader.audioSource.time = 0;
        }

        // Clear labels
        leftStoryLabel.text = "";
        rightStoryLabel.text = "";

        mapDescHolder.text = "";
        mapDescBG.SetActive(false);

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

        // Reset play/pause icon
        UpdatePlayButtonImage();

        // Reset progress bar
        storyProgressBar.SetValue(0);
        storyProgressBar.isOn = false;
    }
}
