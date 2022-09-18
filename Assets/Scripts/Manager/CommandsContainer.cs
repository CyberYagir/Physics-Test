using System;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class CommandsContainer : Manager
    {
        [SerializeField] private MainManager mainManager;
        private Dictionary<string, Action> allActions = new Dictionary<string, Action>();
        public override void Init()
        {
            allActions.Clear();
            foreach (var manager in mainManager.Managers)
            {
                if (manager != this)
                {
                    foreach (var command in manager.GetCommands())
                    {
                        allActions.Add(command.Key, command.Value);
                    }
                }
            }
        }

        public override Dictionary<string, Action> GetCommands()
        {
            return allActions;
        }
    }
}
