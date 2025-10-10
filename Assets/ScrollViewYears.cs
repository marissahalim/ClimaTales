using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;
using System;

public class ScrollViewYears : MonoBehaviour
{

    public int startYr;
    public int endYr;
    public int step;

    public GameObject scrollViewItem;

    public SimpleScrollSnap scrollView;

    public RectTransform contentHolder;

    // Start is called before the first frame update
    void Start()
    {
        if (!isContentHolderEmpty())
        {
            // first, change the text of the first object to the start year
            TextMeshProUGUI tmp = contentHolder.GetChild(0).GetComponent<TextMeshProUGUI>();
            tmp.text = startYr.ToString();
            // then instantiate and count backwards from the end year
            int toInstantiate = (endYr - startYr) / step - 1; // (2024 - 1990) / 1 - 1 = 33
            for (int i = 0; i < toInstantiate; i++)
            {
                // Instantiate a new TMP_Text
                GameObject newTextObj = Instantiate(scrollViewItem, contentHolder);
                TextMeshProUGUI newTMP = newTextObj.GetComponent<TextMeshProUGUI>();

                // Set the year value for this text
                int year = endYr - i;
                newTMP.text = year.ToString();
            }
        }
    }

    bool isContentHolderEmpty()
    {
        if (contentHolder.childCount > 0)
        {
            Debug.Log("Content has children!");
            return false;
        }
        else
        {
            Debug.Log("Content is empty.");
            return true;
        }
    }
}
