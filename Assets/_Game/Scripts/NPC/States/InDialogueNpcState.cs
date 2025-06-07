using _Game.Scripts.GameStages;

namespace _Game.Scripts.NPC.States
{
    public class InDialogueNpcNpcState : INpcState
    {
        public void Enter(NpcStateMachine stateMachine)
        {
            stateMachine.controller.OnDialogueStart();
        }

        public void Exit() { }

        public void Update() { }

        public void OnDestroy() { }

        public void OnNpcEnable() { }

        public void OnNpcDisable() { }
    }
}