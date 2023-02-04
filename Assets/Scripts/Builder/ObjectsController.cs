using System;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;
using UnityEngine.Events;

namespace Builder
{
    public class ObjectsController : MonoBehaviour
    {
        private static ObjectsController Instance;

        [SerializeField] private List<GameObject> selected;
        
        [SerializeField] private List<Tool> tools;
        private Tool currentTool;
        private Manager manager;
        public List<Tool> Tools => tools;
        public List<GameObject> Selection => selected;

        public UnityEvent ChangeSelect = new UnityEvent();
        
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
            if (!manager.TabsManager.HaveOpenedWindowsOrUI())
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Ray ray = manager.Controller.Camera.ScreenPointToRay(Input.mousePosition);
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

                if (currentTool != null)
                {
                    currentTool.Update(Selection);
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
                Instance.currentTool.Update(Instance.selected);
            }
        }
    }
}
