using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.Networking;

public class BaseLayer : MonoBehaviour
{ 
    [Header("General Settings")]
    public string jsonURL;
    public string imgURL;
    public string baseTitle; 
    public string baseDesc;
    [Tooltip("The image of the baseMap")]
    public Texture baseMap;
    [Header("Base Map Transform")]
    public Vector3 mapPos = new Vector3(0f,0.01f,-9.96f);
    public Vector3 mapRot = new Vector3(0f,180f,0f);
    public Vector3 mapScale = new Vector3(1f,1f,1f);
    public List<DataLayer> DataLayers_List = new List<DataLayer>(1);
    public List<string> Tags_List = new List<string>(1);

    void Start()
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, "Makawalu_Projects");
        if(!Directory.Exists(directoryPath))
        {	
            Directory.CreateDirectory(directoryPath);
        }
    }

    public void PopulateBaseFromJSON(string urlJSON)
    {
        Debug.Log (urlJSON);
        string jsonString = File.ReadAllText(urlJSON);
        BaseLayerDataJSON data = JsonUtility.FromJson<BaseLayerDataJSON>(jsonString);
        baseTitle = data.baseTitle;
        baseDesc = data.baseDesc;
        jsonURL = data.jsonURL;
        imgURL = data.imgURL;
        mapPos = data.mapPos;
        mapRot = data.mapRot;
        mapScale = data.mapScale;
        OnCreateImage(data.imgURL);
    }

    public void SaveBaseToJSON(string projectName)
    {
        string path = Path.Combine(Application.persistentDataPath, "Makawalu_Projects");
        path = Path.Combine(path, projectName);
        path = Path.Combine(path, "BaseLayer");
        if(!Directory.Exists(path))
        {	
            Directory.CreateDirectory(path);
        }
        path = Path.Combine(path, baseTitle + "_bl.json");
        Debug.Log(path);
        jsonURL = path;
        BaseLayerDataJSON data = new BaseLayerDataJSON(imgURL, path, baseTitle, baseDesc, mapPos, mapRot, mapScale, DataLayers_List);
        string entry = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(path, entry);
    }

    public void OnCreateImage(string url)
    {
        //CHANGE PARAMETERS WHEN NOT WORKING ON EXAMPLE
        StartCoroutine(CreateBaseImage(url, "Test"));
    }

    public IEnumerator CreateBaseImage(string url, string projectName)
    {
        string path = Path.Combine(Application.persistentDataPath, "Makawalu_Projects");
        path = Path.Combine(path, projectName);
        path = Path.Combine(path, "BaseLayer");
        if(!Directory.Exists(path))
        {	
            Directory.CreateDirectory(path);
        }
        path = Path.Combine(path, baseTitle + ".png");
        Debug.Log(path);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        DownloadHandler handle = www.downloadHandler;
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(www.error);
        } 
        else 
        {
            Texture2D texture2d = DownloadHandlerTexture.GetContent(www);
            baseMap = texture2d;
            byte[] bytes = ImageConversion.EncodeToPNG(texture2d);
            File.WriteAllBytes(path, bytes);
            imgURL = path;
            this.gameObject.GetComponent<Renderer>().material.SetTexture("_BaseMap", baseMap);
        }
    }
}
 