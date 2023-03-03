using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Builder.UI
{
    public class UIModulesManager : UIController, IPointerEnterHandler, IPointerExitHandler
    {
        [System.Serializable]
        public class TabButton
        {
            public enum TabType
            {
                Transform,
                Physics,
                Mesh,
                Light
            }

            [SerializeField] private TabType type;
            [SerializeField] private Button button;
            [SerializeField] private GameObject window;

            public TabType Type => type;

            public void SetState(bool state)
            {
                window.gameObject.SetActive(state);
            }

            public void ShowButton(bool state)
            {
                button.gameObject.SetActive(state);
            }
        }
        
        [SerializeField] private List<TabButton> modules = new List<TabButton>();


        private Dictionary<TabButton.TabType, TabButton> modulesDictionary = new Dictionary<TabButton.TabType, TabButton>(5);

        public override void Init(Manager manager)
        {
            base.Init(manager);

            foreach (var win in modules)
            {
                win.SetState(false);
                modulesDictionary.Add(win.Type, win);
            }
            
            
            
            manager.SelectionService.ChangeSelect.AddListener(UpdateButton);
            manager.SelectionService.DeleteSelect.AddListener(delegate(GameObject arg0) { UpdateButton(); });
            manager.SelectionService.DublicateSelect.AddListener(delegate(GameObject arg0) { UpdateButton(); });

            UpdateButton();
        }

        private List<GameObject> fullSelectedList = new List<GameObject>(100);

        private void UpdateButton()
        {
            modulesDictionary[TabButton.TabType.Transform].ShowButton(Manager.SelectionService.SelectionBuildParts.Count != 0);

            fullSelectedList.Clear();
            fullSelectedList.AddRange(Manager.SelectionService.SelectionBuildParts);
            fullSelectedList.AddRange(Manager.SelectionService.SelectionParts);

            modulesDictionary[TabButton.TabType.Mesh].ShowButton(fullSelectedList.Find(x => x.GetComponent<MeshRenderer>()) != null);
            modulesDictionary[TabButton.TabType.Light].ShowButton(fullSelectedList.Find(x => x.GetComponent<Light>()) != null);
            modulesDictionary[TabButton.TabType.Physics].ShowButton(Manager.SelectionService.SelectionBuildParts.Find(x => x.GetComponent<Collider>() || x.GetComponent<Rigidbody>()) != null);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Manager.UIWindowsService.Over(gameObject, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Manager.UIWindowsService.Over(gameObject, false);
        }
    }
}
