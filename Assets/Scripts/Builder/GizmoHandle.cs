using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Builder
{
    public class GizmoHandle : MonoBehaviour
    {
        [SerializeField] private ObjectsController controller;
        [SerializeField] private Axis axis;
        [SerializeField] private string tool;


        private Vector3 localScale;
        
        private bool isActive;

        private List<BoxCollider> colliders = new List<BoxCollider>(10);
        private Collider collider;
        public bool IsActive => isActive;


        public void Init(ObjectsController controller, Axis axis, string tool)
        {
            this.controller = controller;
            this.axis = axis;
            this.tool = tool;

            foreach (Transform chil in transform)
            {
                var col = chil.gameObject.GetComponent<BoxCollider>();
                if (col)
                {
                    colliders.Add(col);   
                }
            }

            collider = GetComponent<Collider>();
            
            localScale = transform.localScale;
        }

        public void Show()
        {
            transform.DOScale(localScale, 0.2f);         
            SetColliers();
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
            SetColliers();
        }

        private void OnMouseUp()
        {
            isActive = false;
            SetColliers();
        }


        public void SetColliers()
        {
            for (var i = 0; i < colliders.Count; i++)
            {
                colliders[i].gameObject.SetActive(isActive);
            }
        }
        
        public void SetMainCollider(bool state)
        {
            collider.enabled = state;
        }
    }
}
