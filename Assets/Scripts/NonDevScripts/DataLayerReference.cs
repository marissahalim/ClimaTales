using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLayerReference : MonoBehaviour
{
    public DataLayer datalayer; 
    public GameObject datalayer_OBJ;

    public void ToggleObject()
    {
        if(datalayer_OBJ != null)
        {
            datalayer_OBJ.SetActive(!datalayer_OBJ.activeSelf);
        }
    }
}
