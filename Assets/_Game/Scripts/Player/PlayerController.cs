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

        private InputAction _startDialogueAction;
        private InputAction _openHandbookAction;
        
        private DialogueGraph _dialogueGraph;
        
        private WaypointMover _waypointMover;
        private ParallaxCamera _parallaxCamera;
        private PlayerInteraction _playerInteraction;
        
        
        private void Awake()
        {
            _startDialogueAction = InputSystem.actions.FindAction("Talk");
            _openHandbookAction = InputSystem.actions.FindAction("OpenHandbook");
            
            _waypointMover = GetComponent<WaypointMover>();
            _parallaxCamera = G.camera.GetComponent<ParallaxCamera>();
            _playerInteraction = GetComponent<PlayerInteraction>();
        }

        private void OnEnable()
        {
            _openHandbookAction.performed += OpenHandbook;
         
            // Востанавливаем работу зависимостей
            _startDialogueAction.Enable();
            _openHandbookAction.Enable();
            
            _waypointMover.enabled = true;
            _parallaxCamera.enabled = true;
            _playerInteraction.enabled = true;
        }

        private void OnDisable()
        {
            _openHandbookAction.performed -= OpenHandbook;
            
            
            // Выключаем зависимости если выключен игрок
            _startDialogueAction.Disable();
            _openHandbookAction.Disable();
            
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _dialogueGraph?.NextPhrase();
            }
        }
    }
}