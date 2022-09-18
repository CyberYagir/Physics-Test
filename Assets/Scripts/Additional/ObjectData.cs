using UnityEngine;

namespace Additional
{
    public class ObjectData : MonoBehaviour
    {
        [System.Serializable]
        public class MainData
        {
            [SerializeField] private string name;
            [SerializeField] private string type;

            public string Name => name;
            public string Type => type;
        }

        [SerializeField] private MainData data;
        public MainData Data => data;
    }
}
