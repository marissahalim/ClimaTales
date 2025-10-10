using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DynamicScrollViewContent : MonoBehaviour
{
    public enum ScrollViewInputType { Integer, Strings };

    [Header("Basic Properties")]
    //public int id;
    public ScrollViewInputType inputType = ScrollViewInputType.Strings;

    [SerializeField]
    private Transform scrollViewContent;

    [SerializeField]
    private GameObject contentPrefab;

    // for days or months
    [Header("Customize string time values")]
    [SerializeField]
    private string[] stringVals;

    // for days, months, or years
    [Header("Customize int time values")]
    [SerializeField]
    private int startVal;
    [SerializeField]
    private int endVal;
    [SerializeField]
    private int timeJump;

    private void Awake()
    {
        if(inputType == ScrollViewInputType.Strings)
        {
            foreach (string str in stringVals)
            {
                GameObject newTimeVal = Instantiate(contentPrefab, scrollViewContent);
                if (newTimeVal.TryGetComponent<ScrollViewItem>(out ScrollViewItem item))
                {
                    item.ChangeStringTimeVal(str);
                }
            }
        }
        if(inputType == ScrollViewInputType.Integer)
        {
            int diff = endVal - startVal;
            for (int i = startVal; i <= endVal; i += timeJump)
            {
                GameObject newTimeVal = Instantiate(contentPrefab, scrollViewContent);
                if (newTimeVal.TryGetComponent<ScrollViewItem>(out ScrollViewItem item))
                {
                    item.ChangeIntTimeVal(i);
                }
            }
        }
    }

}
