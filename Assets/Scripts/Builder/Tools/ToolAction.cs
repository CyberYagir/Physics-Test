using UnityEngine;

namespace Builder.Tools
{
    public class ToolAction : MonoBehaviour
    {
        protected Vector3 startMousePos = Vector3.zero;
        protected Vector3 startMouseLocalPos = Vector3.zero;
        protected Vector3 startGizmoPos = Vector3.zero;
        protected Tool.AxisOption handle;
        public void Init(Tool.AxisOption handle)
        {
            this.handle = handle;
            var arrowTransform = handle.GizmoHandle.transform;
            startGizmoPos = arrowTransform.position;
            startMousePos = Raycast().point;
            
            startMouseLocalPos = arrowTransform.InverseTransformPoint(startMousePos);
            startMouseLocalPos.Scale(arrowTransform.localScale);
            startMouseLocalPos = new Vector3(0, 0, startMouseLocalPos.z);
        }


        public Vector3 CalculateAxisVector(Vector3 mousePos, Transform arrow)
        {
            Vector3 nextPos;
            Vector3 point = arrow.InverseTransformPoint(mousePos);
            point.Scale(arrow.localScale);

            var pos = point - startMouseLocalPos;
            nextPos = arrow.forward * pos.z;
            
            return nextPos;
        }
        
        public RaycastHit Raycast()
        {
            var camera = ObjectsController.GetManager().Controller.Camera;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Gizmo")))
            {
                Debug.DrawLine(camera.transform.position, hit.point, Color.white, 1f);
                return hit;
            }
            
            
            return new RaycastHit();
        }
    }
    
}