using System.Collections.Generic;
using UnityEngine;

namespace Builder
{
    // [CreateAssetMenu(menuName = "Create MaterialsHolder", fileName = "MaterialsHolder", order = 0)]
    public class MaterialsHolder : ScriptableObject
    {
        [System.Serializable]
        public class HolderData
        {
            [SerializeField] private string materialName;
            [SerializeField] private Material material;
        }

        [SerializeField] private List<HolderData> materials;
    }
}
