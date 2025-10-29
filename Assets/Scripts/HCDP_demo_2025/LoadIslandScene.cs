using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadIslandScene : MonoBehaviour
{
    public string islandName;
    
    // TODO Fade in/out animation in each load scene

    public void LoadChooseScene()
    {
        string sceneName = "ChooseIsland";
        Debug.Log("sceneName to load: " + sceneName);
        SceneManager.LoadScene(sceneName);
        islandName = "";
    }

    public void LoadOahudScene()
    {
        string sceneName = "ClimaTales_Oct2025_Oahu";
        Debug.Log("sceneName to load: " + sceneName);
        SceneManager.LoadScene(sceneName);
        islandName = "Oahu";
    }

    public void LoadMauiScene()
    {
        string sceneName = "ClimaTales_Oct2025_Maui";
        Debug.Log("sceneName to load: " + sceneName);
        SceneManager.LoadScene(sceneName);
        islandName = "Maui";
    }

}
