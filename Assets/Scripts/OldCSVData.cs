using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

/***
Class which processes the CSV data and is used on the bar chart and line chart storing all the information into 9 dictionaries
***/
public class OldCSVData : MonoBehaviour
{
     // Scenario E3
    // public List<KeyValuePair<int, ScenarioData>> myList = new List<KeyValuePair<int, ScenarioData>>();
    public Dictionary<int, ScenarioData> generationE3 = new Dictionary<int, ScenarioData>();
    public Dictionary<int, ScenarioData> curtailmentE3 = new Dictionary<int, ScenarioData>();
    public Dictionary<int, ScenarioData> capacityE3 = new Dictionary<int, ScenarioData>();
    // Scenario E3genmod
    public Dictionary<int, ScenarioData> generationE3genmod = new Dictionary<int, ScenarioData>();
    public Dictionary<int, ScenarioData> curtailmentE3genmod = new Dictionary<int, ScenarioData>();
    public Dictionary<int, ScenarioData> capacityE3genmod = new Dictionary<int, ScenarioData>();
    // Scenario Postapril
    public Dictionary<int, ScenarioData> generationPostapril = new Dictionary<int, ScenarioData>();
    public Dictionary<int, ScenarioData> curtailmentPostapril = new Dictionary<int, ScenarioData>();
    public Dictionary<int, ScenarioData> capacityPostapril = new Dictionary<int, ScenarioData>();

    void Awake()
    {
        List<string> jsonList = new List<string>
            { "generation_file.json", "curtailment_file.json", "capacity_file.json" };

        for(int i = 0; i < jsonList.Count; i++)
        {
            Debug.Log("hello");
            string path = "DataFiles/" + jsonList[i];
            string filePath = Path.Combine(Application.dataPath, path);
            string json = File.ReadAllText(filePath);

            if (filePath.Contains("generation") == true)
            {
                // Debug.Log(json);
                Scenario generation = JsonUtility.FromJson<Scenario>(json);
                ProcessFileToDict(generation.e3, generationE3);
                ProcessFileToDict(generation.e3genmod, generationE3genmod);
                ProcessFileToDict(generation.postapril, generationPostapril);
            }
            else if (filePath.Contains("curtailment") == true)
            {
                Scenario curtailment = JsonUtility.FromJson<Scenario>(json);
                ProcessFileToDict(curtailment.e3, curtailmentE3);
                ProcessFileToDict(curtailment.e3genmod, curtailmentE3genmod);
                ProcessFileToDict(curtailment.postapril,curtailmentPostapril);
            }
            else if (filePath.Contains("capacity") == true)
            {
                Scenario capacity = JsonUtility.FromJson<Scenario>(json);
                ProcessFileToDict(capacity.e3, capacityE3);
                ProcessFileToDict(capacity.e3genmod, capacityE3genmod);
                ProcessFileToDict(capacity.postapril,capacityPostapril);
            }
        }

        // foreach(var kvp in generationE3genmod)
        // {
        //    Debug.Log($"Key: {kvp.Key}, Value: Bio = {kvp.Value.Bio}, DER = {kvp.Value.DER}, Fossil = {kvp.Value.Fossil}, PV = {kvp.Value.PV}, Offshore = {kvp.Value.Offshore}, Wind = {kvp.Value.Wind}");
        // }
    }

    public void ProcessFileToDict(ScenarioData[] data, Dictionary<int, ScenarioData> targetDict)
    {
        for(int i = 0; i < data.Length; i++)
        {
            int key = data[i].year;
            
            ScenarioData newScenarioData = new ScenarioData
            {
                year = data[i].year,
                Bio = data[i].Bio,
                DER = data[i].DER,
                Fossil = data[i].Fossil,
                PV = data[i].PV,
                Offshore = data[i].Offshore,
                Wind = data[i].Wind
            };

            targetDict.Add(key, newScenarioData);
        }
    }

    public ScenarioData GetScenarioDataFromYear(int year, Dictionary<int, ScenarioData> dictionary)
    {
        ScenarioData selectedData = null;

        if (dictionary.ContainsKey(year))
        {
            selectedData = dictionary[year];
        }
        else
        {
            Debug.Log("couldnotRETURN key is not valid");
        }

        return selectedData;
    }
}

[Serializable]
public class ScenarioData
{
    public int year;
    public double Bio;
    public double DER;
    public double Fossil;
    public double PV;
    public double Offshore;
    public double Wind;
}

[Serializable]
public class Scenario
{
    public ScenarioData[] e3;
    public ScenarioData[] e3genmod;
    public ScenarioData[] postapril;
}