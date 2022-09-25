using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Manager
{
    public class GizmoManager : Manager
    {  
       
        
        [SerializeField] private Camera camera;
        [SerializeField] private GizmoMove gizmoMove;
        [SerializeField] private Selector selector;

        public UnityEvent<List<Transform>> OnSelectObject = new UnityEvent<List<Transform>>();
        public override void Init()
        {
            base.Init();
            gizmoMove.Init(camera, selector);
            selector.Init(camera, gizmoMove, this);
        }

        private void Update()
        {
            gizmoMove.Update();
            selector.Update();
        }
    }
}
