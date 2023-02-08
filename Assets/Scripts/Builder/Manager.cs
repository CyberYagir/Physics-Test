using System;
using Builder.UI;
using UnityEngine;

namespace Builder
{
    public class Manager : MonoBehaviour
    {
        [SerializeField] private ModsManager modsService;
        [SerializeField] private ItemsService itemsLoaderService;
        [SerializeField] private UIService uIWindowsService;
        [SerializeField] private BuilderController playerService;
        [SerializeField] private SelectionService selectionService;
        [SerializeField] private KeyboardService keyboardService;
        
        public ItemsService ItemsService => itemsLoaderService;
        public BuilderController PlayerService => playerService;
        public SelectionService SelectionService => selectionService;
        public UIService UIWindowsService => uIWindowsService;

        private void Awake()
        {
            modsService.Singleton();
            keyboardService.Init(this);
            ItemsService.Init();
            UIWindowsService.Init(this);
            playerService.Init(UIWindowsService);
            SelectionService.Init(this);
        }
    }
}
