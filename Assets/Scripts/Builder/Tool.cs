using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Builder
{
    [System.Serializable]
    public class Tool
    {
        [System.Serializable]
        public class AxisOption
        {
            [SerializeField] private Axis axis;
            [SerializeField] private GameObject handler;


            private Renderer renderer;
            private MeshCollider box;

            public MeshCollider Box => box;

            private GizmoHandle gizmoHandle;

            public GizmoHandle GizmoHandle => gizmoHandle;

            public Axis Axis => axis;

            public Renderer Renderer => renderer;

            public void Init(string name, SelectionService selectionService)
            {
                box = handler.AddComponent<MeshCollider>();
                box.convex = true;
                gizmoHandle = handler.AddComponent<GizmoHandle>();
                renderer = gizmoHandle.GetComponent<Renderer>();
                GizmoHandle.Init(selectionService, Axis, name);
            }
        }
        
        [SerializeField] private string name;
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject gizmo;
        [SerializeField] private List<AxisOption> list;
        [SerializeField] private Keymap key;
        
        
        private SelectionService _selectionService;

        public UnityEvent<List<GameObject>, GameObject> ToolActive = new UnityEvent<List<GameObject>, GameObject>();
        public UnityEvent<List<GameObject>> ToolDisable = new UnityEvent<List<GameObject>>();
        public UnityEvent<List<GameObject>, AxisOption> HandleActionStart = new UnityEvent<List<GameObject>, AxisOption>();
        public UnityEvent<List<GameObject>, AxisOption> HandleAction = new UnityEvent<List<GameObject>, AxisOption>();
        public UnityEvent<bool> ChangeToolState = new UnityEvent<bool>();

        public Sprite Icon => icon;

        public string Name => name;

        public Keymap Key => key;

        public bool Dragged => dragged;

        private Tween animation;
        private bool dragged;
        
        public void Init(SelectionService selectionService)
        {
            this._selectionService = selectionService;
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Init(name, selectionService);
            }
        }


        public void Active(bool state)
        {
            if (gizmo.active != state)
            {
                if (animation != null)
                {
                    animation.Kill();
                }

                if (state == false)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].GizmoHandle.Hide();
                    }

                    animation = DOVirtual.DelayedCall(0.2f, () => { gizmo.SetActive(false); });
                }
                else
                {
                    gizmo.SetActive(state);
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].GizmoHandle.Show();
                    }
                }
            }
        }
        
        public virtual void Update(List<GameObject> selection, bool animate)
        {
            if (CalculateAction(selection))
            {
                dragged = true;
                GizmoPosition(selection, animate);
            }
            else
            {
                if (Dragged)
                {
                    dragged = false;
                    ToolDisable.Invoke(selection);
                    ActiveHandlesColliders(true);
                }
                GizmoPosition(selection, false);
            }

            ScaleGizmo(selection);
            ToolActive.Invoke(selection, gizmo);
        }

        public void ActiveHandlesColliders(bool state)
        {
            for (var i = 0; i < list.Count; i++)
            {
                list[i].Box.enabled = state;
                if (!state)
                {
                    if (!list[i].GizmoHandle.IsActive)
                    {
                        list[i].Renderer.sharedMaterial.color = GetAlphaColor(list[i].Renderer.sharedMaterial.color, 0.1f);
                    }
                }
                else
                {
                    list[i].Renderer.sharedMaterial.color = GetAlphaColor(list[i].Renderer.sharedMaterial.color, 1f);
                }
            }


            Color GetAlphaColor(Color cl, float alpha)
            {
                var color = new Color(cl.r, cl.g, cl.b, alpha);
                return color;
            }
        }

        public void ScaleGizmo(List<GameObject> selected)
        {
            if (selected.Count == 0) return;
            Vector3 center = Vector3.zero;
            foreach(var item in selected)
            {
                center += item.transform.position;
            }
            center = center / selected.Count;
            gizmo.transform.localScale = Vector3.one * ((center - _selectionService.Manager.PlayerService.Camera.transform.position).magnitude / 15f);
        }
        
        public bool CalculateAction(List<GameObject> selection)
        {
            var axis = FindAxis();
            if (axis != null)
            {
                HandleAction.Invoke(selection,axis);
                return true;
            }

            return false;
        }

        public void ActionStart(List<GameObject> selection)
        {
            var axis = FindAxis();
            if (axis != null)
            {
                HandleActionStart.Invoke(selection, axis);
                ActiveHandlesColliders(false);
            }
        }

        public AxisOption FindAxis()
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].GizmoHandle.IsActive)
                {
                    return list[i];
                }
            }

            return null;
        }
        public void GizmoPosition(List<GameObject> selection, bool animate)
        {
            if (selection.Count == 0)
            {            
                Active(false);
            }
            else
            {
                Vector3 center = Vector3.zero;
                foreach(var item in selection)
                {
                    center += item.transform.position;
                }

                if (animate)
                {
                    gizmo.transform.position = Vector3.Lerp(gizmo.transform.position, center / selection.Count, 50 * Time.deltaTime);
                }
                else
                {
                    gizmo.transform.position = center / selection.Count;
                }

                Active(true);
            }
        }
        
        public void UnSelect(List<GameObject> instanceSelected)
        {
            Active(false);
            ToolDisable.Invoke(instanceSelected);
            ChangeToolState.Invoke(false);
        }

        public void Select()
        {
            Active(_selectionService.Selection.Count != 0);
            ChangeToolState.Invoke(true);
        }

        
    }
}