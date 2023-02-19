using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Builder.HierarchyService;

namespace Builder.UI
{
    public class UIHierarchyItem : UIController
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image isOpened;
        [SerializeField] private float tabLength;

        private OrderedItem item;
        
        private Vector2 startSize;

        private UIHierarchyWindow window;
        
        public void Init(Manager manager, UIHierarchyWindow window)
        {
            base.Init(manager);
            this.window = window;
            startSize = content.sizeDelta;
        }


        public void Redraw(OrderedItem item)
        {
            this.item = item;
            if (item != null)
            {
                isOpened.gameObject.SetActive(item.OrderedItems.Count != 0);
                UpdateChildsRotation();
                text.text = item.Target.name;

                var it = item;
                int parentCounts = 0;
                while (it.Parent != null)
                {
                    parentCounts++;
                    it = it.Parent;
                }
                
                content.sizeDelta = startSize - new Vector2(tabLength * parentCounts, 0);
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
                isOpened.gameObject.SetActive(false);
                text.text = "";
                content.sizeDelta = startSize;
            }
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
    }
}
