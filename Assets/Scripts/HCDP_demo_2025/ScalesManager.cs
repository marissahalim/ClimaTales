using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalesManager : MonoBehaviour
{

    public GameObject[] scales;

    public void SetActiveScale(int activeScale)
    {

        for (int i = 0; i < scales.Length; i++)
        {
            scales[i].gameObject.SetActive(false);
        }

        scales[activeScale].gameObject.SetActive(true);

    }

    public void ResetScales()
    {

        for (int i = 0; i < scales.Length; i++)
        {
            scales[i].gameObject.SetActive(false);
        }

    }
}
