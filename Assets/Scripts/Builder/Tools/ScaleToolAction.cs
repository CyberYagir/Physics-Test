using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace Builder.Tools
{
    public class ScaleToolAction : ToolAction
    {
        
        // private List<Vector3> selectionStartScales = new List<Vector3>(100);
        // private List<Vector3> selectionStartPoses = new List<Vector3>(100);
        //
        //
        // public void StartAction(List<GameObject> selection, Tool.AxisOption handle)
        // {
        //     handle.GizmoHandle.SetMainCollider(false);
        //
        //     selectionStartScales.Clear();
        //     selectionStartPoses.Clear();
        //     
        //     startMousePos = Raycast().point;
        //
        //     foreach (var item in selection)
        //     {
        //         selectionStartScales.Add(item.transform.localScale);
        //         selectionStartPoses.Add(item.transform.position);
        //     }
        //
        //     handle.GizmoHandle.SetMainCollider(true);
        // }
        //
        // public void Action(List<GameObject> selection, Tool.AxisOption handle)
        // {
        //     handle.GizmoHandle.SetMainCollider(false);
        //     var cast = Raycast();
        //     if (cast.collider != null)
        //     {
        //         var currentMousePos = cast.point;
        //         var delta = currentMousePos - startMousePos;
        //         CalculateAction(selection, CalculateAxisVector(handle.Axis, delta, selection.Last().transform), handle.Axis);
        //     }
        //
        //     handle.GizmoHandle.SetMainCollider(true);
        // }
        //
        // public void UpdateAction(List<GameObject> selection, GameObject gizmo)
        // {
        //     
        // }
        //
        //
        // public void CalculateAction(List<GameObject> selection, Vector3 nextPos, Axis axis)
        // {
        //     var isGrid = Input.GetKey(KeyCode.LeftControl);
        //     var isMove = Input.GetKey(KeyCode.LeftAlt);
        //     for (int i = 0; i < selection.Count; i++)
        //     {
        //         selection[i].transform.position = selectionStartPoses[i];
        //         selection[i].transform.localScale = selectionStartScales[i] + nextPos;
        //
        //         if (isGrid)
        //         {
        //             var scale = selection[i].transform.localScale;
        //             var pos = selection[i].transform.position;
        //             switch (axis)
        //             {
        //                 case Axis.X:
        //                     scale = new Vector3(Mathf.RoundToInt(scale.x), scale.y, scale.z);
        //                     pos = new Vector3(Mathf.RoundToInt(pos.x), pos.y, pos.z);
        //                     break;
        //                 case Axis.Y:
        //                     scale = new Vector3(scale.x, Mathf.RoundToInt(scale.y), scale.z);
        //                     pos = new Vector3(pos.x, Mathf.RoundToInt(pos.y), pos.z);
        //                     break;
        //                 case Axis.Z:
        //                     scale = new Vector3(scale.x, scale.y, Mathf.RoundToInt(scale.z));
        //                     pos = new Vector3(pos.x, pos.y, Mathf.RoundToInt(pos.z));
        //                     break;
        //             }
        //
        //             selection[i].transform.localScale = scale;
        //             selection[i].transform.position = pos;
        //         }
        //         if (isMove)
        //         {
        //             selection[i].transform.position = selectionStartPoses[i] + (isGrid ? Vector3Int.RoundToInt((nextPos / 2f)) : (nextPos / 2f));
        //         }
        //     }
        // }

        // public void OnSelect(List<GameObject> selection, GameObject gizmo)
        // {
        //     if (selection.Count == 1)
        //     {
        //         gizmo.transform.DORotateQuaternion(selection[0].transform.rotation, 0.2f);
        //     }
        //     else
        //     {
        //         gizmo.transform.DORotateQuaternion(quaternion.Euler(Vector3.zero), 0.2f);
        //     }
        // }
    }
}
