using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Base.Shell
{
    public class ConsoleSceneManagement : ICommandExecutable
    {
        public List<ConsoleCommandData> commands = new List<ConsoleCommandData>();
        public List<ConsoleCommandData> CommandsList => commands;
        
        public ConsoleSceneManagement()
        {
            LoadSceneCommand();
            UnloadSceneCommand();
            ListScenesCommand();
        }
        public void LoadSceneCommand()
        {
            var command = new ConsoleCommandData("/sceneload", new List<Argument>()
            {
                new Argument("sceneName", ArgumentType.String),
                new Argument("async", ArgumentType.Bool),
                new Argument("addative", ArgumentType.Bool),
            });
            
            
            command.Action.AddListener(delegate(ArgumentsShell args)
            {
                ConsoleService.HideConsole();
                if (args.GetBool("async"))
                {
                    SceneManager.LoadScene(args.GetString("sceneName"), args.GetBool("addative") ? LoadSceneMode.Additive : LoadSceneMode.Single);
                }
                else
                {
                    SceneManager.LoadSceneAsync(args.GetString("sceneName"), args.GetBool("addative") ? LoadSceneMode.Additive : LoadSceneMode.Single);
                }

            });
            
            
            commands.Add(command);
        }
        
        public void UnloadSceneCommand()
        {
            var command = new ConsoleCommandData("/sceneunload", new List<Argument>()
            {
                new Argument("sceneName", ArgumentType.String)
            });
            
            
            command.Action.AddListener(delegate(ArgumentsShell args)
            {
                SceneManager.UnloadSceneAsync(args.GetString("sceneName"));
            });
            
            
            commands.Add(command);
        }

        public void ListScenesCommand()
        {
            var command = new ConsoleCommandData("/scenelist", new List<Argument>(){});
            
            
            command.Action.AddListener(delegate(ArgumentsShell args)
            {
                List<string> scenes = new List<string>()
                {
                    "Menu", "Game", "Builder"
                };


                for (int i = 0; i < scenes.Count; i++)
                {
                    Debug.Log(" > `" + scenes[i] + "`");
                }
            });
            
            
            commands.Add(command);
        }
    }
}
