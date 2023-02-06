using System;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;
using UnityEngine.Events;

namespace Builder
{
    public class ObjectsController : MonoBehaviour
    {
        public enum Space
        {
            Local, Global
        }
        
        
        private static ObjectsController Instance;

        [SerializeField] private List<GameObject> selected;
        [SerializeField] private List<Tool> tools;
        [SerializeField] private Space space;
        
        private Tool currentTool;
        private Manager manager;
        private bool gizmoHandled = false;
        
        public UnityEvent ChangeSelect = new UnityEvent();
        
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
            if (!Manager.TabsManager.HaveOpenedWindowsOrUI())
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

                if (currentTool != null)
                {
                    currentTool.Update(Selection, true);
                }
            }
        }

        public bool CalculateGizmoProcess()
        {
            Ray ray = Manager.Controller.Camera.ScreenPointToRay(Input.mousePosition);
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
            Ray ray = Manager.Controller.Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Default", "Player")))
            {
                if (hit.collider != null)
                {
                    SelectObject(hit.transform.root.gameObject, Input.GetKey(KeyCode.LeftShift));
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
        
        
        
        
        
        public static void SelectObject(GameObject obj, bool multiple)
        {

            if (Instance.selected.Contains(obj) && multiple)
            {
                Instance.selected.Remove(obj);
                obj.GetComponent<Outlinable>().enabled = false;
                return;
            }
            
            if (Instance.selected != null && !multiple)
            {
                ClearSelect();
            }
            
            Instance.selected.Add(obj);
            obj.GetComponent<Outlinable>().enabled = true;
            Instance.ChangeSelect.Invoke();
        }

        public static void ClearSelect()
        {
            foreach (var item in Instance.selected)
            {
                item.GetComponent<Outlinable>().enabled = false;
            }
            Instance.selected.Clear();
            Instance.ChangeSelect.Invoke();
        }

        public static void SelectTool(string str)
        {
            if (Instance.currentTool != null)
            {
                Instance.currentTool.UnSelect();
            }
            Instance.currentTool = Instance.Tools.Find(x => x.Name == str);

            if (Instance.currentTool != null)
            {
                Instance.currentTool.Select();
                Instance.currentTool.Update(Instance.selected, false);
            }
        }
    
        public static Manager GetManager()
        {
            return Instance.manager;
        }

        public static Space GetSpace()
        {
            return Instance.space;
        }

        public static void ChangeSpace(Space space)
        {
            Instance.space = space;
        }
    }
}
