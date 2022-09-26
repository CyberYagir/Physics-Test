using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menu
{
    public class ButtonAnimations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text text;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            background.DOFade(1, 0.5f);
            text.DOColor(Color.gray, 0.5f);
            CursorManager.SetCursor(CursorManager.CursorState.Hand);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            background.DOFade(0, 0.5f);
            text.DOColor(Color.white, 0.5f);
            CursorManager.SetCursor(CursorManager.CursorState.Pointer);
        }
    }
}
