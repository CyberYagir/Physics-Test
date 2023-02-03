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

        private void Awake()
        {
            rectTransform.sizeDelta = Vector2.zero;
        }

        public void OpenClose()
        {
            opened = !opened;
            rectTransform.DOKill();
            rectTransform.DOSizeDelta(opened ? finalSize : Vector2.zero, 0.2f);
        }
    }
}
