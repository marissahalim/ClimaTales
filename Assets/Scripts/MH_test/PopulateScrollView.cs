using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class ScrollViewContent<T>
{
    public List<T> scrollViewContent;
}

public class PopulateScrollView : MonoBehaviour
{

    public string scrollViewContentPath = "Months"; // file path in Resources, no extension
    public TMP_Text scrollViewItem;
    public RectTransform scrollViewContentHolder;
    public SimpleScrollSnap simpleScrollSnap;

    // Start is called before the first frame update
    void Start()
    {
        LoadAndPopulate();
    }

    void LoadAndPopulate()
    {
        TextAsset jsonText = Resources.Load<TextAsset>(scrollViewContentPath);
        if (jsonText == null)
        {
            Debug.LogError($"Could not find {scrollViewContentPath} in Resources.");
            return;
        }

        string json = jsonText.text;

        try
        {
            JObject jObj = JObject.Parse(json);
            JToken contentToken = jObj["scrollViewContent"];

            if (contentToken is JArray array)
            {
                // Check if all items are strings
                if (array.All(t => t.Type == JTokenType.String))
                {
                    var strings = array.ToObject<List<string>>();
                    PopulateList(strings);
                    return;
                }

                // Check if it's a 3-length int array for range
                if (array.Count == 3 && array.All(t => t.Type == JTokenType.Integer))
                {
                    List<int> values = array.ToObject<List<int>>();
                    List<int> range = GenerateRange(values[0], values[1], values[2]);
                    PopulateList(range);
                    return;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to parse JSON: " + e.Message);
        }

        Debug.LogError("JSON format did not match expected structure.");
    }


    List<int> GenerateRange(int start, int end, int step)
    {
        List<int> result = new List<int>();

        // Add start and end first
        result.Add(start);
        result.Add(end);

        // Add values from (end - step) to (start + step) in reverse
        for (int val = end - step; val > start; val -= step)
        {
            result.Add(val);
        }

        return result;
    }


    void PopulateList<T>(List<T> items)
    {
        foreach (T item in items)
        {
            TMP_Text obj = Instantiate(scrollViewItem, scrollViewContentHolder);
            obj.text = item.ToString();
        }
    }


}
