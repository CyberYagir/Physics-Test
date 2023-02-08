using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Builder.UI
{
    public class UIChangeMatrix : MonoBehaviour
    {
        [System.Serializable]
        public class Icon
        {
            [SerializeField] private Sprite icon;
            [SerializeField] private SelectionService.Space space;
            [SerializeField] private string name;

            public string Name => name;

            public SelectionService.Space Space => space;

            public Sprite TypeIcon => icon;
        }


        [SerializeField] private List<Icon> icons;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;

        [SerializeField] private SelectionService.Space space = SelectionService.Space.Local;

        private void Start()
        {
            space = SelectionService.GetSpace();
            UpdateIcon();
        }

        private void Update()
        {
            if (KeyboardService.GetDown(Keymap.Tool_Space))
            {
                Click();
            }
        }

        public void Click()
        {
            space = SelectionService.GetSpace();
            if (space == SelectionService.Space.Local)
            {
                space = SelectionService.Space.Global;
            }
            else
            {
                space = SelectionService.Space.Local;
            }

            SelectionService.ChangeSpace(space);
            UpdateIcon();
        }

        public void UpdateIcon()
        {
            var icon = icons.Find(x => x.Space == space);
            text.text = icon.Name;
            image.sprite = icon.TypeIcon;
        }
    }
}
