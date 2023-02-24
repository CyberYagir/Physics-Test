using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Builder.UI
{
    
    
    public class UIHierarchyWindow : UIController, IPointerEnterHandler
    {
        [System.Serializable]
        public class ItemsPool
        {
            [SerializeField] private UIHierarchyItem item;
            [SerializeField] private Transform holder;
            
            private List<UIHierarchyItem> items = new List<UIHierarchyItem>(100);
            private int currentItem;
            
            private Manager manager;
            private UIHierarchyWindow window;
            public void Init(Manager manager, UIHierarchyWindow window)
            {
                this.manager = manager;
                this.window = window;
                for (int i = 0; i < 20; i++)
                {
                    AddItem();
                }
            }

            public UIHierarchyItem GetRow()
            {
                if (currentItem >= items.Count)
                {
                    AddItem();
                }

                var it = items[currentItem];
                currentItem++;
                return it;
            }

            public void ResetRows()
            {
                for (int i = 0; i < currentItem; i++)
                {
                    items[i].gameObject.SetActive(false);
                }

                currentItem = 0;
            }
            
            public void AddItem()
            {
                var it = Instantiate(item, holder);
                it.Init(manager, window);
                items.Add(it);
            }
        }
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private ItemsPool pool;

        private RectTransform rectTransform;
        private float screenWidth;
        private int screenHeight;
        
        private UIOpenWindow openWindow;

        private int itemRow = 0;
        private int row = 0;
        public override void Init(Manager manager)
        {
            base.Init(manager);
            pool.Init(manager, this);
            manager.HierarchyService.OnChanged.AddListener(UpdateList);
            manager.SelectionService.ChangeSelect.AddListener(UpdateList);
            
            
            rectTransform = GetComponent<RectTransform>();
            screenWidth = Screen.width;
            openWindow = GetComponent<UIOpenWindow>();
            openWindow.AnimationEnd.AddListener(delegate(bool state)
            {
                var uiRect = rectTransform.rect;
                var targetWidth = state ? screenWidth - (uiRect.width * mainCanvas.transform.localScale.x) : screenWidth;           
                ChangeCameraWidth(targetWidth);

                StopAllCoroutines();
            });
            
            openWindow.OpencChange.AddListener(delegate(bool state)
            {
                StartCoroutine(AnimateCameraSize(state));
            });
            UpdateList();
        }
        

        IEnumerator AnimateCameraSize(bool state)
        {
            var uiRect = rectTransform.rect;
            var targetWidth = state ? screenWidth - (uiRect.width * mainCanvas.transform.localScale.x) : screenWidth;

            float time = 0;
            while (time < 0.2f)
            {
                time += Time.deltaTime;
                ChangeCameraWidth(Mathf.Lerp(Manager.PlayerService.Camera.pixelRect.width, targetWidth, time/0.2f));
                yield return null;
            }  
            ChangeCameraWidth(targetWidth);
        }

        public void ChangeCameraWidth(float width)
        {
            Manager.PlayerService.Camera.pixelRect = new Rect(0, 0, width, Screen.height);
        }

        private List<GameObject> selectedObjects;

        public void UpdateList()
        {
            pool.ResetRows();

            selectedObjects = SelectionService.GetSelected();

            if (Manager.HierarchyService.CheckDeletedItems()) return;

            foreach (var item in Manager.HierarchyService.OrderedItems)
            {
                DrawItemRecursive(item, selectedObjects.Contains(item.Target));
            }
        }

        public void DrawItemRecursive(HierarchyService.OrderedItem item, bool selected)
        {
            if (selected == false)
            {
                selected = selectedObjects.Contains(item.Target);
            }
            pool.GetRow().Redraw(item, selected);
            if (!item.Opened) return;
            foreach (var it in item.OrderedItems)
            {
                DrawItemRecursive(it, selected);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UpdateList();
        }
    }
}
