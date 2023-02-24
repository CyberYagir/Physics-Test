using System;
using UnityEngine;

namespace Builder
{
    [RequireComponent(typeof(Camera))]
    public class CameraSync : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        private Camera current;

        private void Awake()
        {
            current = GetComponent<Camera>();
        }

        void LateUpdate()
        {
            current.fieldOfView = mainCamera.fieldOfView;
            current.rect = mainCamera.rect;
        }
    }
}
