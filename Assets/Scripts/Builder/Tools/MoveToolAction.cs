using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace Builder.Tools
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
    public class MoveToolAction : ToolAction, IToolAction
    {
        private List<SelectionData> selectionDatas = new List<SelectionData>(100);

        
        
        public void StartAction(List<GameObject> selection, Tool.AxisOption handle)
        {
            selectionDatas.Clear();
            Init(handle);
            selectionDatas = GetCollection(selection);
        }


        public void Action(List<GameObject> selection, Tool.AxisOption handle)
        {
            var offset = Vector3.zero;
            offset = GetOffcet(selectionDatas).Item1;

            for (int i = 0; i < selection.Count; i++)
            {
                Vector3 position = selectionDatas[i].Pos + offset;
                selection[i].transform.position = Input.GetKey(KeyCode.LeftControl) ? Vector3Int.RoundToInt(position) : position;
            }
        }

        public void UpdateAction(List<GameObject> selection, GameObject gizmo)
        {
            GizmoCalculations(selection, gizmo);
        }

        public void RemoveAction(List<GameObject> selection)
        {
            
        }
    }
}
