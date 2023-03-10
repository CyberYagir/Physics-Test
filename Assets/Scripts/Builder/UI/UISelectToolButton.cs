using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Builder.UI
{
    public class UISelectToolButton : UIController
    {
        [SerializeField] private GameObject item;
        [SerializeField] private UIButton spaceButtons;
        void Start()
        {
            spaceButtons.Init(Manager.UIWindowsService);
            foreach (var tool in Manager.SelectionService.Tools)
            {
                var it = Instantiate(item.gameObject, item.transform.parent);
                var icon = it.transform.GetChild(0).GetComponent<Image>();
                icon.sprite = tool.Icon;
                
                it.GetComponent<Button>().onClick.AddListener(delegate { SelectTool(tool.Name); });
                it.GetComponent<UIButton>().Init(Manager.UIWindowsService);
                icon.DOColor(Color.gray, 0);
                tool.ChangeToolState.AddListener(delegate(bool state)
                {
                    if (it != null)
                    {
                        icon.DOColor(state ? Color.white : Color.gray, 0.1f);
                    }
                });
            }
            item.gameObject.SetActive(false);
        }


        public void SelectTool(string str)
        {
            SelectionService.SelectTool(str);
        }

    }
}
