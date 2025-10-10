using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class BaseLayerDataJSON 
{
    public string imgURL;
    public string jsonURL;
    public string baseTitle;
    public string baseDesc; 
    public Vector3 mapPos;
    public Vector3 mapRot;
    public Vector3 mapScale;
    public List<DataLayer> DataLayers_List;

    public BaseLayerDataJSON (string img, string json, string title, string desc, Vector3 pos, Vector3 rot, Vector3 scale, List<DataLayer> datalist)
    {
        imgURL = img;
        jsonURL = json;
        baseTitle = title;
        baseDesc = desc; 
        mapPos = pos; 
        mapRot = rot;
        mapScale = scale; 
        DataLayers_List = datalist;
    }
}