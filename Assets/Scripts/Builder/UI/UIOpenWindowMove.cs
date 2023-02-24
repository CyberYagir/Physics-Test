using DG.Tweening;
using UnityEngine;

namespace Builder.UI
{
    public class UIOpenWindowMove : UIOpenWindow
    {
        private Vector3 startPos;
        [SerializeField] private Vector3 nextPos;
        public override void OpenClose()
        {
            opened = !Opened;
            if (Opened)
            {
                canvas.enabled = true;
            }

            if (rectTransform.DOKill() != 0)
            {
                AnimationEnd.Invoke(Opened);
            }

            rectTransform.DOAnchorPos(Opened ? nextPos : startPos, 0.2f).onComplete += () =>
            {
                if (!Opened)
                {
                    canvas.enabled = false;
                }
                AnimationEnd.Invoke(Opened);
            };
            OpencChange.Invoke(opened);
        }

        protected override void Awake()
        {
            Init();
            startPos = rectTransform.anchoredPosition;
        }
    }
}
