using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Manager
{
    [System.Serializable]
    public class GizmoMove
    {
        [System.Serializable]
        public class Mover
        {
            [SerializeField] private Renderer rn;
            [SerializeField] private Vector3 vector;
            [SerializeField] private BoxCollider[] collider;
            [SerializeField] private BoxCollider mainCollider;
            public Transform Transform => rn.transform;
            public Material Material => rn.sharedMaterial;
            public Vector3 Vector => vector;

            public void SetMaterial(Material mat)
            {
                rn.sharedMaterial = mat;
            }

            public void SetCollider(bool state)
            {
                mainCollider.enabled = !state;
                for (int i = 0; i < collider.Length; i++)
                {
                    collider[i].enabled = state;
                }
            }
        }
        [SerializeField] private Mover x, y, z;
            
        [SerializeField] private Material selectMat;


        [SerializeField] private Transform mesh;
        [SerializeField] private float size;

        private Selector selector;
        private Camera cam;
        private LayerMask mask;
        public bool IsMoved => selectedOrt != null;


        public void Init(Camera camera, Selector selector)
        {
            this.selector = selector;
            cam = camera;
            mask = LayerMask.GetMask("Gizmo");
                
            x.SetCollider(false);
            y.SetCollider(false);
            z.SetCollider(false);
        }

        private Mover lastOrt;
        private Mover selectedOrt;
        private Material lastOrtMat;
        
        
        public void Update()
        {
            mesh.transform.localScale = Vector3.Distance(cam.transform.position, mesh.transform.position) * size * Vector3.one;

            if (selectedOrt == null)
            {
                SelectNewOrt();
            }
            else
            {
                OrtMoving();
            }
        }

        public void SelectNewOrt()
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Collide))
            {
                if (hit.collider != null)
                {
                    if (x.Transform == hit.transform)
                    {
                        SelectOrt(x);
                    }

                    if (y.Transform == hit.transform)
                    {
                        SelectOrt(y);
                    }

                    if (z.Transform == hit.transform)
                    {
                        SelectOrt(z);
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        selectedOrt = lastOrt;
                        selectedOrt.SetCollider(true);
                        ray = cam.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Collide))
                            startPoint = hit.point;
                        selector.SaveStartPositions();
                    }
                }
            }
            else
            {
                Select(lastOrt, false);
            }
        }

        private Vector3 startPoint;
        
        public void OrtMoving()
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Collide))
            {
                var deltaPos = startPoint - hit.point;
                var moveVector = new Vector3(deltaPos.x * selectedOrt.Vector.x, deltaPos.y * selectedOrt.Vector.y, deltaPos.z * selectedOrt.Vector.z);
                selector.TranslateObjects(-moveVector);
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                selector.SkipFrame();
                selectedOrt.SetCollider(false);
                selectedOrt = null;
            }
        }
            
        public void SelectOrt(Mover ort)
        {
            if (lastOrt != null)
            {
                if (lastOrt != ort)
                {
                    Select(lastOrt, false);
                }
            }
            else
            {
                lastOrt = ort;
                lastOrtMat = ort.Material;
                Select(ort, true);
            }
        }

        public void Select(Mover ort, bool select)
        {
            if (select)
            {
                ort.SetMaterial(selectMat);
            }
            else
            {
                if (ort != null)
                {
                    ort.SetMaterial(lastOrtMat);
                    lastOrt = null;
                }
            }
        }

        public void SetGizmo(bool state, List<Transform> selectedObjects, bool update = true)
        {
            mesh.gameObject.SetActive(state);
            if (!state)
            {
                return;
            }

            Vector3 middlePos = selectedObjects[0].transform.position;
            if (selectedObjects.Count > 1)
            {
                middlePos = Vector3.zero;
                foreach (var obj in selectedObjects)
                {
                    middlePos += obj.transform.position;
                }

                middlePos /= selectedObjects.Count;
            }

            mesh.transform.position = middlePos;

            if (update)
                Update();
        }
    }
}