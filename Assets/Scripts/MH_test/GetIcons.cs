using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetIcons : MonoBehaviour
{
    public string dataType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDataType(int iconIndex)
    {
        switch (iconIndex)
        {
            case 0:
                Debug.Log("Its rain");
                dataType = "Rain";
                break;
            case 1:
                Debug.Log("Its temp");
                dataType = "Temp";
                break;
            default:
                break;
        }
    }
}
