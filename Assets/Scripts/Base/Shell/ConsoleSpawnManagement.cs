using System.Collections.Generic;
using System.Linq;
using Base.MapBuilder;
using Builder;
using Builder.UI;
using EPOOutline;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Base.Shell
{
    public class ConsoleSpawnManagement : ICommandExecutable
    {
        public List<ConsoleCommandData> commands = new List<ConsoleCommandData>();
        public List<ConsoleCommandData> CommandsList => commands;


        private Camera camera;

        public ConsoleSpawnManagement()
        {
            SpawnItemCommand();
            DestroyItemCommand();
            SceneHierarchyCommand();
        }

        public Camera GetCamera()
        {
            if (camera == null)
            {
                camera = Camera.main;
            }

            return camera;
        }

        public void SpawnItemCommand()
        {
            
            var command = new ConsoleCommandData("/spawn", new List<Argument>()
            {
                new Argument("objectName", ArgumentType.String),
                new Argument("fromGameResources", ArgumentType.Bool)
            });
            
            command.Action.AddListener(delegate(ArgumentsShell args)
            {
                var mainCamera = GetCamera();
                if (mainCamera != null)
                {
                    if (!args.GetBool("fromGameResources"))
                    {
                        var item = ItemsService.GetItem(args.GetString("objectName"));
                        if (item != null)
                        {
                            var buildPart = item.gameObject.GetComponent<BuildPart>();
                            if (Builder.Manager.Instance)
                            {
                                Builder.Manager.Instance.PlayerService.SpawnItem(item.gameObject, ItemsUtility.GetName(item.PartName));
                            }
                            else
                            {
                                Object.Instantiate(item.gameObject, camera.transform.position + camera.transform.forward * 2f, Quaternion.identity);
                            }
                        }
                        else
                        {
                            Debug.LogError("BuildParts collection unavailable");
                        }
                    }
                    else
                    {
                        var item = Resources.Load(args.GetString("objectName")) as GameObject;
                        if (item != null)
                        {
                            Object.Instantiate(item.gameObject, camera.transform.position + camera.transform.forward * 2f, Quaternion.identity);
                        }
                        else
                        {
                            Debug.LogError("Resource not found or not GameObject");
                        }
                    }
                }
            });
            
            commands.Add(command);
        }

        public void DestroyItemCommand()
        {
            var command = new ConsoleCommandData("/destroy", new List<Argument>()
            {
                new Argument("sceneObjectPath", ArgumentType.String),
            });
            
            command.Action.AddListener(delegate(ArgumentsShell args)
            {
                var path = args.GetString("sceneObjectPath");
                var array = path.Split('/');
                int nextName = 1;

                if (array.Length != 0)
                {
                    GameObject point = SceneManager.GetActiveScene().GetRootGameObjects().ToList().Find(x => x.name.ToLower() == array.First().ToLower());
                    if (point != null)
                    {
                        if (point.name.ToLower() == array.Last())
                        {
                            Object.Destroy(point.gameObject);
                            return;
                        }
                    }

                    while (point != null && nextName < array.Length)
                    {
                        for (int i = 0; i < point.transform.childCount; i++)
                        {
                            if (point.transform.GetChild(i).name.ToLower() == array[nextName].ToLower())
                            {
                                point = point.transform.GetChild(i).gameObject;
                                if (point.name.ToLower() == array.Last())
                                {
                                    Object.Destroy(point.gameObject);
                                    return;
                                }
                                nextName++;
                                continue;
                            }
                        }

                        point = null;
                    }

                    if (point != null)
                    {
                        
                        Debug.LogError($"Object '{array.Last()}' not found");
                    }
                    else
                    {
                        Debug.LogError($"Object '{array.Last()}' not found");
                    }
                }
                else
                {
                    Debug.Log("Empty Path");
                }


            });
            commands.Add(command);
            
            
            
        }

        public void SceneHierarchyCommand()
        {
            var command = new ConsoleCommandData("/sceneshow", new List<Argument>());
            
            command.Action.AddListener(delegate(ArgumentsShell arg)
            {
                string list = "";
                foreach (var item in SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    PrintChildrens(item.transform, 0);
                }
                Debug.Log(list);
                
                void PrintChildrens(Transform item, int tabs)
                {
                    string str = "";
                    for (int i = 0; i < tabs; i++)
                    {
                        str += "  ";
                    }
                    foreach (Transform child in item)
                    {
                        list += str  + child.name + "\n";
                        PrintChildrens(child, tabs + 1);
                    }
                }
            });
            
            commands.Add(command);
        }
    }
}
