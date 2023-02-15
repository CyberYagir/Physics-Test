using System.Collections.Generic;
using System.Linq;
using Base.MapBuilder;
using UnityEngine;
using UnityEngine.Events;

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
        private BuildPart part;

        public UnityEvent IconSetted = new UnityEvent();
        
        public Item(string name, RenderTexture icon, BuildPart part) : base(name)
        {
            this.icon = icon;
            this.part = part;
        }

        public RenderTexture Icon => icon;

        public GameObject Prefab => part.gameObject;

        public BuildPart Part => part;

        public void SetTexture(RenderTexture camTargetTexture)
        {
            icon = camTargetTexture;
            if (icon != null)
            {
                IconSetted.Invoke();
            }
        }
    }
}