using UnityEngine;
using UnityEngine.UI;

namespace Builder.UI
{
    public class UIModulesManager : MonoBehaviour
    {
        [System.Serializable]
        class TabButton
        {
            public enum TabType
            {
                Transform, Physics, Mesh, Light
            }

            [SerializeField] private TabType type;
            [SerializeField] private Button button;
            [SerializeField]
        }
    }
}
