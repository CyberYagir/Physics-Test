using UnityEngine;

namespace PartsModule
{
    public class SingleSun : MonoBehaviour
    {
        private static SingleSun Instance;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}
