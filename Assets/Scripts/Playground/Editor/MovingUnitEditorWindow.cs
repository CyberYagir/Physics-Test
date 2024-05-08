using System.Linq;
using Playground;
using UnityEditor;
using UnityEngine;

public class MovingUnitEditorWindow : EditorWindow
{

    private static bool IsPointsShowed = false;
    private float radius = 5;
    
    [MenuItem("Tools/Unit Path Generator")]
    public static void ShowWindow()
    {
        var window = GetWindow<MovingUnitEditorWindow>();

        Texture2D iconTexture = EditorGUIUtility.Load("Icons/unityseo.jpg") as Texture2D;
        window.titleContent = new GUIContent("Unit Path Generator", iconTexture);
    }

    private void OnGUI()
    {
        if (Selection.activeObject is MovingUnit unit)
        {
            PointsLogic(unit);

            radius = EditorGUILayout.FloatField("Radius: ", radius);

            if (GUILayout.Button("Arrange Points"))
            {
                var path = unit.GetPath();

                for (int i = 0; i < path.Count; i++)
                {
                    var percent = i / (float) path.Count;
                    var rad = percent * 2 * Mathf.PI;

                    var x = Mathf.Sin(rad);
                    var y = Mathf.Cos(rad);


                    path[i].localPosition = new Vector3(x, 0, y) * radius;
                }
            }
            
        }
        else
        {
            EditorGUILayout.LabelField("SELECT UNIT",
                new GUIStyle()
                {
                    alignment = TextAnchor.MiddleCenter,
                    normal = new GUIStyleState()
                    {
                        textColor = GUI.skin.label.normal.textColor
                    }
                }
            );

            var units = FindObjectsOfType<MovingUnit>();
            
            for (int i = 0; i < units.Length; i++)
            {
                if (GUILayout.Button(units[i].name))
                {
                    Selection.activeObject = units[i];
                }
            }
        }
    }

    private static void PointsLogic(MovingUnit unit)
    {
        IsPointsShowed = EditorGUILayout.Foldout(IsPointsShowed, "Points", true);

        if (IsPointsShowed)
        {
            var path = unit.GetPath();

            for (int i = 0; i < path.Count; i++)
            {
                path[i] = (Transform) EditorGUILayout.ObjectField(path[i].gameObject.name, path[i], typeof(Transform), allowSceneObjects: true);
            }

            if (path.Count != 0)
            {
                if (path[0] != null)
                {
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("+"))
                        {
                            var newPoint = new GameObject();
                            newPoint.transform.parent = path[0].transform.parent;
                            newPoint.transform.localPosition = Vector3.zero;
                            newPoint.gameObject.name = "Point " + path.Count;
                            Undo.RegisterCreatedObjectUndo(newPoint, "Creating Point");
                            Undo.RecordObject(unit, "Add new Point");

                            path.Add(newPoint.transform);
                        }

                        if (GUILayout.Button("-"))
                        {
                            Undo.DestroyObjectImmediate(path.Last().gameObject);
                            Undo.RecordObject(unit, "Remove Point");
                            path.Remove(path.Last());
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }
    }
}
