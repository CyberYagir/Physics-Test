using System.Collections.Generic;
using UnityEngine;

namespace Builder.UI
{
    public class UIHierarchyWindow : UIController
    {
        [SerializeField] private Transform holder;
        [SerializeField] private List<UIHierarchyItem> items;
        [SerializeField] private int offcet;
        
        int i = 0;
        public override void Init(Manager manager)
        {
            base.Init(manager);
            foreach (var it in items)
            {
                it.Init(Manager, this);
            }
            manager.HierarchyService.OnChanged.AddListener(UpdateList);
        }

        public void UpdateList()
        {
            var orderedItems = Manager.HierarchyService.OrderedItems;

            i = 0;
            int ordered = 0;
            while (i < items.Count)
            {
                var item = ordered + offcet < orderedItems.Count ? orderedItems[ordered + offcet] : null;
                items[i].Redraw(item);
                i++;
                if (item != null)
                {
                    DrawChilds(item);
                }

                ordered++;
            }
        }

        public void DrawChilds(HierarchyService.OrderedItem item)
        {
            if (item.OrderedItems.Count != 0 && item.Opened)
            {
                for (int j = 0; j < item.OrderedItems.Count; j++)
                {
                    if (i < items.Count)
                    {
                        items[i].Redraw(item.OrderedItems[j]);
                        i++;
                        DrawChilds(item.OrderedItems[j]);
                    }
                }
            }
        }
    }
}
