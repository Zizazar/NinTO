using _Game.Scripts.NPC;

namespace _Game.Scripts.GameStages
{
    public class NpcStateMachine
    {
        public INpcState currentNpcState { get; private set; }
        public NpcController controller { get; private set; }

        public NpcStateMachine(NpcController controller)
        {
            this.controller = controller;
        }

        // <summary>
        // Generic класс для смены состояния.
        // </summary>
        public void ChangeState<T>() where T : INpcState, new()
        {
            currentNpcState?.Exit();
            currentNpcState = new T();
            currentNpcState?.Enter(this);
        }

        public void Update()
        {
            currentNpcState?.Update();
        }
    }
}