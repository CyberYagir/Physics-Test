using System.Collections.Generic;
using Base.MapBuilder;
using UnityEngine;

namespace Builder
{
    public class ItemsService : MonoBehaviour
    {
        private static ItemsService Instance;
        
        [SerializeField] private List<GameObject> objectsList;
        [SerializeField] private List<BuildPart> buildParts;
        [SerializeField] private List<MaterialsHolder> materials;
        public List<BuildPart> BuildParts => buildParts;

        public void Init()
        {
            Instance = this;
            
            foreach (var mod in ModsManager.Instance.modLoader.mods)
            {
                objectsList.AddRange(mod.data.prefabs);
                materials.AddRange(mod.GetAllAssetsType<MaterialsHolder>());
            }

            for (int i = 0; i < objectsList.Count; i++)
            {
                var part = objectsList[i].GetComponent<BuildPart>();
                if (part)
                {
                    buildParts.Add(part);
                }
            }
        }

        
        
        public static BuildPart GetItem(string name)
        {
            if (Instance == null)
            {
                return null;
            }

            foreach (var item in Instance.buildParts)
            {
                var objName = item.name.Split('$')[0];
                
                if (objName.ToLower().Trim() == name.ToLower())
                {
                    return item;
                }
            }

            return null;
        }
    }
}
