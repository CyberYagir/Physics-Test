using System;
using UnityEngine;

namespace Base.MapBuilder
{
    public class BuildPart : MonoBehaviour
    {
        [System.Serializable]
        public class PreviewOptions
        {
            [SerializeField] private Vector3 position, rotation;

            public Vector3 Rotation => rotation;

            public Vector3 Position => position;
        }
        
        [SerializeField] private string partName;
        [SerializeField] private PreviewOptions previewOptions;
        public string PartName => partName;
        public PreviewOptions PartPreview => previewOptions;
    }
}
