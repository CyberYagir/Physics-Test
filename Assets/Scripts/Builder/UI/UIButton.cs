using System.Runtime.Serialization;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Builder.UI
{
    public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private UIService _window;
        public void Init(UIService window)
        {
            this._window = window;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            _window.Over(gameObject, true);
            CursorManager.SetCursor(CursorManager.CursorState.Hand);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _window.Over(gameObject, false);
            CursorManager.SetCursor(CursorManager.CursorState.Pointer);
        }
    }
}
