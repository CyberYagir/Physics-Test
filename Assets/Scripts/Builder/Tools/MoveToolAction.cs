using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace Builder.Tools
{
    public class MoveToolAction : ToolAction, IToolAction
    {
        public class SelectionData
        {
            private Vector3 pos;
            private Ray ray;
            private Vector3 interactionOffset;

            public SelectionData(Vector3 pos, Ray ray, Vector3 interactionOffset)
            {
                this.pos = pos;
                this.ray = ray;
                this.interactionOffset = interactionOffset;
            }

            public Vector3 InteractionOffset => interactionOffset;

            public Ray Ray => ray;

            public Vector3 Pos => pos;
        }
        private List<SelectionData> selectionDatas = new List<SelectionData>(100);

        
        
        public void StartAction(List<GameObject> selection, Tool.AxisOption handle)
        {
            handle.GizmoHandle.SetMainCollider(false);
            selectionDatas.Clear();
            ///////////////////
            
            switch (handle.Axis)
            {
                case Axis.X:
                    axis = Vector3.right;
                    break;
                case Axis.Y:
                    axis = Vector3.up;
                    break;
                case Axis.Z:
                    axis = Vector3.forward;
                    break;
            }

            Ray cameraRay = ObjectsController.GetManager().Controller.Camera.ScreenPointToRay(Input.mousePosition);
            for (int i = 0; i < selection.Count; i++)
            {

                var pos = selection[i].transform.position;

                Vector3 raxis = ObjectsController.GetSpace() == ObjectsController.Space.Local && selection.Count == 1
                    ? selection[i].transform.rotation * axis
                    : axis;
                var raxisRay = new Ray(pos, raxis);
                
                float closestT = HandleMathUtils.ClosestPointOnRay(raxisRay, cameraRay);
                Vector3 hitPoint = raxisRay.GetPoint(closestT);

                var interactionOffset = pos - hitPoint;
                
                selectionDatas.Add(new SelectionData(pos, raxisRay, interactionOffset));
            }

            //////////////////////
            handle.GizmoHandle.SetMainCollider(true);
        }

        public void Action(List<GameObject> selection, Tool.AxisOption handle)
        {
            handle.GizmoHandle.SetMainCollider(false);
            
            Ray cameraRay = ObjectsController.GetManager().Controller.Camera.ScreenPointToRay(Input.mousePosition);

            float   closestT = HandleMathUtils.ClosestPointOnRay(selectionDatas[0].Ray, cameraRay);
            Vector3 hitPoint = selectionDatas[0].Ray.GetPoint(closestT);

            Vector3 offset = hitPoint + selectionDatas[0].InteractionOffset - selectionDatas[0].Pos;
            
            
            
            for (int i = 0; i < selection.Count; i++)
            {
                Vector3 position = selectionDatas[i].Pos + offset;
                selection[i].transform.position = position;
                
            }
            handle.GizmoHandle.SetMainCollider(true);
        }

        public void UpdateAction(List<GameObject> selection, GameObject gizmo)
        {
            Quaternion rot = Quaternion.Euler(Vector3.zero);

            if (ObjectsController.GetSpace() == ObjectsController.Space.Local)
            {
                if (selection.Count == 1)
                {
                    rot = selection[0].transform.rotation;
                }
            }

            gizmo.transform.rotation = Quaternion.Lerp(gizmo.transform.rotation, rot, Time.deltaTime * 20);
        }
    }
}
