using System;
using System.Collections.Generic;
using Control;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CommandsList : MonoBehaviour
    {
        [SerializeField] private Transform holder;
        [SerializeField] private Button line;
        private List<LineData> lineDatas = new List<LineData>();
        
        public class LineData
        {
            [SerializeField] private Action action;
            [SerializeField] private string command;
            [SerializeField] private Button line;

            public LineData(string commandKey, Action commandValue, Button item)
            {
                command = commandKey;
                action = commandValue;
                line = item;
                item.GetComponentInChildren<TMP_Text>().text = command;
                item.gameObject.SetActive(true);
                Init();
            }


            public void Init()
            {
                line.onClick.RemoveAllListeners();
                line.onClick.AddListener(delegate { action?.Invoke(); });
            }

            public void SelfDestroy()
            {
                Destroy(line.gameObject);
            }
        }
        
        public void Init(CommandsContainer commandsContainer)
        {
            DrawCommands(commandsContainer);
            Controller.OnLookClick.AddListener(Disable);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
        public void DrawCommands(CommandsContainer commandsContainer)
        {
            foreach (var data in lineDatas)
            {
                data.SelfDestroy();
            }
            lineDatas.Clear();

            foreach (var command in commandsContainer.GetCommands())
            {
                var item = Instantiate(line, holder);
                lineDatas.Add(new LineData(command.Key, command.Value, item));
            }
        }
    }
}
