using Builder.UI;
using UnityEditor;
using UnityEngine;

namespace Builder.Editor
{
    [CustomEditor(typeof(UICreateWindow))]
    public class CreateUIFoldersList : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            
            var w = target as UICreateWindow;
            var items = w.Holder.Items;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] is Folder)
                {
                    RecursiveFolders("", items[i] as Folder);
                }
            }
        }

        public void RecursiveFolders(string tabs, Folder folder)
        {
            tabs += "  ";
            GUILayout.Label(tabs + "┣" + folder.Name);
            for (int i = 0; i < folder.Items.Count; i++)
            {
                if (folder.Items[i] is Folder)
                {
                    RecursiveFolders(tabs + "│", folder.Items[i] as Folder);
                }
                else
                {
                    
                    GUILayout.Label(tabs + "│"+ "  ┣" + folder.Items[i].Name);
                }
            }
            GUILayout.Label(tabs + "┗━━━");
        }
    }
}
