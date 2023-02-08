using System.Collections.Generic;
using UnityEngine;

namespace Builder.UI
{
    public class UIWindowsManager : UIController
    {
            
        [System.Serializable]
        public class HotKeyWin
        {
            [SerializeField] private Keymap key;
            [SerializeField] private UIOpenWindow obj;
            public bool Opened => obj.Opened;

            public void Activate() => obj.gameObject.SetActive(true);
            
            public bool Check()
            {
                if (KeyboardService.GetDown(key))
                {
                    obj.OpenClose();
                    return true;
                }

                return false;
            }
        }

        [SerializeField] private List<HotKeyWin> hotkey;

    
    
        public bool HaveOpenedWindows()
        {
            for (int i = 0; i < hotkey.Count; i++)
            {
                if (hotkey[i].Opened == true)
                {
                    return true;
                }
            }
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
