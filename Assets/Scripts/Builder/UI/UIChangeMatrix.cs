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
            [SerializeField] private ObjectsController.Space space;
            [SerializeField] private string name;

            public string Name => name;

            public ObjectsController.Space Space => space;

            public Sprite TypeIcon => icon;
        }


        [SerializeField] private List<Icon> icons;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;

        [SerializeField] private ObjectsController.Space space = ObjectsController.Space.Local;

        private void Start()
        {
            space = ObjectsController.GetSpace();
            UpdateIcon();
        }

        public void Click()
        {
            space = ObjectsController.GetSpace();
            if (space == ObjectsController.Space.Local)
            {
                space = ObjectsController.Space.Global;
            }
            else
            {
                space = ObjectsController.Space.Local;
            }

            ObjectsController.ChangeSpace(space);
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
