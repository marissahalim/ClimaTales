using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DataLayerIconListController : MonoBehaviour
{
    public GameObject bl_plane;
    public GameObject icon_prefab; 

    public void UpdateIconList()
    {
        /* List<DataLayer> dl_list = bl_plane.GetComponent<BaseLayerController>().dataLayers;
        foreach (DataLayer dl in dl_list)
        {
            GameObject icon = Instantiate(icon_prefab, new Vector3(0, 0, 0), Quaternion.identity);
            icon.name = "DL_icon_" + dl.layerTitle; 
            icon.transform.SetParent(this.gameObject.transform, false);
            //icon.GetComponent<Image>().sprite = dl.layerICON;
            icon.transform.GetChild(0).GetComponent<TMP_Text>().text = dl.layerTitle;
            icon.GetComponent<DataLayerReference>().datalayer = dl;
            icon.GetComponent<DataLayerReference>().datalayer_OBJ = GameObject.Find("Makawalu_System/BaseMapPlane/DataLayers/" + dl.layerTitle);
        }*/
    }
}
