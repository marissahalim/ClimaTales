/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2020-10-27       WP      Initial version
 * 
 * *****************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using MzTool;
using UnityEngine.UI;

namespace MzDemo
{

    public class Demo_UIScrollCtrl : MonoBehaviour
    {
        private List<Demo_UIScrollItem> items = new List<Demo_UIScrollItem>();
        [SerializeField] private Demo_SObj_ScrollConfig config = null;
        [SerializeField] private MzUGUIScrollCtrl scrollCtrl = null;
        [SerializeField] private Demo_UIScrollItem templateItem = null;
        [SerializeField] private Text textState = null;

        public Demo_UIScrollItem curSelectItem;

        // HCDP variables
        public MapLoaderCirc map;
        public PrebuiltStory story1;
        public int itemIndex = 0;

        // Start is called before the first frame update
        private void Start()
        {
            if (config != null && scrollCtrl)
            {
                var configItems = config.arrayItemInfo;

                var uiItems = scrollCtrl.CreateItems(configItems.Length);

                for (int i = 0; i < uiItems.Count; i++)
                {
                    var uiItem = uiItems[i] as Demo_UIScrollItem;
                    uiItem.Refresh(configItems[i]);

                    items.Add(uiItem);
                }

                scrollCtrl.onSelectItemChange.AddListener(OnTemplateItem);
            }
        }

        private void OnTemplateItem(MzUGUIScrollItem item)
        {
            curSelectItem = item as Demo_UIScrollItem;

            itemIndex = items.IndexOf(curSelectItem);

            // LOGIC FOR HISTORICAL MAPS
            if (gameObject.name == "Canvas_Hor_Icons")
            {
                // if story isn't playing, 
                if (story1.IsPlaying)
                {
                    // map.SetHistDataType(itemIndex);
                }
                else
                {
                    templateItem = items[2];
                    // map.SetHistDataType(2);
                }


                // map.SetHistDataType(itemIndex);
                //Debug.Log(map.dataType);
            }
            else if (gameObject.name == "Canvas_Hor_Month")
            {
                // map.SetMonth(itemIndex);
            }
            else if (gameObject.name == "Canvas_Hor_Years")
            {
                // map.SetYear(itemIndex);
            }

            // LOGIC FOR FUTURE MAPS
            if (gameObject.name == "Canvas_Hor_Icons_FCP")
            {
                map.SetFCPDataType(itemIndex);
                //Debug.Log(map.fcpDataType);
            }


            if (templateItem) templateItem.Refresh(curSelectItem);

            RefreshStateText();
        }

        public void OnClickSelect()
        {
            scrollCtrl.SetSelectItemToCurrentItem();

            RefreshStateText();
        }

        private void RefreshStateText()
        {
            if (textState)
            {
                textState.text = scrollCtrl.IsChosenWithSelectItem ? "Current" : "Choose";
            }
        }

        public void SelectByIndex(int i)
        {
            if (i >= 0 && i < items.Count)
            {
                OnTemplateItem(items[i]);
            }
        }

    }
}