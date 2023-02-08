using System.Collections.Generic;
using Base.MapBuilder;
using UnityEngine;

namespace Builder
{
    public class ItemsService : MonoBehaviour
    {
        [SerializeField] private List<GameObject> objectsList;
        [SerializeField] private List<BuildPart> buildParts;
        [SerializeField] private List<Material> materials;
        public List<BuildPart> BuildParts => buildParts;

        public void Init()
        {
            foreach (var mod in ModsManager.Instance.modLoader.mods)
            {
                objectsList.AddRange(mod.data.prefabs);
                materials.AddRange(mod.GetAllAssetsType<Material>());
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
    }
}
