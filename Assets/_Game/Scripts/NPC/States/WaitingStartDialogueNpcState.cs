using _Game.Scripts.GameStages;
using UnityEngine;

namespace _Game.Scripts.NPC.States
{
    public class WaitingStartDialogueNpcState : INpcState
    {
        private NpcStateMachine _stateMachine;
        
        public void Enter(NpcStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            stateMachine.controller.ShowHint();
        }

        public void Exit()
        {
            _stateMachine.controller.HideHint();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.E)) _stateMachine.ChangeState<InDialogueNpcNpcState>();
        }

        public void OnDestroy()
        {
            
        }

        public void OnNpcEnable()
        {
            
        }

        public void OnNpcDisable()
        {
            
        }
    }
}