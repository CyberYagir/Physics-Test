using System;
using System.Collections.Generic;
using UnityEngine;

namespace Builder.UI
{
    public class UITabsManager : UIController
    {
        [System.Serializable]
        public class HotKeyWin
        {
            [SerializeField] private KeyCode key;
            [SerializeField] private UIOpenWindow obj;
            public bool Opened => obj.Opened;

            public void Activate() => obj.gameObject.SetActive(true);
            
            public bool Check()
            {
                if (Input.GetKeyDown(key))
                {
                    obj.OpenClose();
                    return true;
                }

                return false;
            }
        }

        [SerializeField] private List<HotKeyWin> hotkey;
        [SerializeField] private Manager manager;

        [SerializeField] private List<GameObject> overUI = new List<GameObject>();

        public Manager Manager => manager;


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
            for (int i = 0; i < hotkey.Count; i++)
            {
                if (hotkey[i].Opened == true)
                {
                    return true;
                }
            }

            if (overUI.Count != 0) return true;
            
            return false;
        }

        private void Awake()
        {
            for (int i = 0; i < hotkey.Count; i++)
            {
                hotkey[i].Activate();
            }
        }

        private void Update()
        {
            for (int i = 0; i < hotkey.Count; i++)
            {
                hotkey[i].Check();
            }
        }
    }
}
