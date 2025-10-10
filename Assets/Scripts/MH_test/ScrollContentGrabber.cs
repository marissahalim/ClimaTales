using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MzTool
{
    public class ScrollContentGrabber : MonoBehaviour
    {

        public int itemIndex = 0;


        // Start is called before the first frame update
        void Start()
        {

            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PrintMe()
        {
            Debug.Log(itemIndex);
        }
    }
}
