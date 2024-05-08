using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Playground;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(MovingUnit))]
public class MovingUnitEditor : Editor
{
    private MovingUnit movingUnit;

    private bool isButtonsShowed = false;
    private bool isOptionsShowed = false;
    
    
    private string PrefButtonsIsShowed => "_buttonsIsShowed";
    private string PrefOptionsIsShowed => "_optionsIsShowed";
    
    private void OnEnable()
    {
        movingUnit = target as MovingUnit;


        isButtonsShowed = EditorPrefs.GetBool(PrefName(PrefButtonsIsShowed), true);
        isOptionsShowed = EditorPrefs.GetBool(PrefName(PrefOptionsIsShowed), true);
    }


    private void OnDisable()
    {
        EditorPrefs.SetBool(PrefName(PrefButtonsIsShowed), isButtonsShowed);
        EditorPrefs.SetBool(PrefName(PrefOptionsIsShowed), isOptionsShowed);
    }


    public string PrefName(string str)
    {
        return movingUnit.GetInstanceID() + str;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BotControlsRegionLogic();
        ButtonsRegionLogic();
    }

    private void BotControlsRegionLogic()
    {
        
        isOptionsShowed = EditorGUILayout.Foldout(isOptionsShowed, "Options", true);


        if (isOptionsShowed)
        {
            EditorGUI.BeginChangeCheck();
            var newSpeed = EditorGUILayout.FloatField("Speed: ", movingUnit.Agent.speed);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(movingUnit.Agent, "Change speed");
                movingUnit.Agent.speed = newSpeed;
            }
        }
    }
    
    private void ButtonsRegionLogic()
    {
        isButtonsShowed = EditorGUILayout.Foldout(isButtonsShowed, "Buttons", true);


        if (isButtonsShowed)
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Shorten Path"))
                {
                    var path = movingUnit.GetPath();
                    var newPath = new List<Transform>();

                    for (int i = 0; i < path.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            newPath.Add(path[i]);
                        }
                    }


                    Undo.RecordObject(movingUnit, "Shorten Path");

                    movingUnit.SetPath(newPath);

                    EditorUtility.SetDirty(movingUnit);
                }

                if (GUILayout.Button("Reset Path"))
                {
                    var path = movingUnit.GetPath();

                    if (path.Count == 0) return;

                    var firstPoint = path[0];

                    if (firstPoint == null) return;

                    var holder = firstPoint.parent;

                    var newPath = new List<Transform>();


                    var objects = new List<Object>();

                    for (int i = 0; i < holder.childCount; i++)
                    {
                        objects.Add(holder.GetChild(i));
                    }

                    objects.Add(movingUnit);
                    Undo.RecordObjects(objects.ToArray(), "Reset");


                    for (int i = 0; i < holder.childCount; i++)
                    {
                        var child = holder.GetChild(i);
                        child.name = "Point " + i;
                        newPath.Add(child);
                    }

                    movingUnit.SetPath(newPath);

                    EditorUtility.SetDirty(movingUnit);
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
