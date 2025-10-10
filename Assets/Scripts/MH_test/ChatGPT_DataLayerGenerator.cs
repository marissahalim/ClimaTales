using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatGPT_DataLayerGenerator : MonoBehaviour
{

    public GameObject parentGameObject;
    public List<Texture2D> textures;

    Dictionary<string, List<Texture2D>> yearMonth = new Dictionary<string, List<Texture2D>>();

    // Start is called before the first frame update
    void Start()
    {

        foreach(Texture2D txt in textures)
        {
            string[] parsedTextureName = ParseTextureName(txt);
            Debug.Log(parsedTextureName[0] + parsedTextureName[1]);
            //AddToDictionaryList(yearMonth, parsedTextureName[0], txt, parsedTextureName[1]);

        }


        //FillDictWithYears();
        //SortIntoDict(textures);

        //LogDictionary(yearMonth);

        //sortIntoDict(textures);

        //Dictionary<string, GameObject> yearParents = new Dictionary<string, GameObject>();

        //foreach (Texture2D namedTexture in textures)
        //{
        //    string[] parts = namedTexture.name.Split('_');
        //    if (parts.Length != 2)
        //    {
        //        Debug.LogWarning("Texture name format incorrect: " + namedTexture.name);
        //        continue;
        //    }

        //    string year = parts[0];
        //    string month = parts[1];

        //    if (!yearParents.ContainsKey(year))
        //    {
        //        GameObject yearParent = new GameObject(year);
        //        yearParent.transform.SetParent(parentGameObject.transform); // Set the parent to the specified GameObject
        //        yearParents[year] = yearParent;
        //    }

        //    GameObject monthChild = new GameObject(month);
        //    monthChild.transform.SetParent(yearParents[year].transform);

        //    RawImage rawImage = monthChild.AddComponent<RawImage>();
        //    rawImage.texture = namedTexture;
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddToDictionaryList<TKey>(Dictionary<TKey, List<Texture2D>> dictionary, TKey key, Texture2D value, string textureName)
    {
        value.name = textureName;

        if (dictionary.TryGetValue(key, out List<Texture2D> list))
        {
            list.Add(value);
        }
        else
        {
            list = new List<Texture2D> { value };
            dictionary[key] = list;
        }
    }

    //SORT THE TEXTURES INTO A DICTIONARY
    //textureList is the list of textures dragged & dropped from the Asset folder
    //public Dictionary<string, List<Texture2D>> SortIntoDict(List<Texture2D> textureList)
    //{
    //    //loop thru every texture
    //    foreach (Texture2D text in textureList)
    //    {
    //        //store the year and month of the texture in a string[]
    //        string[] parsedTextureName = ParseTextureName(text);

    //        // if the dictionary doesn't contain the year
    //        if (!yearMonth.ContainsKey(parsedTextureName[0]))
    //        {
    //            // create a temp list 
    //            // add the year as the key value
    //            yearMonth[parsedTextureName[0]] = text;
    //        }

    //        // check if the Dictionary contains the year
    //        if (yearMonth.ContainsKey(textName[0]))
    //        {
    //            //temporarily store the List<Texture2D> with the specific year key
    //            List<Texture2D> tempTextHolder = yearMonth[textName[0]];

    //            //check if the list already contains the texture
    //            if (!tempTextHolder.Contains(text))
    //            {
    //                // if it doesn't, add the texture to the list
    //                tempTextHolder.Add(text);
    //            }

    //            //update the list with the new texture
    //            yearMonth[textName[0]] = tempTextHolder;
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"Key '{textName[0]}' not found in the dictionary.");
    //        }
    //    }

    //    return yearMonth;
    //}

    //PARSE THRU THE NAMES OF THE TEXTURES
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

    void LogDictionary<TKey>(Dictionary<TKey, List<Texture2D>> dictionary)
    {
        foreach (KeyValuePair<TKey, List<Texture2D>> kvp in dictionary)
        {
            List<string> textureNames = new List<string>();
            foreach (Texture2D texture in kvp.Value)
            {
                textureNames.Add(texture.name);
            }
            Debug.Log($"Key: {kvp.Key}, Textures: {string.Join(", ", textureNames)}");
        }
    }

}
