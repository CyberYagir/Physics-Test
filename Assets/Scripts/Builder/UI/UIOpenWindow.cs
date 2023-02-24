using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Builder.UI
{
    public class UIOpenWindow : MonoBehaviour
    {
        [SerializeField] protected RectTransform rectTransform;
        [SerializeField] private Vector2 finalSize;
        [SerializeField] private bool notFreezeControls;

        protected Canvas canvas;
        
        protected bool opened;

        public bool Opened => opened;

        public bool NotFreezeControls => notFreezeControls;
        public Canvas Canvas => canvas;

        public UnityEvent<bool> OpencChange = new UnityEvent<bool>();
        public UnityEvent<bool> AnimationEnd = new UnityEvent<bool>();

        protected void Init()
        {
            
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
        }
        
        protected virtual void Awake()
        {
            Init();
            rectTransform.sizeDelta = Vector2.zero;

        }

        public virtual void OpenClose()
        {
            opened = !Opened;
            if (Opened)
            {
                canvas.enabled = true;
            }

            if (rectTransform.DOKill() != 0)
            {
                AnimationEnd.Invoke(opened);
            };
            rectTransform.DOSizeDelta(Opened ? finalSize : Vector2.zero, 0.2f).onComplete += () =>
            {
                if (!Opened)
                {
                    canvas.enabled = false;
                }
                AnimationEnd.Invoke(opened);
            };
            OpencChange.Invoke(opened);
        }
    }
}
