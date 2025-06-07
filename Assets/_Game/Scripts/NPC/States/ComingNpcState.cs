using System.Linq;
using _Game.Scripts.GameStages;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.NPC.States
{
    public class ComingNpcNpcState : INpcState
    {
        private Tweener _moveTweener;
        private NpcController _controller;
        private NpcStateMachine _stateMachine;
        
        public void Enter(NpcStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _controller = stateMachine.controller;
            
            _controller.transform.position = _controller.pathPositions.First();
            _moveTweener = _controller.transform.DOPath(_controller.pathPositions, _controller.movementSpeed, PathType.CatmullRom, gizmoColor: Color.red)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .SetLookAt(0.1f) // Поворот в сторону движения
                .OnComplete(OnNpcCome);
        }

        private void OnNpcCome()
        {
            _stateMachine.ChangeState<WaitingStartDialogueNpcState>();
        }

        public void Exit()
        {
            _moveTweener?.Kill();
        }

        public void Update() { }

        public void OnDestroy()
        {
            _moveTweener?.Kill();
        }

        public void OnNpcEnable()
        {
            _moveTweener?.Play();
        }

        public void OnNpcDisable()
        {
            _moveTweener?.Pause();
        }
    }
}