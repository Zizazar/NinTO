using System;
using _Game.Legacy.DialogueSystem;
using _Game.Scripts.NPC;
using _Game.Scripts.Player;
using _Game.Scripts.UI.Screens;
using GoT._Game.Scripts.Utils;
using UnityEditor.U2D.Animation;
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
    [Header("References")]
    [SerializeField] private GameObject playerPrefab;
    
    // Ивенты
    [HideInInspector] public UnityEvent onNpcCome;
    [HideInInspector] public UnityEvent onPause;
    [HideInInspector] public UnityEvent onResume;
    
    
    void Start()
    {

        G.input = new GameInput();
        
        G.input.Main.Pause.performed += TogglePause;
        G.input.Main.Enable();
        
        onPause.AddListener(OnPause);
        onResume.AddListener(OnResume);

        G.currentNpcIndex = -1;
        G.openedNpcsCount = 0;
        G.paused = false;
        
        G.main = this;
        
        G.player = SpawnPlayer();
        G.player.Init();
        
        G.npcSpawner.Init();

        G.npcSpawner.SpawnNext();

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

    // Выключаем контоллеры и все классы которые не должны обновлятся на паузе
    private void OnPause()
    {
        G.ui.ShowScreen<PauseScreen>();
        G.player.enabled = false;
        G.npcSpawner.current.enabled = false;
    }

    private void OnResume()
    {
        G.ui.HideScreen<PauseScreen>();
        G.player.enabled = true;
        G.npcSpawner.current.enabled = true;
    }

    private PlayerController SpawnPlayer()
    {
        var player = FindObjectOfType<PlayerController>();
        return !player ? Instantiate(playerPrefab).GetComponent<PlayerController>() : player;
    }
    
    #if !UNITY_EDITOR
    private void OnApplicationFocus(bool hasFocus)
    {
        // Авто пауза (только в билде)
        if (!hasFocus)
        {
            onPause.Invoke();
        }
    }
    #endif
    
}
