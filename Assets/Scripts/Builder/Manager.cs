using System;
using Builder.UI;
using UnityEngine;

namespace Builder
{
    public class Manager : MonoBehaviour
    {
        [SerializeField] private ModsManager modsManager;
        [SerializeField] private ItemsGetter itemsGetter;
        [SerializeField] private UITabsManager tabsManager;
        public ItemsGetter ItemsGetter => itemsGetter;

        private void Awake()
        {
            modsManager.Singleton();
            itemsGetter.Init();
            tabsManager.Init(tabsManager);
        }
    }
}
