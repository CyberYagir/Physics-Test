using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Builder.UI
{
    public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private UITabsManager tabs;
        public void Init(UITabsManager tabs)
        {
            this.tabs = tabs;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            tabs.Over(gameObject, true);
            CursorManager.SetCursor(CursorManager.CursorState.Hand);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabs.Over(gameObject, false);
            CursorManager.SetCursor(CursorManager.CursorState.Pointer);
        }
    }
}
