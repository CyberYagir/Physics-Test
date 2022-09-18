using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    [System.Serializable]
    public class Selector
    {
        [SerializeField] private List<Transform> selectedObjects = new List<Transform>(100);
    
                
                
        private Camera cam;
    
        private GizmoMove gizmo;
        private LayerMask mask;
        public void Init(Camera camera, GizmoMove giz)
        {
            gizmo = giz;
            cam = camera;
            mask = LayerMask.GetMask("Default");
        }

        public void AddSelection(Transform obj)
        {
            selectedObjects.Add(obj);
        }
    
        public void RemoveSelection(Transform obj)
        {
            selectedObjects.Remove(obj);
        }
    
        public void ObjectClick(Transform obj)
        {
            if (selectedObjects.Contains(obj))
            {
                selectedObjects.Remove(obj);
            }
            else
            {
                selectedObjects.Add(obj);
            }
        }
    
        public void Update()
        {
            if (skipFrame)
            {
                skipFrame = false;
                return;
            }
            if (!gizmo.IsMoved && Input.GetKeyUp(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        ObjectClick(hit.transform);
                    }
                    else
                    {
                        selectedObjects.Clear();
                        ObjectClick(hit.transform);
                    }
                    gizmo.SetGizmo(true, selectedObjects);
                }
                else
                {
                    selectedObjects.Clear();
                    gizmo.SetGizmo(false, null);
                }
            }
        }

        private List<Vector3> selectedStartPoses = new List<Vector3>();
        public void SaveStartPositions()
        {
            selectedStartPoses.Clear();
            foreach (var obj in selectedObjects)
            {
                selectedStartPoses.Add(obj.position);
            }
        }

        public void TranslateObjects(Vector3 moveVector)
        {
            for (int i = 0; i < selectedObjects.Count; i++)
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    selectedObjects[i].transform.position = selectedStartPoses[i] + (moveVector);
                }
                else
                {
                    selectedObjects[i].transform.position = Vector3Int.RoundToInt(selectedStartPoses[i] + (moveVector));
                }
            }

            gizmo.SetGizmo(true, selectedObjects, false);
        }

        private bool skipFrame = false;
        public void SkipFrame()
        {
            skipFrame = true;
        }
    }
}