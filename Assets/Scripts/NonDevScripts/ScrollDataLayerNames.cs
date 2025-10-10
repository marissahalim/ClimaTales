using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollDataLayerNames : MonoBehaviour
{
    public List<GameObject> currentDataLayer = new List<GameObject>();
    [SerializeField] private Transform contentContainer;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int itemsToGenerate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateScrollView(GameObject dataLayerName)
    {
        // Data Layer gameobject
        var theParent = dataLayerName.transform;
        foreach(Transform childImage in theParent)
        {
            currentDataLayer.Add(childImage.gameObject);
        }

        itemsToGenerate = currentDataLayer.Count;

        // if (itemsToGenerate == 1) {
        //     // Do nothing, will figure out later
        //     Debug.Log("its a single layer");
        // }
        // else
        // {
        //     for(int i = 0; i < itemsToGenerate; i++)
        //     {
        //         var itemListed = Instantiate(itemPrefab);
        //         GameObject dataLayer = currentDataLayer[i].gameObject;
        //         string newName = dataLayer.name.ToString();
        //         Debug.Log(dataLayer.name);
        //         itemListed.transform.GetComponent<TMP_Text>().text = newName;
        //         itemListed.transform.SetParent(contentContainer);
        //     }
        // }
    }
}
