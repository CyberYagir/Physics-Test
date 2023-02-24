using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Builder
{
    public class HierarchyService : MonoBehaviour
    {
        [System.Serializable]
        public class OrderedItem
        {
            [SerializeField] private bool opened;
            private int order;
            private OrderedItem parent;
            
            [SerializeField] private GameObject target;
            [SerializeField] private List<OrderedItem> orderedItems = new List<OrderedItem>(10);
            public bool Opened => opened;

            public int Order => order;

            public OrderedItem Parent => parent;

            public GameObject Target => target;

            public List<OrderedItem> OrderedItems => orderedItems;

            public OrderedItem(GameObject target, int order, OrderedItem parent)
            {
                this.target = target;
                this.order = order;
                this.parent = parent;
            }

            public void Init()
            {
                for (int i = 0; i < target.transform.childCount; i++)
                {
                    var item = new OrderedItem(target.transform.GetChild(i).gameObject, i, this);
                    orderedItems.Add(item);
                    item.Init();
                }
            }

            public void ChangeOpen()
            {
                opened = !opened;
            }
        }

        private static HierarchyService Instance;
        
        [SerializeField] private Transform holder;
        [SerializeField] private List<OrderedItem> orderedItems = new List<OrderedItem>();

        [HideInInspector]
        public UnityEvent OnChanged = new UnityEvent();

        public List<OrderedItem> OrderedItems => orderedItems;

        public void Init(BuilderController builderController, SelectionService selectionService)
        {
            Instance = this;
            builderController.CreateObject.AddListener(AddItem);
            selectionService.DublicateSelect.AddListener(AddItem);
            selectionService.DeleteSelect.AddListener(DeleteItem);
        }
        

        
        public void AddItem(GameObject obj)
        {
            var item = new OrderedItem(obj, OrderedItems.Count, null);
            item.Init();
            obj.transform.parent = holder;
            OrderedItems.Add(item);
            OnChanged.Invoke();
        }
        public void DeleteItem(GameObject obj)
        {
            var item = FindItem(obj, OrderedItems);
            if (item.Parent != null)
            {
                item.Parent.OrderedItems.Remove(item);
            }
            else
            {
                OrderedItems.Remove(item);
            }
            OnChanged.Invoke();
        }



        public bool CheckDeletedItems()
        {
            int count = 0;
            count += orderedItems.RemoveAll(x => x.Target == null);
            for (int i = 0; i < orderedItems.Count; i++)
            {
                if (orderedItems[i].Opened)
                {
                    count += ClearInstancesRecurvive(orderedItems[i]);
                }
            }

            if (count != 0)
            {
                OnChanged.Invoke();
                return true;
            }

            return false;
        }

        private int ClearInstancesRecurvive(OrderedItem item)
        {
            int count = item.OrderedItems.RemoveAll(x => x.Target == null);
            for (int i = 0; i < item.OrderedItems.Count; i++)
            {
                if (item.OrderedItems[i].Opened)
                {
                    count += ClearInstancesRecurvive(item.OrderedItems[i]);
                }
            }
            return count;
        }

        public OrderedItem FindItem(GameObject item, List<OrderedItem> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Target == item)
                {
                    return list[i];
                }
                
                var finded = FindItem(item, list[i].OrderedItems);
                if (finded != null)
                {
                    return finded;
                }
            }

            return null;
        }

    }
}
