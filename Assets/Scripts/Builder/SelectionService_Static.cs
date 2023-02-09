using EPOOutline;
using UnityEngine;

namespace Builder
{
    public partial class SelectionService
    {



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
                if (str == Instance.currentTool.Name) return;
                
                Instance.currentTool.UnSelect(Instance.selected);
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

        public static Tool GetTool()
        {
            return Instance.currentTool;
        }
    }
}
