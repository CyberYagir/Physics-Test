using UI;
using UnityEngine;

namespace Manager
{
    public class UIManager : Manager
    {
        [SerializeField] private CommandsContainer commandsContainer;
        [SerializeField] private CommandsList commandList;
        [SerializeField] private Inspector inspector;

        public override void Init()
        {
            base.Init();

            commandList.Init(commandsContainer);
            inspector.Init();
        }

        public void SwitchState(GameObject obj)
        {
            obj.active = !obj.active;
        }
    }
}
