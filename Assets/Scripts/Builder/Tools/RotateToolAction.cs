using System.Collections.Generic;
using UnityEngine;

namespace Builder.Tools
{
    public class RotateToolAction : ToolAction, IToolAction
    {
        private Vector3 rotatedAxis;
        private Plane axisPlane;
        private Vector3 tangent;
        private Vector3 biTangent;

        private float rotationSnap;

        private Quaternion _startRotation;

        private Vector3 p_hitPoint;

        private Transform scaler;
        private Vector3 scalerPos;
        private List<Transform> parents = new List<Transform>(100);
        

        public void StartAction(List<GameObject> selection, Tool.AxisOption handle)
        {
            Init(handle);
            scaler = new GameObject("Rotator").transform;
            Vector3 center = Vector3.zero;
            foreach (var item in selection)
            {
                center += item.transform.position;
            }

            scalerPos = center / selection.Count;
            scaler.position = scalerPos;

            
            MoveParent(selection);
            
            if (selection.Count == 1 && SelectionService.GetSpace() == SelectionService.Space.Local)
            {
                CreateScaler(selection[0].transform, selection);
            }
            else
            {
                CreateScaler(scaler, selection);
            }
            
        }


        public void CreateScaler(Transform obj, List<GameObject> selection)
        {
            var local = SelectionService.GetSpace() == SelectionService.Space.Local && selection.Count == 1;
            _startRotation = local ? obj.localRotation : obj.rotation;
            if (local)
            {
                rotatedAxis = _startRotation * axis;
            }
            else
            {
                rotatedAxis = axis;
            }

            axisPlane = new Plane(rotatedAxis, obj.position);

            Vector3 startHitPoint;
            Ray cameraRay = camera.ScreenPointToRay(Input.mousePosition);
            if (axisPlane.Raycast(cameraRay, out float hitT))
            {
                startHitPoint = cameraRay.GetPoint(hitT);
                p_hitPoint = startHitPoint;
            }
            else
            {
                startHitPoint = axisPlane.ClosestPointOnPlane(p_hitPoint);
            }

            tangent = (startHitPoint - obj.position).normalized;
            biTangent = Vector3.Cross(rotatedAxis, tangent);
        }
        


        public void Action(List<GameObject> selection, Tool.AxisOption handle)
        {
            Ray cameraRay = camera.ScreenPointToRay(Input.mousePosition);

            var local = SelectionService.GetSpace() == SelectionService.Space.Local && selection.Count == 1;
            if (!axisPlane.Raycast(cameraRay, out float hitT))
            {
                return;
            }

            Vector3 hitPoint = cameraRay.GetPoint(hitT);
            Vector3 hitDirection = (hitPoint - (local ? selection[0].transform.position : scaler.position)).normalized;
            float x = Vector3.Dot(hitDirection, tangent);
            float y = Vector3.Dot(hitDirection, biTangent);
            float angleRadians = Mathf.Atan2(y, x);
            float angleDegrees = angleRadians * Mathf.Rad2Deg;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                rotationSnap = 10;
            }
            else
            {
                rotationSnap = 0;
            }

            

            if (rotationSnap != 0)
            {
                angleDegrees = Mathf.Round(angleDegrees / rotationSnap) * rotationSnap;
                angleRadians = angleDegrees * Mathf.Deg2Rad;
            }
            
            if (local)
            {
                selection[0].transform.localRotation = _startRotation * Quaternion.AngleAxis(angleDegrees, axis);
            }
            else
            {
                Vector3 invertedRotatedAxis = Quaternion.Inverse(_startRotation) * axis;
                scaler.rotation = _startRotation * Quaternion.AngleAxis(angleDegrees, invertedRotatedAxis);
            }
        }

        public void UpdateAction(List<GameObject> selection, GameObject gizmo)
        {
            GizmoCalculations(selection, gizmo);
        }

        public void RemoveAction(List<GameObject> selection)
        {
            RemoveParent(selection);
            if (scaler != null)
                Destroy(scaler.gameObject);
        }


        public void MoveParent(List<GameObject> selection)
        {
            parents.Clear();
            if (selection.Count == 1)
            {
                scaler.rotation = selection[0].transform.rotation;
            }
            else
            {
                scaler.rotation = Quaternion.Euler(Vector3.zero);
            }

            foreach (var item in selection)
            {
                parents.Add(item.transform.parent);
                item.transform.parent = scaler;
            }
        }

        public void RemoveParent(List<GameObject> selection)
        {
            for (var i = 0; i < parents.Count; i++)
            {
                if (i < selection.Count)
                {
                    selection[i].transform.parent = parents[i];
                }
            }
        }
    }
}
