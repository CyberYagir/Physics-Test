using System;
using System.Collections.Generic;
using UnityEngine;

namespace Builder.UI
{
    public class UIService : UIController{
    
        
        [SerializeField] private List<GameObject> overUI = new List<GameObject>();
        [SerializeField] private UIWindowsManager windowsManager;


        public void Over(GameObject obj, bool over)
        {
            if (over)
            {
                if (!overUI.Contains(obj))
                {
                    overUI.Add(obj);
                }
            }
            else
            {
                overUI.Remove(obj);
            }
        }

        public bool HaveOpenedWindowsOrUI()
        {
            return windowsManager.HaveOpenedWindows() || overUI.Count != 0;
        }
    }
}
