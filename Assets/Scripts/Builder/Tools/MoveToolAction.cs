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

        private List<Vector3> selectionStartPositions = new List<Vector3>(100);

        public void StartAction(List<GameObject> selection, Tool.AxisOption handle)
        {
            handle.GizmoHandle.SetMainCollider(false);

            selectionStartPositions.Clear();
            
            Init(handle);
            
            
            foreach (var item in selection)
            {
                selectionStartPositions.Add(item.transform.position);
            }
            
            handle.GizmoHandle.SetMainCollider(true);
        }

        public void Action(List<GameObject> selection, Tool.AxisOption handle)
        {
            this.handle = handle;
            handle.GizmoHandle.SetMainCollider(false);
            var oldPos = handle.GizmoHandle.transform.position;
            handle.GizmoHandle.transform.position = startGizmoPos;
            
            
            
            var cast = Raycast();
            if (cast.collider != null)
            {
                var currentMousePos = cast.point;
                
                CalculateAction(selection, CalculateAxisVector(currentMousePos, handle.GizmoHandle.transform));
            }

            handle.GizmoHandle.transform.position = oldPos;
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


        public void CalculateAction(List<GameObject> selection, Vector3 nextPos)
        {
            var isGrid = Input.GetKey(KeyCode.LeftControl);
            for (int i = 0; i < selection.Count; i++)
            {
                selection[i].transform.position = selectionStartPositions[i] + nextPos;
                if (isGrid)
                {
                    selection[i].transform.position = Vector3Int.RoundToInt(selection[i].transform.position);
                }
            }
        }
    }
}
