using System;
using _Game.Legacy.DialogueSystem;
using _Game.Scripts.NPC;
using _Game.Scripts.Player;
using _Game.Scripts.UI.Screens;
using UnityEngine;
using UnityEngine.Events;


// <summary>
// Главный класс игры
// Тут проходит основная иницилизация логики после перехода на главную сцену.
// Игровой цикл тоже тут
// </summary>
public class Main : MonoBehaviour
{
    [Header("NPC")]
    [SerializeField] private GameObject npcPathRoot;
    [SerializeField] private Transform npcSpawnPoint;
    [SerializeField] private GameObject[] npcs;
    
    [Header("References")]
    [SerializeField] private _Game.Scripts.UI.UIController uiController;
    
    // Ивенты
    [HideInInspector] public UnityEvent onNextNpc = new UnityEvent();
    [HideInInspector] public UnityEvent onNpcCome = new UnityEvent();
    
    
    void Start()
    {
        onNextNpc.AddListener(OnNextNpc);
        
        
        G.main = this;

        G.ui = uiController;
        
        // G.player = SpawnPlayer();

        G.currentNpc = SpawnNextNPC();
    }

    private void Update()
    {
        
        // TEST 
        if (Input.GetKeyDown(KeyCode.L))
        {
            uiController.ToggleScreen<HandbookScreen>();
        }
    }

    private PlayerController SpawnPlayer()
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        return GameObject.Instantiate(playerPrefab).GetComponent<PlayerController>();
    }

    private NpcController SpawnNextNPC()
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
    
    // Колбэки ивентов
    private void OnNextNpc()
    {
        if (G.currentNpc == null) // Удаляем NPC если уже был заспавнен
            Destroy(G.currentNpc.gameObject);
        
        G.currentNpc = SpawnNextNPC();
    }
    
    
}
