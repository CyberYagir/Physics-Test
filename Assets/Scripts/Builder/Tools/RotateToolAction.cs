using System.Collections.Generic;
using UnityEngine;

namespace Builder.Tools
{
    public class RotateToolAction : ToolAction, IToolAction
    {
        private Vector3  _axis;
        private Vector3  _rotatedAxis;
        private Plane    _axisPlane;
        private Vector3  _tangent;
        private Vector3  _biTangent;
        
        private Quaternion _startRotation;
        public void StartAction(List<GameObject> selection, Tool.AxisOption handle)
        {
            // _startRotation = ObjectsController.GetSpace() == ObjectsController.Space.Local ? _parentTransformHandle.target.localRotation : _parentTransformHandle.target.rotation;
        }

        public void Action(List<GameObject> selection, Tool.AxisOption handle)
        {
            
        }

        public void UpdateAction(List<GameObject> selection, GameObject gizmo)
        {
            
        }

        public void RemoveAction(List<GameObject> selection)
        {
        }
    }
}
