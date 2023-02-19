using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Base.Shell
{

    public class ConsoleService : MonoBehaviour
    {
        private static ConsoleService Instance;
        
        [System.Serializable]
        public class LogsColors
        {
            [SerializeField] private LogType type;
            [SerializeField] private Color color;
            private string colorHex;

            public string ColorHex => colorHex;

            public LogType Type => type;

            public void Init()
            {
                colorHex = "#" + ColorUtility.ToHtmlStringRGB(color);
            }
        }
        
        [SerializeField] private Canvas canvas;
        [SerializeField] private TMP_InputField input;
        [SerializeField] private TMP_Text outputText;
        [SerializeField] private List<LogsColors> colors;

        private bool cursorVisible;
        private CursorLockMode cursorMode;
        
        private RectTransform rectTransform;
        private bool isOpened;


        private List<ICommandExecutable> commands = new List<ICommandExecutable>(100);

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }

            foreach (var color in colors)
            {
                color.Init();
            }

            rectTransform = canvas.GetComponent<RectTransform>();
            canvas.enabled = false;
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0);
            ReloadShellCommands();
            input.onSubmit.AddListener(delegate(string arg0) { input.ActivateInputField(); });
            Application.logMessageReceived += ApplicationOnlogMessageReceived;
        }

        private void ApplicationOnlogMessageReceived(string condition, string stacktrace, LogType type)
        {
            ShowText(condition, type);
        }

        private void LateUpdate()
        {
            OpenClose();

            if (isOpened)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (input.text.Trim() != "")
                    {
                        ShowText(input.text, LogType.Log);
                        CalculateText(input.text);
                        input.text = "";
                    }
                }
            }
        }



        public static void ReloadShellCommands()
        {
            string nspace = "Base.Shell";

            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.IsClass && t.Namespace == nspace && t.GetInterface(nameof(ICommandExecutable)) == typeof(ICommandExecutable)
                select t;

            Instance.commands.Clear();

            foreach (var type in q)
            {
                var item = (ICommandExecutable)Activator.CreateInstance(type);
                if (item != null)
                {
                    Instance.commands.Add(item);
                }
            }
        }

        public static List<ICommandExecutable> GetCommands()
        {
            return Instance.commands;
        }

        private void CalculateText(string inputText)
        {
            var items = inputText.Trim().ToLower().Split(' ');

            if (items.Length >= 1)
            {

                var commandsClass = commands.Find(x => x.CommandsList.Find(y => y.CommandBase == items[0]) != null);
                if (commandsClass != null)
                {
                    var command = commandsClass.CommandsList.Find(y => y.CommandBase == items[0]);
                    if (command != null)
                    {
                        command.Execute(items.Skip(1).ToArray());
                        return;
                    }
                }
                
                Debug.LogError("Command not found");
            }
        }


        public void ShowText(string message, LogType type)
        {

            string str = "\n" + DateTime.Now.ToString("hh:mm:ss") +  $" - <color={colors.Find(x => x.Type == type).ColorHex}>";
            str += message;
            str += $"</color>";

            outputText.text += str;
        }
        
        private void OpenClose()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                isOpened = !isOpened;
                if (isOpened)
                {
                    rectTransform.DOKill();
                    canvas.enabled = true;
                    rectTransform.DOSizeDelta(new Vector2(rectTransform.sizeDelta.x, 500), 0.2f);
                    input.Select();
                    cursorVisible = Cursor.visible;
                    cursorMode = Cursor.lockState;
                    
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    HideConsole();
                }
            }
        }

        public static void HideConsole()
        {
            Instance.rectTransform.DOSizeDelta(new Vector2(Instance.rectTransform.sizeDelta.x, 0), 0.2f).onComplete += () =>
            {
                Instance.canvas.enabled = false;
            };

            Cursor.visible = Instance.cursorVisible;
            Cursor.lockState = Instance.cursorMode;
                    
            EventSystem.current.SetSelectedGameObject(null);

            Instance.isOpened = false;
        }
        
        public static void ClearText()
        {
            Instance.outputText.text = "";
        }
    }
}
