using System.Collections.Generic;
using System.Linq;
using Base.MapBuilder;
using UnityEngine;
using UnityEngine.Events;

namespace Builder
{
    public partial class SelectionService : MonoBehaviour
    {
        public enum Space
        {
            Local, Global
        }
        
        
        private static SelectionService Instance;

        [SerializeField] private List<GameObject> selected;
        [SerializeField] private List<Tool> tools;
        [SerializeField] private Space space;

        private Tool currentTool;
        private Manager manager;
        private bool gizmoHandled = false;
        
        public UnityEvent ChangeSelect = new UnityEvent();
        public UnityEvent<GameObject> DeleteSelect = new UnityEvent<GameObject>();
        public UnityEvent<GameObject> DublicateSelect = new UnityEvent<GameObject>();
        
        public List<Tool> Tools => tools;
        public List<GameObject> Selection => selected;

        public Manager Manager => manager;



        
        public void Init(Manager manager)
        {
            Instance = this;
            this.manager = manager;
            foreach (var tool in Tools)
            {
                tool.Init(this);
            }
        }

        private void Update()
        {
            SelectObjects();
            
            if (IsGizmoNotDrag())
            {
                SelectDelete();
                SelectDublicate();
                SelectToolsByKeys();
                
            }
        }

        public void SelectObjects()
        {
            if (!Manager.UIWindowsService.HaveOpenedWindowsOrUI())
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    var isGizmoMove = CalculateGizmoProcess() || gizmoHandled;

                    if (!isGizmoMove)
                    {
                        CalculateSelectionProcess();
                    }
                }else
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    if (gizmoHandled)
                    {
                        gizmoHandled = false;
                    }
                }
            }
            if (currentTool != null)
            {
                currentTool.Update(Selection, true);
            }
        }

        public void SelectToolsByKeys()
        {

            foreach (var tool in tools)
            {
                if (KeyboardService.GetDown(tool.Key))
                {
                    SelectTool(tool.Name);
                    break;
                }
            }
        }

        public void SelectDelete()
        {
            if (KeyboardService.GetDown(Keymap.Delete_Selection))
            {
                for (int i = 0; i < Selection.Count; i++)
                {
                    DeleteSelect.Invoke(Selection[i].gameObject);
                    Destroy(Selection[i].gameObject);
                }
                Selection.Clear();
            }
        }

        public void SelectDublicate()
        {
            if (KeyboardService.GetDown(Keymap.Dublicate_Selection))
            {
                List<GameObject> newItems = new List<GameObject>(Selection.Count);
                for (int i = 0; i < Selection.Count; i++)
                {
                    newItems.Add(Instantiate(Selection[i].gameObject));
                    newItems.Last().name = Selection[i].name;
                    DublicateSelect.Invoke(newItems.Last());
                }
                ClearSelect();
                for (int i = 0; i < newItems.Count; i++)
                {
                    SelectObject(newItems[i], true);
                }
            }
        }
        
        public bool IsGizmoNotDrag()
        {
            return !Input.GetKey(KeyCode.Mouse1) && tools.Find(x => x.Dragged) == null;
        }
        
        public bool CalculateGizmoProcess()
        {
            Ray ray = Manager.PlayerService.Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Gizmo")))
            {
                gizmoHandled = true;
                if (currentTool != null)
                {
                    currentTool.ActionStart(Selection);
                    
                }
                return true;
            }
            return false;
        }
        
        
        private void CalculateSelectionProcess()
        {
            Ray ray = Manager.PlayerService.Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Default", "Player")))
            {
                if (hit.collider != null)
                {
                    SelectObject(hit.transform.GetComponentInParent<BuildPart>().gameObject, Input.GetKey(KeyCode.LeftShift));
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        ClearSelect();
                    }
                }
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    ClearSelect();
                }
            }
        }
    }
}
