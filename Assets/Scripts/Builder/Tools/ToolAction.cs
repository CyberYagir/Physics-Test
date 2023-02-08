using System.Collections.Generic;
using UnityEngine;

namespace Builder.Tools
{
    public class ToolAction : MonoBehaviour
    {
        protected Vector3 axis;
        protected Camera camera;

        public void Init(Tool.AxisOption handle)
        {
            camera = SelectionService.GetManager().PlayerService.Camera;
            
            switch (handle.Axis)
            {
                case Axis.X:
                    axis = Vector3.right;
                    break;
                case Axis.Y:
                    axis = Vector3.up;
                    break;
                case Axis.Z:
                    axis = Vector3.forward;
                    break;
                case Axis.XYZ:
                    axis = Vector3.one;
                    break;
                
            }
        }

        public (Vector3, Vector3) GetOffcet(List<SelectionData> selectionDatas)
        {
            var isGrid = Input.GetKey(KeyCode.LeftControl);
            
            Ray cameraRay = camera.ScreenPointToRay(Input.mousePosition);

            float   closestT = HandleMathUtils.ClosestPointOnRay(selectionDatas[0].Ray, cameraRay);
            Vector3 hitPoint = selectionDatas[0].Ray.GetPoint(closestT);

            var pos = selectionDatas[0].Pos;
            var posRount = selectionDatas[0].Pos;
            posRount = Vector3Int.RoundToInt(pos);
            
            Vector3 offsetNotRound = hitPoint + selectionDatas[0].InteractionOffset - pos;
            Vector3 offcetRound = Vector3Int.RoundToInt(hitPoint + selectionDatas[0].InteractionOffset - posRount);
            
            return (offsetNotRound, offcetRound);
        }

        public List<SelectionData> GetCollection(List<GameObject> selection)
        {
            var selectionDatas = new List<SelectionData>(selection.Count);
            Ray cameraRay = camera.ScreenPointToRay(Input.mousePosition);
            for (int i = 0; i < selection.Count; i++)
            {
                var pos = selection[i].transform.position;

                Vector3 raxis = GetAxis(selection.Count, selection[i].transform.rotation);
                var raxisRay = new Ray(pos, raxis);
                
                float closestT = HandleMathUtils.ClosestPointOnRay(raxisRay, cameraRay);
                Vector3 hitPoint = raxisRay.GetPoint(closestT);

                var interactionOffset = pos - hitPoint;
                
                selectionDatas.Add(new SelectionData(pos, raxisRay, interactionOffset));
            }

            return selectionDatas;
        }

        public void GizmoCalculations(List<GameObject> selection, GameObject gizmo)
        {
            Quaternion rot = Quaternion.Euler(Vector3.zero);

            if (SelectionService.GetSpace() == SelectionService.Space.Local)
            {
                if (selection.Count == 1)
                {
                    rot = selection[0].transform.rotation;
                }
            }

            gizmo.transform.rotation = Quaternion.Lerp(gizmo.transform.rotation, rot, Time.deltaTime * 20);
        }
        
        public Vector3 GetAxis(int selectedCount, Quaternion rotation)
        {
            return SelectionService.GetSpace() == SelectionService.Space.Local && selectedCount == 1
                ? rotation * axis
                : axis;
        }

    }
    
}