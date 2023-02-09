using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace Builder.Tools
{
    public class ScaleToolAction : ToolAction, IToolAction
    {
        private Transform scaler;
        private List<Transform> parents = new List<Transform>(100);
        private Vector3 scale;
        private List<SelectionData> selectionDatas = new List<SelectionData>(100);

        private Vector3 scalerPos;
        private Vector3 firstItemScale;
        public void StartAction(List<GameObject> selection, Tool.AxisOption handle)
        {
            UpdateScalesZeros(selection);
            Init(handle);
            scaler = new GameObject("Scaler").transform;
            scale = Vector3.one;
            selectionDatas = GetCollection(selection);
            Vector3 center = Vector3.zero;
            foreach(var item in selection)
            {
                center += item.transform.position;
            }
            scalerPos = center / selection.Count;
            scaler.position = scalerPos;
            firstItemScale = selection[0].transform.localScale;
        }

        public void UpdateScalesZeros(List<GameObject> selection)
        {
            for (int i = 0; i < selection.Count; i++)
            {
                selection[i].transform.localScale = RemoveZeroScale(selection[i].transform.localScale);
            }
        }
        
        public void Action(List<GameObject> selection, Tool.AxisOption handle)
        {
            MoveParent(selection);

            scaler.localScale = Vector3.one;
            axis = GetAxis(selection.Count, scaler.rotation);

            OneObject(selection);
            MultipleObjects(selection);

            scaler.position = scalerPos;
            
            UpdateScalesZeros(selection);
            RemoveParent(selection);
        }

        public void MultipleObjects(List<GameObject> selection)
        {
            if (selection.Count != 1)
            {
                
                var offset = GetOffcet(selectionDatas).Item1;
                var dir = scaler.InverseTransformDirection(offset);

                scaler.localScale = Vector3.one + dir;
            }
        }

        public void OneObject(List<GameObject> selection)
        {
            if (selection.Count == 1)
            {
                var offset = GetOffcet(selectionDatas).Item1;
                var dir = scaler.InverseTransformDirection(offset);
                
                var scale = firstItemScale;
                
                scale.Scale(Vector3.one + dir);
                
                
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    selection[0].transform.localScale = Vector3Int.RoundToInt(scale);
                }
                else
                {
                    selection[0].transform.localScale = scale;
                }

                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    var scaledDir = dir;
                    scaledDir.Scale(firstItemScale);
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        scaledDir = Vector3Int.RoundToInt(scaledDir);
                    }
                    selection[0].transform.localPosition = scaledDir/2f;
                }
                else
                {
                    selection[0].transform.localPosition = Vector3.zero;
                }
                
            }
        }

        public Vector3 RemoveZeroScale(Vector3 objScale)
        {
            var scale = 0.05f;

            for (int i = 0; i < 3; i++)
            {
                if (Mathf.Abs(objScale[i]) < scale)
                {
                    objScale[i] = scale;
                }
            }
            return objScale;
        }

        public void UpdateAction(List<GameObject> selection, GameObject gizmo)
        {
            GizmoCalculations(selection, gizmo);
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
                selection[i].transform.parent = parents[i];
            }
        }
        
        public void RemoveAction(List<GameObject> selection)
        {
           
            if (scaler != null)
            {
                Destroy(scaler.gameObject);
            }
        }
    }
}
