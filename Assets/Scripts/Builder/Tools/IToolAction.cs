using System.Collections.Generic;
using UnityEngine;

namespace Builder.Tools
{
    interface IToolAction
    {
        public void StartAction(List<GameObject> selection, Tool.AxisOption handle);
        public void Action(List<GameObject> selection, Tool.AxisOption handle);

        public void UpdateAction(List<GameObject> selection, GameObject gizmo);

        public void RemoveAction(List<GameObject> selection);
    }
}