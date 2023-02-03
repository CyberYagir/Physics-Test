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

        public Manager Manager => manager;

        private void Update()
        {
            for (int i = 0; i < hotkey.Count; i++)
            {
                hotkey[i].Check();
            }
        }
    }
}
