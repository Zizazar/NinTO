using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using _Game.Scripts.NPC.States;
using _Game.Scripts.Utils;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.NPC
{
    [RequireComponent(typeof(Animator))]
    public class NpcController : MonoBehaviour
    {
        public float movementSpeed = 10f;
        public Vector3[] pathPositions;
        public NpcStateMachine stateMachine {get; private set;}

        [Expandable, SerializeField] private NpcData _data;
        
        public NpcData Data => _data;
        
        [HideInInspector] public UnityEvent onNpcCome = new UnityEvent();
        [HideInInspector] public UnityEvent onNpcLeave = new UnityEvent();
        
        private Tweener _moveTweener;
        private Animator _animator;
        private DialogueGraph _currentDialogueGraph;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Init(NpcData data, Vector3[] path)
        {
            stateMachine = new NpcStateMachine();
            _data = data;
            pathPositions = path;
            
            SetRandomParams();
            
            MoveTo();
        }
        
        private void SetRandomParams()
        {
            _currentDialogueGraph = WeightedRandomizer.GetWeightedValue(_data.dialogueGraphs);
            _data.role = WeightedRandomizer.GetWeightedValue(NpcRandomDataWeights.Roles);
            _data.order = WeightedRandomizer.GetWeightedValue(NpcRandomDataWeights.Coffee);
            _data.mood = WeightedRandomizer.GetWeightedValue(NpcRandomDataWeights.Moods);
        }

        public void OnDialogueStart()
        {
            _currentDialogueGraph.Start();
        }

        private void OnDisable()
        {
            // Останавливаем движение нпс если на паузе
            _moveTweener.Pause();
        }

        private void OnEnable()
        {
            _moveTweener.Play();
        }

        private void OnDestroy()
        {
            _moveTweener?.Kill();
        }

        public void SetMood(NpcMood newMood)
        {
            _data.mood = newMood;
        }

        public void MoveTo()
        {
            stateMachine.ChangeState<ComingNpcState>();
            
            transform.position = pathPositions.First();
            _moveTweener = transform.DOPath(pathPositions, movementSpeed, PathType.CatmullRom, gizmoColor: Color.red)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .SetLookAt(0.1f) // Поворот в сторону движения
                .OnComplete(onNpcCome.Invoke); 
        }
        public void MoveFrom()
        {
            stateMachine.ChangeState<LeavingCoffeeNpcState>();
            
            transform.position = pathPositions.Last();
            _moveTweener = transform.DOPath(pathPositions.Reverse().ToArray(), movementSpeed, PathType.CatmullRom, gizmoColor: Color.red)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .SetLookAt(0.1f)
                .OnComplete(onNpcLeave.Invoke);
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var labelPos = transform.position + new Vector3(0, 1.5f, 0);
            Handles.Label(labelPos, 
                "Name: " + _data.npcName + "\n" + 
                "State: " + stateMachine?.CurrentState.ToString().Split(".").Last()
                );

        }
        #endif
    }
}