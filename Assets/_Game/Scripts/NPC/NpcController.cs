using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.NPC.States;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.NPC
{
    public class NpcController : MonoBehaviour
    {
        public float movementSpeed = 10f;
        public Vector3[] pathPositions;
        public NpcStateMachine stateMachine {get; private set;}
        
        public UnityEvent onNpcCome = new UnityEvent();
        public UnityEvent onNpcLeave = new UnityEvent();

        public void Init()
        {
            stateMachine = new NpcStateMachine();
            stateMachine.ChangeState<ComingNpcState>();
            MoveTo();
        }

        public void MoveTo()
        {
            transform.position = pathPositions.First();
            transform.DOPath(pathPositions, movementSpeed, PathType.CatmullRom, gizmoColor: Color.red)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .SetLookAt(0.1f) // Поворот в сторону движения
                .OnComplete(onNpcCome.Invoke); 
        }
        public void MoveFrom()
        {
            transform.position = pathPositions.Last();
            transform.DOPath(pathPositions.Reverse().ToArray(), movementSpeed, PathType.CatmullRom, gizmoColor: Color.red)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .SetLookAt(0.1f)
                .OnComplete(onNpcLeave.Invoke);
        }
    }
}