using System;
using System.Collections.Generic;
using System.Linq;
using GoT._Game.Scripts.Utils;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Game.Scripts.NPC
{
    public class NpcSpawner : MonoBehaviour
    {
        [SerializeField, CharacterSelector] private List<NpcData> availableNpcs;
        [SerializeField] private GameObject npcPathRoot;
        
        private Stack<NpcData> _npcToSpawn;
        
        public NpcController current;

        private Vector3[] _pathPositions;

        public void Awake()
        {
            G.npcSpawner = this;
        }

        public void Init()
        {
            _npcToSpawn = new Stack<NpcData>(availableNpcs.OrderBy( x => Random.value ).ToArray());
            
            // Получаем все позиции путя из root 
            _pathPositions = new Vector3[npcPathRoot.transform.childCount];
            for (int i = 0; i < npcPathRoot.transform.childCount; i++)
            {
                _pathPositions[i] = npcPathRoot.transform.GetChild(i).position;
            }
        }

        [Button("Spawn next NPC")]
        public NpcController SpawnNext()
        {
            DespawnCurrent();
            
            var newNpcData = _npcToSpawn.Pop();
            NpcController newNpc = Instantiate(newNpcData.prefab).GetOrAddComponent<NpcController>();
            
            newNpc.Init(newNpcData, _pathPositions);
            
            current = newNpc;
            return newNpc;
        }

        private void DespawnCurrent()
        {
            if (current)
            {
                Destroy(current.gameObject);
                current = null;
            }
        }
        
    }
}