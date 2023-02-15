using System;
using DG.Tweening;
using UnityEngine;

namespace Builder.UI
{
    public class UIOpenWindow : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Vector2 finalSize;

        private Canvas canvas;
        
        private bool opened;

        public bool Opened => opened;

        private void Awake()
        {
            rectTransform.sizeDelta = Vector2.zero;
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;

        }

        public void OpenClose()
        {
            opened = !Opened;
            if (Opened)
            {
                canvas.enabled = true;
            }
            rectTransform.DOKill();
            rectTransform.DOSizeDelta(Opened ? finalSize : Vector2.zero, 0.2f).onComplete += () =>
            {
                if (!Opened)
                {
                    canvas.enabled = false;
                }
            };
        }
    }
}
