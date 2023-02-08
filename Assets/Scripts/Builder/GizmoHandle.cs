using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Builder
{
    public class GizmoHandle : MonoBehaviour
    {
        [SerializeField] private SelectionService controller;
        [SerializeField] private Axis axis;
        [SerializeField] private string tool;


        private Vector3 localScale;
        
        private bool isActive;

        public bool IsActive => isActive;


        public void Init(SelectionService controller, Axis axis, string tool)
        {
            this.controller = controller;
            this.axis = axis;
            this.tool = tool;
            localScale = transform.localScale;
        }

        public void Show()
        {
            transform.DOScale(localScale, 0.2f);   
        }
        public void Hide()
        {
            transform.DOScale(Vector3.zero, 0.2f);
        }

        private void OnMouseEnter()
        {
            transform.DOScale(localScale * 1.2f, 0.2f);
        }

        private void OnMouseExit()
        {
            transform.DOScale(localScale, 0.2f);
        }

        private void OnMouseDown()
        {
            isActive = true;    
        }

        private void OnMouseUp()
        {
            isActive = false;
        }

        
    }
}
