using System;
using UnityEngine;

namespace Manager
{
    public class GizmoManager : Manager
    {  
       
        
        [SerializeField] private Camera camera;
        [SerializeField] private GizmoMove gizmoMove;
        [SerializeField] private Selector selector;
        public override void Init()
        {
            base.Init();
            gizmoMove.Init(camera, selector);
            selector.Init(camera, gizmoMove);
        }

        private void Update()
        {
            gizmoMove.Update();
            selector.Update();
        }
    }
}
