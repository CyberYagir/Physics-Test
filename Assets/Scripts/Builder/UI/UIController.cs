using System.Collections.Generic;
using UnityEngine;

namespace Builder.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private List<UIController> subItems;
        private Manager manager;

        protected Manager Manager => manager;

        public virtual void Init(Manager manager)
        {
            this.manager = manager;
            foreach (var i in subItems)
            {
                i.Init(manager);
            }
        }
        
    }
}