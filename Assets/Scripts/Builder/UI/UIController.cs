using System.Collections.Generic;
using UnityEngine;

namespace Builder.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private List<UIController> subItems;
        protected UITabsManager tabsManager;

        public virtual void Init(UITabsManager tabsManager)
        {
            this.tabsManager = tabsManager;
            foreach (var i in subItems)
            {
                i.Init(tabsManager);
            }
        }
        
    }
}