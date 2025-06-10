using System;
using System.Linq;
using _Game.Legacy.Player;
using _Game.Scripts.UI.Screens;
using DG.Tweening;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts.Player
{
    [RequireComponent(typeof(WaypointMover))]
    public class PlayerController : MonoBehaviour
    {

        private bool _initialized;

        private DialogueGraph _dialogueGraph;

        private WaypointMover _waypointMover;
        private ParallaxCamera _parallaxCamera;
        private PlayerInteraction _playerInteraction;


        public void Init()
        {
            G.input.Player.Enable();
            
            _waypointMover = GetComponent<WaypointMover>();
            _parallaxCamera = G.camera.GetComponent<ParallaxCamera>();
            _playerInteraction = GetComponent<PlayerInteraction>();
            
            G.input.Player.OpenHandbook.performed += OpenHandbook;
            
            _initialized = true;
        }

        private void OnEnable()
        {
            if (!_initialized) return;
            
            G.input.Player.Enable();

            _waypointMover.enabled = true;
            _parallaxCamera.enabled = true;
            _playerInteraction.enabled = true;
        }

        private void OnDisable()
        {
            if (!_initialized) return;
            // Выключаем зависимости если выключен игрок
            G.input.Player.Disable();

            _waypointMover.enabled = false;
            _parallaxCamera.enabled = false;
            _playerInteraction.enabled = false;
        }

        private void OpenHandbook(InputAction.CallbackContext ctx)
        {
            G.ui.ToggleScreen<HandbookScreen>();
        }

    private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                _dialogueGraph = Resources.Load<DialogueGraph>("Dialogues/TestDialogue");
                _dialogueGraph.Start();
            }
        }
    }
}