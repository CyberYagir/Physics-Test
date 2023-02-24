using System;
using Builder.UI;
using UnityEngine;

namespace Builder
{
    public class Manager : MonoBehaviour
    {
        public static Manager Instance { get; private set; }
        
        [SerializeField] private ModsManager modsService;
        [SerializeField] private ItemsService itemsLoaderService;
        [SerializeField] private UIService uIWindowsService;
        [SerializeField] private BuilderController playerService;
        [SerializeField] private SelectionService selectionService;
        [SerializeField] private KeyboardService keyboardService;
        [SerializeField] private HierarchyService hierarchyService;
        
        public ItemsService ItemsService => itemsLoaderService;
        public BuilderController PlayerService => playerService;
        public SelectionService SelectionService => selectionService;
        public UIService UIWindowsService => uIWindowsService;

        public HierarchyService HierarchyService => hierarchyService;

        private void Awake()
        {
            Instance = this;
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            if (modsService)
            {
                modsService.Singleton();
            }
            else
            {
                modsService = ModsManager.Instance;
            }

            keyboardService.Init(this);
            ItemsService.Init();
            UIWindowsService.Init(this);
            playerService.Init(UIWindowsService);
            SelectionService.Init(this);
            hierarchyService.Init(PlayerService, selectionService);
        }
    }
}
