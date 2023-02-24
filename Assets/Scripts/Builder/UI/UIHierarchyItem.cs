using System;
using Base.MapBuilder;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Builder.HierarchyService;

namespace Builder.UI
{
    public class UIHierarchyItem : UIController, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image isOpened;
        [SerializeField] private float tabLength;
        [SerializeField] private Color noneColor, overColor, selectionColor;

        private OrderedItem item;
        private bool isSelected;
        private Vector2 startSize;

        private UIHierarchyWindow window;
        
        public void Init(Manager manager, UIHierarchyWindow window)
        {
            base.Init(manager);
            this.window = window;
            startSize = content.sizeDelta;
            GetComponent<UIButton>().Init(Manager.UIWindowsService);
        }


        public void Redraw(OrderedItem item, bool selected)
        {
            if (this.item != item)
            {

                text.text = item.Target.name;
                text.color = item.Target.GetComponent<BuildPart>() ? Color.white : new Color(1, 1, 1, 0.5f);

                var it = item;
                int parentCounts = 0;
                while (it.Parent != null)
                {
                    parentCounts++;
                    it = it.Parent;
                }

                content.sizeDelta = startSize - new Vector2(tabLength * parentCounts, 0);
                isOpened.gameObject.SetActive(item.OrderedItems.Count != 0);
            }

            this.item = item;
            
            isSelected = selected;
            UpdateChildsRotation();
            background.color = isSelected ? selectionColor : noneColor;
            gameObject.SetActive(true);
        }

        public void OpenChilds()
        {
            this.item.ChangeOpen();
            UpdateChildsRotation();
            window.UpdateList();
        }

        public void UpdateChildsRotation()
        {
            if (item.Opened)
            {
                isOpened.rectTransform.localRotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                isOpened.rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            background.color = (isSelected ? selectionColor : noneColor) + overColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            background.color = isSelected ? selectionColor : noneColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SelectionService.SelectObject(item.Target, Input.GetKey(KeyCode.LeftShift));
        }
    }
}
