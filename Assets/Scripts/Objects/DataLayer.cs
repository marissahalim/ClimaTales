using UnityEngine;
using System;
using System.Collections.Generic; 

public class DataLayer : MonoBehaviour
{ 
    [Header("General Settings")]
    public string jsonURL;
    public string layerTitle; 
    public string layerDesc;
    [Tooltip("The image of the layer")]
    public Texture layerIMG;
    [Tooltip("The icon of the layer")]
    public Texture layerICON;
    [Header("Base Map Transform")]
    public Vector3 mapPos = new Vector3(-0.1f,0.1f,-0.1f);
    public Vector3 mapRot = new Vector3(0f,0f,0f);
    public Vector3 mapScale =new Vector3(1f,1f,1f);
    //public List<Media> media_List = new List<Media>(1);
    public List<string> Tags_List = new List<string>(1);

    public void PopulateFromJSON()
    {
        //populates any information from a JSON file
    }

    public void SaveToJSON()
    {
        //saves informaiton to a json file
    }

    public void UpdateTransform(Vector3 pos, Vector3 rot, Vector3 scale) 
    {
        //Updates transform of the datalayer
    }

    public void UpdateDataLayer(string bT, string bD)
    {
       //Update text
    }

    public void UpdateImages (Texture layerIMG, Texture layerICON)
    {
        //updates both the layer image and its icon
    }

    public void AddTags(string newTags)
    {
        //adds strings to tag list, if its a single string with many tags, speeprate by commas
    }

    public void RemoveTags()
    {
        //removes tags from list
    }

    public void AddMediaObject()
    {
        //adds a media object to the list of media objects
    }

    public void RemoveMediaObject()
    {
        //removed media objects from the list
    }
}