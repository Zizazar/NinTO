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
    [Header("References")]
    [SerializeField] private _Game.Scripts.UI.UIController uiController;
    [SerializeField] private NpcSpawner npcSpawner;
    [SerializeField] private GameObject playerPrefab;
    
    // Ивенты
    [HideInInspector] public UnityEvent onNpcCome;
    [HideInInspector] public UnityEvent onPause;
    [HideInInspector] public UnityEvent onResume;
    
    private InputAction _pauseAction;
    
    void Awake()
    {
        
        _pauseAction = InputSystem.actions.FindAction("Pause");
        _pauseAction.performed += TogglePause;
        
        onPause.AddListener(OnPause);
        onResume.AddListener(OnResume);
        
        G.main = this;

        G.ui = uiController;
        
        G.player = SpawnPlayer();

        G.currentNpc = npcSpawner.SpawnNext();

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
        G.currentNpc.enabled = false;
    }

    private void OnResume()
    {
        G.ui.HideScreen<PauseScreen>();
        G.player.enabled = true;
        G.currentNpc.enabled = true;
    }

    private PlayerController SpawnPlayer()
    {
        var player = FindObjectOfType<PlayerController>();
        return player ? player : Instantiate(playerPrefab).GetComponent<PlayerController>();
    }
    /*
    private void OnApplicationFocus(bool hasFocus)
    {
        // Авто пауза
        if (!hasFocus)
        {
            onPause.Invoke();
        }
    }
    */
}
