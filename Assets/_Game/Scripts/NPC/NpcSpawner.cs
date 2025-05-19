using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.NPC
{
    public class NpcSpawner : MonoBehaviour
    {
        
        [SerializeField] private GameObject[] npcs;
        [SerializeField] private GameObject npcPathRoot;

        [HideInInspector] public UnityEvent onNextNpc;
        
        public NpcController SpawnNext()
        {
            G.currentNpcIndex++;
            NpcController newNpc = Instantiate(npcs[G.currentNpcIndex]).GetComponent<NpcController>();

            newNpc.pathPositions = new Vector3[npcPathRoot.transform.childCount];
            for (int i = 0; i < npcPathRoot.transform.childCount; i++)
            {
                newNpc.pathPositions[i] = npcPathRoot.transform.GetChild(i).position;
            }
            newNpc.Init();
            return newNpc;
        }
        
        private void OnNextNpc()
        {
            if (!G.currentNpc) // Удаляем NPC если уже был заспавнен
                Destroy(G.currentNpc.gameObject);
        
            G.currentNpc = SpawnNext();
        }
        
    }
}