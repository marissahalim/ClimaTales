using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class HavenCSVData : MonoBehaviour
{
    // Scenarios for Oahu
    public Dictionary<int, NewScenarioData> referenceManage = new Dictionary<int, NewScenarioData>();
    public Dictionary<int, NewScenarioData> s1ManagedData = new Dictionary<int, NewScenarioData>();
    public Dictionary<int, NewScenarioData> s2ManagedData = new Dictionary<int, NewScenarioData>();
    public Dictionary<int, NewScenarioData> s3ManagedData = new Dictionary<int, NewScenarioData>();

    void Awake()
    {
        referenceManage.Clear();
        s1ManagedData.Clear();
        s2ManagedData.Clear();
        s3ManagedData.Clear();

        List<string> jsonList = new List<string> { "parsed_data_4.json", "parsed_rps_data.json" };
        Islands islandsData;
        Islands rpsData;

        for (int i = 0; i < jsonList.Count; i++)
        {
            string path = "DataFiles/" + jsonList[i];
            string filePath = Path.Combine(Application.dataPath, path);
            string json = File.ReadAllText(filePath);

            if (filePath.Contains("data_4") == true)
            {
                islandsData = JsonUtility.FromJson<Islands>(json);

                if (islandsData.Oahu != null)
                {
                    IslandData oahu = islandsData.Oahu;
                    PopulateScenarioInfo(oahu.reference_managed, referenceManage);
                    PopulateScenarioInfo(oahu.s1_managed, s1ManagedData);
                    PopulateScenarioInfo(oahu.s2_managed, s2ManagedData);
                    PopulateScenarioInfo(oahu.s3_managed, s3ManagedData);
                }
                else
                {
                    Debug.LogWarning("No data found for Oahu.");
                }
            }
            else if (filePath.Contains("rps") == true)
            {
                rpsData = JsonUtility.FromJson<Islands>(json);

                Debug.Log(rpsData);

                if (rpsData.Oahu != null)
                {
                    IslandData oahu = rpsData.Oahu;
                    PopulateRpsInfo(oahu.reference_managed, referenceManage);
                    PopulateRpsInfo(oahu.s1_managed, s1ManagedData);
                    PopulateRpsInfo(oahu.s2_managed, s2ManagedData);
                    PopulateRpsInfo(oahu.s3_managed, s3ManagedData);
                }

            }
        }

        // string path = "DataFiles/" + "parsed_data_4.json";
        // string filePath = Path.Combine(Application.dataPath, path);
        // string json = File.ReadAllText(filePath);

        // Debug.Log(json);
        // Islands islandsData = JsonUtility.FromJson<Islands>(json);

        // if (islandsData.Oahu != null)
        // {
        //     IslandData oahu = islandsData.Oahu;
        //     PopulateScenarioInfo(oahu.reference_managed, referenceManage);
        //     PopulateScenarioInfo(oahu.s1_managed, s1ManagedData);
        //     PopulateScenarioInfo(oahu.s2_managed, s2ManagedData);
        //     PopulateScenarioInfo(oahu.s3_managed, s3ManagedData);
        // }
        // else
        // {
        //     Debug.LogWarning("No data found for Oahu.");
        // }
    }

    private void PopulateScenarioInfo(NewScenarioData[] newScenarioData, Dictionary<int, NewScenarioData> scenarioDict)
    {
        for(int i = 0; i < newScenarioData.Length; i++)
        {
            int key = newScenarioData[i].year;
            NewScenarioData newData = new NewScenarioData
            {
                year = newScenarioData[i].year,
                rps_percent = 0,
                battery = newScenarioData[i].battery,
                biofuel = newScenarioData[i].biofuel,
                biomass = newScenarioData[i].biomass,
                dgpv = newScenarioData[i].dgpv,
                fossil = newScenarioData[i].fossil,
                // geo = newScenarioData[i].geo,
                // hydro_pumped_storage = newScenarioData[i].hydro_pumped_storage,
                // hydro_run_of_river = newScenarioData[i].hydro_run_of_river,
                offshore_wind = newScenarioData[i].offshore_wind,
                onshore_wind = newScenarioData[i].onshore_wind,
                pv = newScenarioData[i].pv,
                waste_to_energy = newScenarioData[i].waste_to_energy
            };

            scenarioDict.Add(key, newData);
        }
    }

    private void PopulateRpsInfo(NewScenarioData[] rpsDict, Dictionary<int, NewScenarioData> scenarioDict)
    {
        int rpsKey = 0;
        // NewScenarioData currentData = null;

        for (int i = 0; i < rpsDict.Length; i++)
        {
            rpsKey = rpsDict[i].year;
            Debug.Log(rpsDict[i].rps_percent);

            if (scenarioDict.ContainsKey(rpsKey))
            {
                // currentData = scenarioDict[rpsKey];
                scenarioDict[rpsKey].rps_percent = rpsDict[i].rps_percent;
            }
        }

        foreach (var kvp in scenarioDict)
        {
            Debug.Log($"Scenario for Year {kvp.Key}:");
            Debug.Log($"RPS: {kvp.Value.rps_percent}");
            Debug.Log($"Biofuel Renewable: {kvp.Value.biofuel.renewable}, Capacity: {kvp.Value.biofuel.capacity_kw}, Generation: {kvp.Value.biofuel.generation_kwh}");
            Debug.Log($"Biomass Renewable: {kvp.Value.biomass.renewable}, Capacity: {kvp.Value.biomass.capacity_kw}, Generation: {kvp.Value.biomass.generation_kwh}");
            Debug.Log($"Offshore Renewable: {kvp.Value.offshore_wind.renewable}, Capacity: {kvp.Value.offshore_wind.capacity_kw}, Generation: {kvp.Value.offshore_wind.generation_kwh}");
            Debug.Log($"Onshore Renewable: {kvp.Value.onshore_wind.renewable}, Capacity: {kvp.Value.onshore_wind.capacity_kw}, Generation: {kvp.Value.onshore_wind.generation_kwh}");
        }
    }

    public NewScenarioData GetScenarioFromYear(int year, Dictionary<int, NewScenarioData> dict)
    {
        NewScenarioData selectedData = null;

        if (dict.ContainsKey(year))
        {
            selectedData = dict[year];
        }
        else
        {
            Debug.Log($"Could not find key value pair given key {year}");
        }

        return selectedData;
    }

    public Technology GetTechnology(NewScenarioData data, string techName)
    {
        Technology technology = null;

        foreach(var tech in typeof(NewScenarioData).GetFields())
        {
            technology = (Technology)tech.GetValue(data);

            if (tech.ToString() == techName)
            {
                return technology;
            }
        }

        return technology;
    }
}

[Serializable]
public class Technology
{
    public bool renewable;
    public double capacity_kw;
    public double generation_kwh;
}

[Serializable]
public class NewScenarioData
{
    public int year;
    public double rps_percent;
    public Technology battery;
    public Technology biofuel;
    public Technology biomass;
    public Technology dgpv;
    public Technology fossil;
    // public Technology geo;
    // public Technology hydro_run_of_river;
    // public Technology hydro_pumped_storage;
    public Technology offshore_wind;
    public Technology onshore_wind;
    public Technology pv;
    public Technology waste_to_energy;
}

[Serializable]
public class IslandData
{
    public NewScenarioData[] reference_managed;
    public NewScenarioData[] s1_managed;
    public NewScenarioData[] s2_managed;
    public NewScenarioData[] s3_managed;
}

[Serializable]
public class Islands
{
    public IslandData Hawaii_Island;
    public IslandData Kauai;
    public IslandData Lanai;
    public IslandData Maui;
    public IslandData Molokai;
    public IslandData Oahu;
}
