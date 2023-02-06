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

            private BoxCollider box;
            private GizmoHandle gizmoHandle;

            public GizmoHandle GizmoHandle => gizmoHandle;

            public Axis Axis => axis;

            public void Init(string name, ObjectsController objectsController)
            {
                box = handler.AddComponent<BoxCollider>();
                gizmoHandle = handler.AddComponent<GizmoHandle>();
                GizmoHandle.Init(objectsController, Axis, name);
            }
        }
        
        [SerializeField] private string name;
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject gizmo;
        [SerializeField] private List<AxisOption> list;

        
        
        private ObjectsController objectsController;

        public UnityEvent<List<GameObject>, GameObject> ToolActive = new UnityEvent<List<GameObject>, GameObject>();
        public UnityEvent<List<GameObject>, AxisOption> HandleActionStart = new UnityEvent<List<GameObject>, AxisOption>();
        public UnityEvent<List<GameObject>, AxisOption> HandleAction = new UnityEvent<List<GameObject>, AxisOption>();
        public UnityEvent<bool> ChangeToolState = new UnityEvent<bool>();

        public Sprite Icon => icon;

        public string Name => name;

        private Tween animation;
        
        public void Init(ObjectsController objectsController)
        {
            this.objectsController = objectsController;
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Init(name, objectsController);
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
                GizmoPosition(selection, animate);
            }
            else
            {
                GizmoPosition(selection, false);
            }
            ToolActive.Invoke(selection, gizmo);
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
            HandleActionStart.Invoke(selection, FindAxis());
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
        
        public void UnSelect()
        {
            Active(false);
            ChangeToolState.Invoke(false);
        }

        public void Select()
        {
            Active(objectsController.Selection.Count != 0);
            ChangeToolState.Invoke(true);
        }

        
    }
}