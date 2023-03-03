using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Builder.UI
{
    public class UIModuleButton : UIController, IPointerEnterHandler, IPointerExitHandler
    {
        private Image image, icon;
        private TMP_Text text;

        private float animSpeed = 0.2f;
        public override void Init(Manager manager)
        {
            base.Init(manager);
            icon = transform.GetChild(0).GetComponent<Image>();
            image = GetComponent<Image>();
            text = GetComponentInChildren<TMP_Text>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            image.DOFade(1, 0.1f);
            text.DOColor(Color.gray, animSpeed);
            icon.DOColor(Color.gray, animSpeed);
            
            CursorManager.SetCursor(CursorManager.CursorState.Hand);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            image.DOFade(0, 0.1f);
            text.DOColor(Color.white, animSpeed);
            icon.DOColor(Color.white, animSpeed);
            CursorManager.SetCursor(CursorManager.CursorState.Pointer);
        }
    }
}
