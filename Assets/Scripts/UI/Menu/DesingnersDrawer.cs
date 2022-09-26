using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menu
{
    public class DesingnersDrawer : MonoBehaviour
    {
        private MenuManager menu;
        [SerializeField] private RectTransform item, holder;
        [SerializeField] private RectTransform viewport, scroll;
        [SerializeField] private Image fader;

        private List<string> desingsList = new List<string>(20);

        public void Init(MenuManager manager)
        {
            menu = manager;

            var desings = Directory.GetDirectories(menu.FileSystem.DesingsFolder);

            foreach (var desing in desings)
            {
                if (Directory.GetFiles(desing).ToList().Find(x => Path.GetExtension(x) == ".des") != null)
                {
                    desingsList.Add(desing);
                }
            }

            foreach (var desItem in desingsList)
            {
                var it = Instantiate(item.gameObject, holder);
                it.gameObject.SetActive(true);
            }

            viewport.sizeDelta = new Vector2(viewport.sizeDelta.x, Mathf.Clamp(desingsList.Count, 0, 4) * (item.sizeDelta.y + 10));
            scroll.sizeDelta = viewport.sizeDelta;
        }


        public void OpenDesignerScene(Button btn)
        {
            btn.interactable = false;
            
            fader.enabled = true;
            fader.DOFade(1, 0.5f).onComplete += () => SceneManager.LoadScene("MapBuilder");
        }
    }
}
