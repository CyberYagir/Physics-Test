using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Builder.UI
{
    [System.Serializable]
    public class Panel
    {
        [SerializeField] private string name;
        public string Name => name;
            
            
        public Panel(string name)
        {
            this.name = name;
        }
    }
    
    
    [System.Serializable]
    public class Folder:Panel
    {
        [SerializeField] private Folder parent;
        [SerializeField] private List<Panel> items = new List<Panel>();

        public Folder(string name, Folder parent) : base(name)
        {
            this.parent = parent;
        }

        public List<Panel> Items => items;

        public Folder Parent => parent;

        public Panel Add(Panel fol)
        {
            
            Items.Add(fol);
            return fol;
        }

        public void SortItems()
        {
            var sort = items.OrderBy(x => x is Folder).Reverse();
            items = sort.ToList();
        }
    }

    [System.Serializable]
    public class Item : Panel
    {
        private RenderTexture icon;
        private GameObject prefab;
            
        public Item(string name, RenderTexture icon, GameObject prefab) : base(name)
        {
            this.icon = icon;
            this.prefab = prefab;
        }

        public RenderTexture Icon => icon;

        public GameObject Prefab => prefab;
    }
}