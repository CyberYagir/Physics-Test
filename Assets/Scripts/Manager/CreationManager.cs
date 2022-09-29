using System;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class CreationManager : Manager
    {
        [SerializeField] private Camera camera;
        [SerializeField] private ObjectsContainer objectsContainer;
        
        
        public void SpawnObject(GameObject go)
        {
            Instantiate(go, camera.transform.position + camera.transform.forward * 5, go.transform.rotation);
        }
        
        
        
        public override Dictionary<string, Action> GetCommands()
        {
            Dictionary<string, Action> commands = new Dictionary<string, Action>();
            foreach (var obj in objectsContainer.Objects)
            {
                string command = "Spawn " + obj.Data.Name;
                if (!commands.ContainsKey(command))
                {
                    Action action = delegate { SpawnObject(obj.gameObject); };
                    commands.Add(command, action);
                }
            }

            return commands;
        }
    }
}
