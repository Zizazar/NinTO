using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace _Game.Scripts.NPC
{
    public class NpcSpawner : MonoBehaviour
    {
        [SerializeField] private List<NpcData> npcs;
        [SerializeField] private GameObject npcPathRoot;

        [HideInInspector] public UnityEvent onNextNpc;
        public NpcController current;

        private Vector3[] _pathPositions;
        
        public void Init()
        {
            npcs = npcs.OrderBy( x => Random.value ).ToList();
            
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
            
            G.currentNpcIndex++;
            
            var newNpcData = npcs[G.currentNpcIndex];
            NpcController newNpc = Instantiate(newNpcData.prefab).GetOrAddComponent<NpcController>();
            
            newNpc.Init(newNpcData, _pathPositions);
            
            current = newNpc;
            return newNpc;
        }

        private void DespawnCurrent()
        {
            if (current)
                Destroy(current.gameObject);
        }
        
    }
}