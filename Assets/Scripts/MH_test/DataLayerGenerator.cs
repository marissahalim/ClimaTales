using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class DataLayerGenerator : MonoBehaviour
{
    public GameObject parentGameObject;
    //public RectTransform parentRectTransform;

    public string startYear;

    public List<Texture2D> textures;

    private string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

    private string layerName;

    [MenuItem("AssetDatabase/LoadAssetExample")]
    static Texture2D ImportExample()
    {
        Texture2D t = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Resources/Textures/Test/temperature_mean_month_oa_data_map_1990_01.png", typeof(Texture2D));
        return t;
    }

    private void Awake()
    {

        CreateDataLayerGameObj();
    }



    private void CreateDataLayerGameObj()
    {
        //VARIABLES
        

        //GET FIRST YEAR
        string firstTexture = textures[0].name;
        int underscore = firstTexture.IndexOf('_');
        string currentYear = firstTexture.Substring(0, underscore);
        

        foreach (Texture2D texture in textures)
        {
            //Debug.Log(ParseTextureName(texture)[0]);

            


        }



        //INSTANTIATE YEAR GAME OBJECT


    }

    public string[] ParseTextureName(Texture2D txt)
    {
        string[] time = new string[2];

        string currentTexture = txt.name;
        int underscoreIndex = currentTexture.IndexOf('_');
        string year = currentTexture.Substring(0, underscoreIndex);
        string month = currentTexture.Substring(underscoreIndex + 1);

        time[0] = year;
        time[1] = month;

        return time;
    }



    //private void CreateDataFolders()
    //{
    //    int yearsToShow = (int) Mathf.Ceil(textures.Count/12);
    //    Debug.Log(yearsToShow);
    //}

    //private void CreateDataLayerGameObj()
    //{
    //    int count = 1;

    //    foreach (Texture2D texture in textures)
    //    {
    //        // TEXTURE PARSING
    //        string currentTexture = texture.name;
    //        int currentYear = 1990;

    //        Debug.Log(currentTexture);

    //        int underscoreIndex = currentTexture.IndexOf('_');

    //        // Extract the substring before the first underscore
    //        string year = currentTexture.Substring(0, underscoreIndex);
    //        Debug.Log("Year: " + year);

    //        // Extract the substring after the first underscore
    //        string month = currentTexture.Substring(underscoreIndex + 1);
    //        //Debug.Log("Month: " + month);

    //        // GAME OBJECT CODE
    //        // Instantiate an empty GameObject
    //        GameObject newGameObject = new GameObject(month);

    //        // Set parent of GameObject
    //        newGameObject.transform.SetParent(parentGameObject.transform, false);

    //        // Add a RawImage component to the new GameObject
    //        RawImage rawImage = newGameObject.AddComponent<RawImage>();

    //        rawImage.texture = texture;
    //    }
    //}
}
