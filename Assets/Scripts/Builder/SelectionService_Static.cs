using System.Collections.Generic;
using Base.MapBuilder;
using EPOOutline;
using UnityEngine;

namespace Builder
{
    public partial class SelectionService
    {
        
        public static void SelectObject(GameObject obj, bool multiple)
        {

            bool isBuildPart = obj.GetComponent<BuildPart>() != null;
            
            if (isBuildPart)
            {
                if (Instance.selectedBuildParts.Contains(obj) && multiple)
                {
                    Instance.selectedBuildParts.Remove(obj);
                    obj.GetComponent<Outlinable>().enabled = false;
                    Instance.ChangeSelect.Invoke();
                    return;
                }

                if (Instance.selectedBuildParts != null && !multiple)
                {
                    ClearSelect();
                }

                Instance.selectedBuildParts.Add(obj);
                obj.GetComponent<Outlinable>().enabled = true;
                Instance.ChangeSelect.Invoke();
            }
            else
            {
                if (Instance.selectedParts.Contains(obj) && multiple)
                {
                    Instance.selectedParts.Remove(obj);
                    Instance.ChangeSelect.Invoke();
                    return;
                }

                if (Instance.selectedParts != null && !multiple)
                {
                    ClearSelect();
                }

                Instance.selectedParts.Add(obj);
                Instance.ChangeSelect.Invoke();
            }
        }

        public static void ClearSelect()
        {
            foreach (var item in Instance.selectedBuildParts)
            {
                var outlinable = item.GetComponent<Outlinable>();
                if (outlinable != null)
                {
                    outlinable.enabled = false;
                }
            }
            Instance.selectedParts.Clear();
            Instance.selectedBuildParts.Clear();
            Instance.ChangeSelect.Invoke();
        }

        public static void SelectTool(string str)
        {
            
            if (Instance.currentTool != null)
            {
                if (str == Instance.currentTool.Name) return;
                
                Instance.currentTool.UnSelect(Instance.selectedBuildParts);
            }

            Instance.currentTool = Instance.Tools.Find(x => x.Name == str);

            if (Instance.currentTool != null)
            {
                Instance.currentTool.Select();
                Instance.currentTool.Update(Instance.selectedBuildParts, false);
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

        public static Tool GetTool()
        {
            return Instance.currentTool;
        }

        public static List<GameObject> GetSelected(bool additional)
        {
            if (Instance != null)
            {
                return additional ? Instance.SelectionBuildParts : Instance.SelectionParts;
            }

            return new List<GameObject>();
        }
    }
}
