using System.Collections.Generic;
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

            public GameObject Handler => handler;

            public Axis Axis => axis;
        }
        
        [SerializeField] private string name;
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject gizmo;
        [SerializeField] private List<AxisOption> list;

        public UnityEvent<bool> ChangeToolState = new UnityEvent<bool>();

        public Sprite Icon => icon;

        public string Name => name;

        public void Init(ObjectsController objectsController)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Handler.AddComponent<BoxCollider>();
                list[i].Handler.AddComponent<GizmoHandle>().Init(objectsController, list[i].Axis, name);
            }
            
        }


        public void Active(bool state)
        {
            gizmo.SetActive(state);
        }
        
        public virtual void Update(List<GameObject> selection)
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

                gizmo.transform.position = center / selection.Count;
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
            Active(true);
            ChangeToolState.Invoke(true);
        }
    }
}