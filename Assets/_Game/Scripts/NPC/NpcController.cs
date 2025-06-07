using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using _Game.Scripts.GameStages;
using _Game.Scripts.NPC.States;
using _Game.Scripts.Utils;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Game.Scripts.NPC
{
    [RequireComponent(typeof(Animator))]
    public class NpcController : MonoBehaviour
    {
        [SerializeField] private GameObject talkHint;
        public float movementSpeed = 10f;
        
        public Vector3[] pathPositions;
        public NpcStateMachine stateMachine {get; private set;}

        [Expandable, SerializeField] private NpcData _data;
        
        public NpcData Data => _data;
        
        [HideInInspector] public UnityEvent onNpcCome = new UnityEvent();
        [HideInInspector] public UnityEvent onNpcLeave = new UnityEvent();
        
        private DialogueGraph _currentDialogueGraph;
        private Animator _animator;
        

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Init(NpcData data, Vector3[] path)
        {
            stateMachine = new NpcStateMachine(this);
            _data = data;
            pathPositions = path;
            
            SetRandomParams();
            
            stateMachine.ChangeState<ComingNpcNpcState>();
        }
        
        private void SetRandomParams()
        {
            _currentDialogueGraph = WeightedRandomizer.GetWeightedValue(_data.dialogueGraphs);
            _data.role = WeightedRandomizer.GetWeightedValue(NpcRandomDataWeights.Roles);
            _data.order = WeightedRandomizer.GetWeightedValue(NpcRandomDataWeights.Coffee);
            _data.mood = WeightedRandomizer.GetWeightedValue(NpcRandomDataWeights.Moods);
        }

        private void Update()
        {
            stateMachine.Update();
        }

        public void OnDialogueStart()
        {
            _currentDialogueGraph.Start();
        }

        private void OnDisable() => stateMachine?.currentNpcState?.OnNpcDisable();

        private void OnEnable() => stateMachine?.currentNpcState?.OnNpcEnable();

        private void OnDestroy() => stateMachine?.currentNpcState?.OnDestroy();

        
        public void ShowHint()  => talkHint.SetActive(true);
        public void HideHint()  => talkHint.SetActive(false);
        
        public void SetMood(NpcMood newMood)
        {
            _data.mood = newMood;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var labelPos = transform.position + new Vector3(0, 1.5f, 0);
            Handles.Label(labelPos, 
                "Name: " + _data.npcName + "\n" + 
                "State: " + stateMachine?.currentNpcState.ToString().Split(".").Last()
                );

        }
        #endif
    }
}