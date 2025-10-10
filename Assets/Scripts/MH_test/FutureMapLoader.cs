using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.MUIP;

public class FutureMapLoader : MonoBehaviour
{
    public WindowManager windowManager;

    public RawImage textureHolder;
    public RawImage textureHolderOther;


    [SerializeField] private string basePath = "Textures"; // Base path for your textures
    public string fcpDataType;

    // Get button values from FCPBtnOptionLogs
    public FCPBtnOptionLog time;
    public FCPBtnOptionLog downscale;

    // Start is called before the first frame update
    void Start()
    {
        fcpDataType = "FCP_Rain";


        //if (windowManager.name.Equals("Future"))
        //{
        //    // LoadHistoricalTexture(setYear, setMonth);
        //    textureHolderOther.SetActive(false);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //if (windowManager.name.Equals("Future"))
        //{
        //    // LoadHistoricalTexture(setYear, setMonth);
        //    textureHolderOther.SetActive(false);
        //}

        //if (time.Equals("Present"))
        //{
        //    LoadPresentTexture(downscale.currentOption);
        //}
    }

    public void SetFCPDataType(int selectedDataType)
    {
        switch (selectedDataType)
        {
            case 0:
                // Debug.Log("I am future rain");
                fcpDataType = "FCP_Rain";
                break;
            case 1:
                // Debug.Log("I am future temp");
                fcpDataType = "FCP_Temp";
                break;
            default:
                break;
        }
    }

    public void LoadFutureTexture(string downgradeMethod, string timePeriod, string content, string scenario)
    {
        string fullPath = $"{basePath}/{fcpDataType}/{downgradeMethod}/{timePeriod}_{content}_{scenario}";

        // Load the texture from the Resources folder
        Texture2D mapTexture = Resources.Load<Texture2D>(fullPath);

        if (mapTexture != null)
        {
            //Debug.Log($"Successfully loaded texture: {fullPath}");
            // Apply the texture to a material or sprite renderer
            textureHolder.texture = mapTexture;
        }
        else
        {
            Debug.LogError($"Failed to load texture: {fullPath}");
        }
    }

    public void LoadPresentTexture(string downgradeMethod)
    {
        string fullPath = $"{basePath}/{fcpDataType}/{downgradeMethod}/{"Present"}";

        // Load the texture from the Resources folder
        Texture2D mapTexture = Resources.Load<Texture2D>(fullPath);

        if (mapTexture != null)
        {
            // Apply the texture to a material or sprite renderer
            textureHolder.texture = mapTexture;
        }
        else
        {
            Debug.LogError($"Failed to load texture: {fullPath}");
        }
    }
}
