using System;
using UnityEngine;

namespace Builder
{
    public class GizmoHandle : MonoBehaviour
    {
        [SerializeField] private ObjectsController controller;
        [SerializeField] private Axis axis;
        [SerializeField] private string tool;

        private bool isActive;

        public void Init(ObjectsController controller, Axis axis, string tool)
        {
            this.controller = controller;
            this.axis = axis;
            this.tool = tool;
        }


        private void OnMouseDown()
        {
            isActive = true;
        }

        private void OnMouseUp()
        {
            isActive = false;
        }
    }
}
