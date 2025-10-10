using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TableObjectsController : MonoBehaviour
{
    public GameObject gridContentParent;
    public List<GameObject> items = new List<GameObject>();
    private List<bool> itemStates = new List<bool>();
    private List<TableObjectData> startPositions = new List<TableObjectData>();
    private List<TableObjectData> savedTablePositions = new List<TableObjectData>();
    public List<GameObject> buttonList = new List<GameObject>();
    public GameObject selectedObject;
    private GameObject nextButton;
    private float positionValue;
    private float scaleValue;
    private float startPos;
    private float startScale;

    void Awake()
    {
        LoadTableObjectsFromSave();
    }

    void Start()
    {
        positionValue = gameObject.GetComponent<PositionScaleInputController>().positionSet;
        scaleValue = gameObject.GetComponent<PositionScaleInputController>().scaleSet;

        startPos = positionValue;
        startScale = scaleValue;

        foreach(Transform child in gridContentParent.transform)
        {
            buttonList.Add(child.gameObject);
        }

        foreach(var button in buttonList)
        {
            button.GetComponent<Button>().onClick.AddListener( delegate { ClickedButton(button); });
        }
    }

    public void SettingsToggled()
    {
        if (itemStates.Count == 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                itemStates.Add(items[i].activeSelf);

                if (!items[i].activeSelf)
                {
                    items[i].SetActive(true);
                }

                // turning on icons and legends 
                if (items[i].name == "TABLE_ICONS" || items[i].name == "TABLE_LEGENDS")
                {
                    for (int j = 0; j < items[i].transform.childCount; j++)
                    {
                        Transform child = items[i].transform.GetChild(j);
                        itemStates.Add(child.gameObject.activeSelf);

                        if (!child.gameObject.activeSelf)
                        {
                            child.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }

        SetOriginalPositions();
    }

    public void SetOriginalPositions()
    {
        if (startPositions.Count == 0)
        {
            foreach(var item in items)
            {
                TableObjectData newStart = new TableObjectData
                {
                    name = item.name,
                    objPosition = item.transform.position,
                    objScale = item.transform.localScale
                };

                startPositions.Add(newStart);
            }
        }
    }

    public void RevertOriginalState()
    {
        if (itemStates.Count > 0)
        {
            int stateIndex = 0;
            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetActive(itemStates[stateIndex]);

                if (items[i].name == "TABLE_ICONS" || items[i].name == "TABLE_LEGENDS")
                {
                    int parent = i;
                    int count = items[i].transform.childCount;

                    for (int j = 0; j < count; j++)
                    {
                        Transform child = items[parent].transform.GetChild(j);
                        child.gameObject.SetActive(itemStates[stateIndex + 1]);
                        stateIndex++;
                    }
                }

                stateIndex++;
            }
        }

        itemStates.Clear();
        selectedObject = null;

        if (nextButton != null)
        {
            nextButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        positionValue = startPos;
        scaleValue = startScale;
    }

    public void BackWithoutSave()
    {
        foreach(var original in startPositions)
        {
            GameObject obj = items.Find(item => item.name == original.name);
            
            if (obj != null)
            {
                obj.transform.position = original.objPosition;
                obj.transform.localScale = original.objScale;
            }
            else
            {
                Debug.Log("Could not find original item");
            }
        }

        RevertOriginalState();
        startPositions.Clear();
    }

    public void ChangeSelectedObject(GameObject obj)
    {
        selectedObject = obj;
    }

    private void ClickedButton(GameObject clickedButton)
    {
        if (nextButton != null)
        {
            nextButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        clickedButton.GetComponent<Image>().color = new Color32(236, 201, 101, 255);
        nextButton = clickedButton;
    }

    public void moveLeft()
    {
        if (selectedObject != null)
        {
            selectedObject.transform.position += Vector3.left * positionValue;
        }
    }

    public void moveRight()
    {
        if (selectedObject != null)
        {
            selectedObject.transform.position += Vector3.right * positionValue;
        }
    }

    public void moveUp()
    {
        if (selectedObject != null)
        {
            selectedObject.transform.position += Vector3.up * positionValue;
        }
    }

    public void moveDown()
    {
        if (selectedObject != null)
        {
            selectedObject.transform.position += Vector3.down * positionValue;
        }
    }

    public void ScaleUp()
    {
        Vector3 scaleChange = new Vector3(scaleValue, scaleValue, scaleValue);

        if (selectedObject != null)
        {
            selectedObject.transform.localScale += scaleChange;
        }
    }

    public void ScaleDown()
    {
        Vector3 scaleChange2 = new Vector3(-scaleValue, -scaleValue, -scaleValue);
        
        if (selectedObject != null)
        {
            selectedObject.transform.localScale += scaleChange2;
        }
    }

    public void UpdatePositionValue(float newPosition)
    {
        positionValue = newPosition;
    }

    public void UpdateScaleValue(float newScale)
    {
        scaleValue = newScale;
    }

    public void SavePosition()
    {
        foreach (var item in items)
        {
            // find matching item in savedList
            TableObjectData objSavedPosition = savedTablePositions.Find(savedInList => savedInList.name == item.name);

            if (objSavedPosition == null)
            {
                TableObjectData newData = new TableObjectData
                {
                    name = item.name,
                    objPosition = item.transform.position,
                    objScale = item.transform.localScale 
                };

                savedTablePositions.Add(newData);
            }
            else
            {
                // Update item in the saved list to reflect new values
                objSavedPosition.objPosition = item.transform.position;
                objSavedPosition.objScale = item.transform.localScale;
            }

        }

        RevertOriginalState();
        SaveTableData();
    }

    public void SaveTableData()
    {
        Wrapper wrapper = new Wrapper();
        wrapper.storedTablePositions = savedTablePositions;


        string json = JsonUtility.ToJson(wrapper);
        string filePath = Application.persistentDataPath + "/table_object_positions.json";
        File.WriteAllText(filePath, json);

        Debug.Log(filePath);
        Debug.Log(json);
    }

    public void LoadTableObjectsFromSave()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "table_object_positions.json");

        if(File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);

            if (wrapper.storedTablePositions != null)
            {
                savedTablePositions = wrapper.storedTablePositions;

                foreach (var saved in savedTablePositions)
                {
                    GameObject itemInScene = items.Find(item => item.name == saved.name);

                    if (itemInScene != null)
                    {
                        itemInScene.transform.position = saved.objPosition;
                        itemInScene.transform.localScale = saved.objScale;
                    }
                    else
                    {
                        Debug.Log("Could not find item in scene");
                    }
                }
            }
        }
        else
        {
            SaveTableData();
        }
    }
}

[Serializable]
public class Wrapper
{
    public List<TableObjectData> storedTablePositions;
}

[Serializable]
public class TableObjectData
{
    public string name;
    public Vector3 objPosition;
    public Vector3 objScale;
}