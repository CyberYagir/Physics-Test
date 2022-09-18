using UI;
using UnityEngine;

namespace Manager
{
    public class UIManager : Manager
    {
        [SerializeField] private CommandsContainer commandsContainer;
        [SerializeField] private CommandsList commandList;

        public override void Init()
        {
            base.Init();

            commandList.Init(commandsContainer);
        }

        public void SwitchState(GameObject obj)
        {
            obj.active = !obj.active;
        }
    }
}
