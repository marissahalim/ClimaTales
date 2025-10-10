using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ScrollViewItem : MonoBehaviour
{
    // TMP_Text prefab 
    [SerializeField]
    private TMP_Text timeVal;

    // update text value in the TMP_Text game object
    public void ChangeStringTimeVal(string str)
    {
        timeVal.SetText(str);
    }

    public void ChangeIntTimeVal(int num)
    {
        timeVal.SetText(num.ToString());
    }


}
