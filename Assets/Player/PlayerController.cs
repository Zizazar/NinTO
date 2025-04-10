using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public UIController uiController;
    public DialogueController dialogueController;
    public HandbookController handbookController;
    public TimerController timer;

    public string playerName;
    public GameObject[] Npcs;
    public TMP_Text clientCounter;
    


    private SplineMovement _splineMovement;

    private InputAction _nextPosAction;
    private InputAction _prevPosAction;
    private InputAction _grabAction;
    private InputAction _HandbookAction;
    private InputAction _InteractAction;
    private InputAction _NextDialogAction;

    private int _clientsCount;
    private NpcController _currentNpc;
    private AudioSource _audioSource;

    [Header("Sounds")]
    [SerializeField] private AudioClip audioClip;


    void Start()
    {
        _splineMovement = GetComponent<SplineMovement>();
        _audioSource = GetComponent<AudioSource>();

        _nextPosAction = InputSystem.actions.FindAction("NextPosition");
        _prevPosAction = InputSystem.actions.FindAction("PreviousPosition");
        _grabAction = InputSystem.actions.FindAction("PreviousPosition");
        _HandbookAction = InputSystem.actions.FindAction("Handbook");
        _InteractAction = InputSystem.actions.FindAction("Interact");
        _NextDialogAction = InputSystem.actions.FindAction("NextDialogue");

        NextClient();
    }

    void Update()
    {
        HandleInput();
        UpdateScenario();
    }

    private void HandleInput()
    {
        if (_nextPosAction.IsPressed()) {_splineMovement.MoveToNextKnot();}

        if (_prevPosAction.IsPressed()) _splineMovement.MoveToPreviousKnot();

        if (_grabAction.IsPressed()) return;

        if (_HandbookAction.WasPressedThisFrame()) uiController.toggleUI(UiType.Handbook);

        if (_InteractAction.WasPressedThisFrame()) Interact();

        if (_NextDialogAction.WasPressedThisFrame()) dialogueController.DialogueNext();
    }


    private void Interact() {
        if (_splineMovement.currentKnotIndex == 1 && !uiController.IsOppened(UiType.Handbook) && !uiController.IsOppened(UiType.Dialogue)) // && _currentNpc.readyForDialog()
        {

            switch (_currentNpc.Stage)
            {
                case 0:
                    uiController.openUI(UiType.Dialogue);
                    dialogueController.StartDialogue(_currentNpc.getOrderDialogue(), _currentNpc);
                    break;
                case 1:  
                    GiveCoffee();
                    break;
                case 2:
                    timer.Pause();
                    uiController.openUI(UiType.Dialogue);
                    dialogueController.StartDialogue(_currentNpc.getDoneDialogue(), _currentNpc);
                    break;
                case 3:
                    addNpcToHandboook();
                    break;
            }
        }
    }

    void UpdateScenario()
    {
        switch (_currentNpc.Stage)
        {
            case 1:
                timer.Begin();
                _currentNpc.stageChanged = false;
                break;

        }
    }

    public void GiveCoffee()
    {
        _currentNpc.Stage++;
        _currentNpc.stageChanged = false;

    }

    private void addNpcToHandboook() {
        handbookController.openedNpcsCount++;
        handbookController.npcPhrases.Add(_currentNpc.saidPhrases);
    }

    private void NextClient()
    {
        _clientsCount++;
        
        GameObject npc = Instantiate(Npcs[_clientsCount - 1], Vector3.zero, new Quaternion(0, 0, 0, 0));
        _currentNpc = npc.GetComponent<NpcController>();
    }

    public void PlayPlayerSound(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    private void UpdateCounter()
    {
        clientCounter.text = string.Format("{0} / 4", _clientsCount);
    }
}
