using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Builder.UI
{
    public class UICreateButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
            
        public void OnPointerEnter(PointerEventData eventData)
        {
            Enter();
            CursorManager.SetCursor(CursorManager.CursorState.Hand);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Exit();
            CursorManager.SetCursor(CursorManager.CursorState.Pointer);
        }


        public virtual void Enter(){}

        public virtual void Exit(){}
    }
}
