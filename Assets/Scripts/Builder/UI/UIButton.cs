using System.Runtime.Serialization;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Builder.UI
{
    public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private UIService window;

        public void Init(UIService window)
        {
            this.window = window;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            CursorManager.SetCursor(CursorManager.CursorState.Hand);
            if (!window) return;
            window.Over(gameObject, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CursorManager.SetCursor(CursorManager.CursorState.Pointer);
            if (!window) return;
            window.Over(gameObject, false);
        }
    }
}
