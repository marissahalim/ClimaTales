using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabButtons : MonoBehaviour
{

    public GameObject byYearsContent;
    public GameObject byMonthsContent;

    public void ToggleByYearsContent() {
        byMonthsContent.SetActive(false);
        byYearsContent.SetActive(true);
    }
    
    public void ToggleByMonthsContent() {
        byYearsContent.SetActive(false);
        byMonthsContent.SetActive(true);
    }
}
