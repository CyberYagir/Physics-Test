using UI;
using UnityEngine;

namespace Manager
{
    public class UIManager : Manager
    {
        [SerializeField] private CommandsContainer commandsContainer;
        [SerializeField] private CommandsList commandList;
        [SerializeField] private Inspector inspector;
        [SerializeField] private ItemsDrawer items;

        public override void Init()
        {
            base.Init();

            commandList.Init(commandsContainer);
            inspector.Init();
            items.Draw();
        }

        public void SwitchState(GameObject obj)
        {
            obj.active = !obj.active;
        }
    }
}
