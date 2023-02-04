using System;
using DG.Tweening;
using UnityEngine;

namespace Builder.UI
{
    public class UIOpenWindow : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Vector2 finalSize;

        private bool opened;

        public bool Opened => opened;

        private void Awake()
        {
            rectTransform.sizeDelta = Vector2.zero;                    
            gameObject.SetActive(false);

        }

        public void OpenClose()
        {
            opened = !Opened;
            if (Opened)
            {
                gameObject.SetActive(true);
            }
            rectTransform.DOKill();
            rectTransform.DOSizeDelta(Opened ? finalSize : Vector2.zero, 0.2f).onComplete += () =>
            {
                if (!Opened)
                {
                    gameObject.SetActive(false);
                }
            };
        }
    }
}
