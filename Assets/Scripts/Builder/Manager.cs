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
        [SerializeField] private BuilderController builderController;
        [SerializeField] private ObjectsController objectsController;
        public ItemsGetter ItemsGetter => itemsGetter;
        public BuilderController Controller => builderController;

        public ObjectsController ObjectsController => objectsController;

        public UITabsManager TabsManager => tabsManager;

        private void Awake()
        {
            modsManager.Singleton();
            itemsGetter.Init();
            TabsManager.Init(TabsManager);
            builderController.Init(TabsManager);
            ObjectsController.Init(this);
        }
    }
}
