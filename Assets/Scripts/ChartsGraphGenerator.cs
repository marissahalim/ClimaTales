using System;
using System.Collections;
using System.Collections.Generic;
using ChartAndGraph;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class ChartsGraphGenerator : MonoBehaviour
{
    public BarChart barChart;
    public GraphChart lineChart;
    public Slider solarSlider;
    public TMP_Text solarYear;
    public TMP_Text tableSolarYear;
    public TMP_Text tableSolarScenario;
    public TMP_Text tableRPS;
    public List<GameObject> refImg;
    public List<GameObject> s1Img;
    public List<GameObject> s2Img;
    public List<GameObject> s3Img;
    public GameObject solarTemp;
    public List<GameObject> scenarioButtons;
    private int displayedScenario;
    private int[] allowedYears = { 2030, 2035, 2040, 2045 };
    [SerializeField]
    private OldCSVData csvData;
    [SerializeField]
    public Transform contentContainer;
    [SerializeField]
    public GameObject windmillPrefab;
    [SerializeField]
    private HavenCSVData havenData;
    private Dictionary<int, NewScenarioData> refScenario;
    private Dictionary<int, NewScenarioData> s1Scenario;
    private Dictionary<int, NewScenarioData> s2Scenario;
    private Dictionary<int, NewScenarioData> s3Scenario;
    private NewScenarioData displayedScen;
    void Start()
    {
        // Array value of 0 in allowedYears
        solarSlider.value = 0;

        if (havenData == null)
        {
            Debug.Log("Haven CSV Data not loaded yet");
        }
        else
        {
            refScenario = havenData.referenceManage;
            s1Scenario = havenData.s1ManagedData;
            s2Scenario = havenData.s2ManagedData;
            s3Scenario = havenData.s3ManagedData;

            displayedScenario = 0;

            displayedScen = havenData.GetScenarioFromYear(2030, refScenario);

            NewScenarioDataPercent startData = GenToPercent(displayedScen);

            // SetGraphInformation(startData);
            SetDisplayedGenValues(startData);
            SetCapVals(refScenario);
            UpdateRpsValue(displayedScen);
        }
    }
    
    public void SetDisplayedGenValues(NewScenarioDataPercent percentData)
    {
        Debug.Log("BIOFUEL " + percentData.biofuel);
        Debug.Log("BIOMASS " + percentData.biomass);
        Debug.Log("OFFSHORE " + percentData.offshore_wind);
        Debug.Log("ONSHORE " + percentData.onshore_wind);

        barChart.DataSource.SetValue("Biofuel", "Generation", percentData.biofuel);
        barChart.DataSource.SetValue("Biomass", "Generation", percentData.biomass);
        barChart.DataSource.SetValue("DGPV", "Generation", percentData.dgpv);
        barChart.DataSource.SetValue("Fossil", "Generation", percentData.fossil);
        barChart.DataSource.SetValue("Offshore", "Generation", percentData.offshore_wind);
        barChart.DataSource.SetValue("Onshore", "Generation", percentData.onshore_wind);
        barChart.DataSource.SetValue("PV", "Generation", percentData.pv);
        barChart.DataSource.SetValue("Waste To Energy", "Generation", percentData.waste_to_energy);
        barChart.GetComponent<BarAnimation>().Animate();
    }

    public void SetCapVals(Dictionary<int, NewScenarioData> selectedScenario)
    {
        string[] categories = { "Biofuel", "Biomass", "DGPV", "Fossil", "Offshore", "Onshore", "PV", "Waste To Energy" };
        NewScenarioData currentYear;
        foreach(string category in categories)
        {
            // Good practice to start and clear batch each time you are changing data in the chart
            lineChart.DataSource.StartBatch();
            lineChart.DataSource.ClearCategory(category);

            foreach(int year in allowedYears)
            {
                if (year == 2030)
                {
                    currentYear = havenData.GetScenarioFromYear(2030, selectedScenario);
                }
                else if (year == 2035)
                {
                    currentYear = havenData.GetScenarioFromYear(2035, selectedScenario);
                }
                else if (year == 2040)
                {
                    currentYear = havenData.GetScenarioFromYear(2040, selectedScenario);
                }
                else
                {
                    currentYear = havenData.GetScenarioFromYear(2045, selectedScenario);
                }
                
                switch (category)
                {
                    case "Biofuel":
                        lineChart.DataSource.AddPointToCategory("Biofuel", year, (currentYear.biofuel.capacity_kw / 1000));
                        break;
                    case "Biomass":
                        lineChart.DataSource.AddPointToCategory("Biomass", year, (currentYear.biomass.capacity_kw/ 1000));
                        break;
                    case "DGPV":
                        lineChart.DataSource.AddPointToCategory("DGPV", year, (currentYear.dgpv.capacity_kw / 1000));
                        break;
                    case "Fossil":
                        lineChart.DataSource.AddPointToCategory("Fossil", year, (currentYear.fossil.capacity_kw / 1000));
                        break;
                    case "Offshore":
                        lineChart.DataSource.AddPointToCategory("Offshore", year, (currentYear.offshore_wind.capacity_kw / 1000));
                        break;
                    case "Onshore":
                        lineChart.DataSource.AddPointToCategory("Onshore", year, (currentYear.onshore_wind.capacity_kw / 1000));
                        break;
                    case "PV":
                        lineChart.DataSource.AddPointToCategory("PV", year, (currentYear.pv.capacity_kw / 1000));
                        break;
                    case "Waste To Energy":
                        lineChart.DataSource.AddPointToCategory("Waste To Energy", year, (currentYear.waste_to_energy.capacity_kw / 1000));
                        break;
                    default:
                        Debug.LogError("Category is not valid");
                        break;
                }
            }

            lineChart.DataSource.EndBatch();
        }
    }

    public bool ChangeDisplayedImages(string scenario, int yearSliderValue)
    {
        refImg[0].SetActive(false);
        s1Img[0].SetActive(false);
        s2Img[0].SetActive(false);
        s3Img[0].SetActive(false);

        for (int i = 1; i < 4; i++)
        {
            refImg[i].SetActive(false);
            s1Img[i].SetActive(false);
            s2Img[i].SetActive(false);
            s3Img[i].SetActive(false);
        }

        int value = solarTemp.GetComponent<DataLayerImages>().checkIfAnyDataImagesOn();

        if (value > 0)
        {
            if (scenario == "ref")
            {
                refImg[yearSliderValue].SetActive(true);
                        return true;
            }
            else if (scenario == "s1")
            {
                s1Img[yearSliderValue].SetActive(true);
                        return true;
            }
            else if (scenario == "s2")
            {
                s2Img[yearSliderValue].SetActive(true);
                        return true;
            }
            else if (scenario == "s3")
            {
                s3Img[yearSliderValue].SetActive(true);
                        return true;
            }
        }

        return false;
    }

    public void UpdateYearWithSlider()
    {
        int value = (int)solarSlider.value;
        solarYear.SetText("Year " + allowedYears[value].ToString());
        tableSolarYear.SetText(allowedYears[value].ToString());
        int newYear = (int)allowedYears[value];
        Dictionary<int, NewScenarioData> changedDict;


        if (displayedScenario == 0)
        {
            displayedScen = havenData.GetScenarioFromYear(newYear, refScenario);
            changedDict = refScenario;
            ChangeDisplayedImages("ref", value);
        }
        else if (displayedScenario == 1)
        {
            displayedScen = havenData.GetScenarioFromYear(newYear, s1Scenario);
            changedDict = s1Scenario;
            ChangeDisplayedImages("s1", value);
        }
        else if (displayedScenario == 2)
        {
            displayedScen = havenData.GetScenarioFromYear(newYear, s2Scenario);
            changedDict = s2Scenario;
            ChangeDisplayedImages("s2", value);
        }
        else
        {
            displayedScen = havenData.GetScenarioFromYear(newYear, s3Scenario);
            changedDict = s3Scenario;
            ChangeDisplayedImages("s3", value);
        }

        NewScenarioDataPercent changedYear = GenToPercent(displayedScen);
        SetDisplayedGenValues(changedYear);
        SetCapVals(changedDict);
        UpdateWindmills(newYear, displayedScenario);
        UpdateRpsValue(displayedScen);
    }

    public NewScenarioDataPercent GenToPercent(NewScenarioData data)
    {
        double biof = data.biofuel.generation_kwh;
        double biom = data.biomass.generation_kwh;
        double dgpvGen = data.dgpv.generation_kwh;
        double foss = data.fossil.generation_kwh;
        double offwind = data.offshore_wind.generation_kwh;
        double onwind = data.onshore_wind.generation_kwh;
        double pvGen = data.pv.generation_kwh;
        double waste = data.waste_to_energy.generation_kwh;

        double totalValue = ( biof + biom + dgpvGen + foss + offwind + onwind + pvGen + waste);

        biof = (biof / totalValue) * 100;
        biom = (biom / totalValue) * 100;
        dgpvGen = (dgpvGen / totalValue) * 100;
        foss = (foss / totalValue) * 100;
        offwind = (offwind / totalValue) * 100;
        onwind = (onwind / totalValue) * 100;
        pvGen = (pvGen / totalValue) * 100;
        waste = (waste / totalValue) * 100;

        NewScenarioDataPercent returnedValues = new NewScenarioDataPercent
        {
            year = data.year,
            biofuel = Mathf.Round((float)biof),
            biomass = Mathf.Round((float)biom),
            dgpv = Mathf.Round((float)dgpvGen),
            fossil = Mathf.Round((float)foss),
            offshore_wind = Mathf.Round((float)offwind),
            onshore_wind = Mathf.Round((float)onwind),
            pv = Mathf.Round((float)pvGen),
            waste_to_energy = Mathf.Round((float)waste),
        };

        return returnedValues;
    }

    public void ChangeToScenario(int number)
    {
        // When a different scenario button is selected, adjust to reflect year displayed
        int value = (int)solarSlider.value;
        int currentYear = (int)allowedYears[value];
        displayedScenario = number;

        if (displayedScenario == 0)
        {
            // E3
            displayedScen = havenData.GetScenarioFromYear(currentYear, refScenario);
            SetCapVals(refScenario);
            tableSolarScenario.SetText("Reference Manage");
            ChangeDisplayedImages("ref", value);
        }
        else if (displayedScenario == 1)
        {
            // E3genmod
            displayedScen = havenData.GetScenarioFromYear(currentYear, s1Scenario);
            SetCapVals(s1Scenario);
            tableSolarScenario.SetText("S1 Manage");
            ChangeDisplayedImages("s1", value);
        }
        else if (displayedScenario == 2)
        {
            // Post april
            displayedScen = havenData.GetScenarioFromYear(currentYear, s2Scenario);
            SetCapVals(s2Scenario);
            tableSolarScenario.SetText("S2 Manage");
            ChangeDisplayedImages("s2", value);
        }
        else
        {
            displayedScen = havenData.GetScenarioFromYear(currentYear, s3Scenario);
            SetCapVals(s3Scenario);
            tableSolarScenario.SetText("S3 Manage");
            ChangeDisplayedImages("s3", value);
        }

        NewScenarioDataPercent changedScenario = GenToPercent(displayedScen);
        SetDisplayedGenValues(changedScenario);
        UpdateWindmills(currentYear, displayedScenario);
        UpdateRpsValue(displayedScen);
        HighlightScenarioButton();
    }

    public void HighlightScenarioButton()
    {

        for (int i = 0; i < scenarioButtons.Count; i++)
        {
            Color imgColor = scenarioButtons[i].GetComponent<Image>().color;
            if (i == displayedScenario)
            {
                imgColor.a = 0.33f;
            }
            else
            {
                imgColor.a = 1f;
            }
            scenarioButtons[i].GetComponent<Image>().color = imgColor;
        }
    }

    public void UpdateWindmills(int year, int scenario)
    {
        NewScenarioData offshoreData;
        int numWindmills = 0;

        if (scenario == 0)
        {
            offshoreData = havenData.GetScenarioFromYear(year, refScenario);
        }
        else if (scenario == 1)
        {
            offshoreData = havenData.GetScenarioFromYear(year, s1Scenario);
        }
        else if (scenario == 2)
        {
            offshoreData = havenData.GetScenarioFromYear(year, s2Scenario);
        }
        else
        {
            offshoreData = havenData.GetScenarioFromYear(year, s3Scenario);
        }

        if (offshoreData != null)
        {
            // Debug.Log(offshoreData.Offshore);
            for (int i = 0; i < (int)(offshoreData.offshore_wind.capacity_kw / 1000); i += 10)
            {
                numWindmills++;
            }

            PopulateOffshoreWind(numWindmills);
        }
    }

    public void PopulateOffshoreWind(int numWind)
    {
        foreach(Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < numWind; i++)
        {
            var windmill = Instantiate(windmillPrefab);
            windmill.transform.SetParent(contentContainer);
            windmill.transform.localScale = Vector2.one;
        }
    }

    public void UpdateRpsValue(NewScenarioData dict)
    {
        double value = dict.rps_percent;
        int percent = (int)(value * 100);
        tableRPS.SetText(percent.ToString() + "%");
    }
}

[Serializable]
public class ScenarioDataPercent
{
    public double year;
    public double Bio;
    public double DER;
    public double Fossil;
    public double PV;
    public double Offshore;
    public double Wind;
}


[Serializable]
public class NewScenarioDataPercent
{
    public int year;
    public double rps;
    public double biofuel;
    public double biomass;
    public double dgpv;
    public double fossil;
    // public double geo;
    // public double hydro_run_of_river;
    // public double hydro_pumped_storage;
    public double offshore_wind;
    public double onshore_wind;
    public double pv;
    public double waste_to_energy;
}