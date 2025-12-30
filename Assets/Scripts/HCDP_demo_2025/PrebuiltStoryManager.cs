using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrebuiltStoryManager : MonoBehaviour
{
    public HistStateManager stateManager;

    public List<PrebuiltStory> prebuiltStories;
    public List<GameObject> storyButtons;

    [Header("Story List UI")]
    public GameObject storyListUI;
    public Transform gridContent;
    public GameObject storyButtonPrefab;
    public GameObject backButton;
    public GameObject playButton;
    public GameObject progressBar;

    [Header("Current Story")]
    public PrebuiltStory currentStory;
    public int currentStoryIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < prebuiltStories.Count; i++)
        {
            PrebuiltStory story = prebuiltStories[i];
            int index = i;

            GameObject storyButton = Instantiate(storyButtonPrefab, gridContent);

            Button button = storyButton.GetComponent<Button>();
            TMP_Text buttonTitle = button.transform.GetChild(0).GetComponent<TMP_Text>();
            buttonTitle.text = story.myStory.name;
            TMP_Text buttonDesc = button.transform.GetChild(1).GetComponent<TMP_Text>();
            buttonDesc.text = story.myStory.description;

            button.onClick.AddListener(() => SelectStory(story, index));

            storyButtons.Add(storyButton);

            story.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowCurrentStory()
    {
        // hide story list
        storyListUI.SetActive(false);

        // show current story
        backButton.SetActive(true);
        currentStory.gameObject.SetActive(true);
        playButton.SetActive(true);
        progressBar.SetActive(true);
    }

    public void ShowStoryList()
    {
        // show story list
        storyListUI.SetActive(true);

        // hide current story
        backButton.SetActive(false);
        currentStory.gameObject.SetActive(false);
        playButton.SetActive(false);
        progressBar.SetActive(false);
    }

    public void SelectStory(PrebuiltStory story, int index)
    {
        stateManager.currentState = HistState.Storytelling;

        currentStory = story;
        currentStoryIndex = index;

        ShowCurrentStory();

        PlayCurrentStory();
    }

    public void PlayCurrentStory()
    {
        if (currentStory != null)
        {
            currentStory.OnStorySelected();
        }
    }

    public void BackButton()
    {
        stateManager.currentState = HistState.Interactive;

        ShowStoryList();

        currentStory.ResetStory();

        currentStory = null;
        currentStoryIndex = 0;
    }
}
