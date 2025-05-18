using System;
using _Game.Legacy.DialogueSystem;
using _Game.Scripts.NPC;
using _Game.Scripts.Player;
using _Game.Scripts.UI.Screens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


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
    [SerializeField] private GameObject playerPrefab;
    
    // Ивенты
    [HideInInspector] public UnityEvent onNextNpc = new UnityEvent();
    [HideInInspector] public UnityEvent onNpcCome = new UnityEvent();
    [HideInInspector] public UnityEvent onPause = new UnityEvent();
    [HideInInspector] public UnityEvent onResume = new UnityEvent();
    
    private InputAction _pauseAction;
    
    void Awake()
    {
        onNextNpc.AddListener(OnNextNpc);
        
        _pauseAction = InputSystem.actions.FindAction("Pause");
        _pauseAction.performed += TogglePause;
        
        onPause.AddListener(OnPause);
        onResume.AddListener(OnResume);
        
        G.main = this;

        G.ui = uiController;
        
         G.player = SpawnPlayer();

        G.currentNpc = SpawnNextNPC();
    }

    private void TogglePause(InputAction.CallbackContext ctx) 
    {
        if (G.paused)
        {
            onResume?.Invoke();
            G.paused = false;
        }
        else
        {
            onPause?.Invoke();
            G.paused = true;
        }
    }

    private void OnPause()
    {
        G.ui.ShowScreen<PauseScreen>();
        G.player.enabled = false;
        G.currentNpc.enabled = false;
    }

    private void OnResume()
    {
        G.ui.HideScreen<PauseScreen>();
        G.player.enabled = true;
        G.currentNpc.enabled = true;
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
        var player = FindObjectOfType<PlayerController>();
        return player ? player : Instantiate(playerPrefab).GetComponent<PlayerController>();
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
