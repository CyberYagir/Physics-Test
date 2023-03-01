using System.Collections.Generic;
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
        [System.Serializable]
        public class TypeIcon
        {
            public enum TypeList
            {
                Physics, Solid, Mesh, Light
            }

            [SerializeField] private TypeList type;
            [SerializeField] private Image icon;

            public TypeList Type => type;

            public Image Icon => icon;
        }
        

        [SerializeField] private RectTransform content;
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image isOpened;
        [SerializeField] private float tabLength;
        [SerializeField] private Color noneColor, overColor, selectionColor;

        [SerializeField] private List<TypeIcon> icons = new List<TypeIcon>(4);

        private OrderedItem item;
        private bool isSelected;
        private Vector2 startSize;
        private Dictionary<TypeIcon.TypeList, Image> iconsList = new Dictionary<TypeIcon.TypeList, Image>(3);
        private UIHierarchyWindow window;
        
        public void Init(Manager manager, UIHierarchyWindow window)
        {
            base.Init(manager);
            this.window = window;
            startSize = content.sizeDelta;
            GetComponent<UIButton>().Init(Manager.UIWindowsService);

            foreach (var ic in icons)
            {
                iconsList.Add(ic.Type, ic.Icon);
            }
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

                
                iconsList[TypeIcon.TypeList.Physics].gameObject.active = item.Target.GetComponent<Rigidbody>();
                iconsList[TypeIcon.TypeList.Mesh].gameObject.active = item.Target.GetComponent<MeshRenderer>();
                iconsList[TypeIcon.TypeList.Solid].gameObject.active = item.Target.GetComponent<Collider>();
                iconsList[TypeIcon.TypeList.Light].gameObject.active = item.Target.GetComponent<Light>();
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
