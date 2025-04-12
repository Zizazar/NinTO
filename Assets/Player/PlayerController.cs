using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public UIController uiController;
    public DialogueController dialogueController;
    public HandbookController handbookController;
    public HintsController hints;
    public TimerController timer;
    public CoffeeContoller CoffeeContoller;

    public string playerName;
    public GameObject[] Npcs;
    public TMP_Text clientCounter;
    public Transform npcPoint;



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
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private SceneAsset endScene;


    void Start()
    {
        _splineMovement = GetComponent<SplineMovement>();
        _audioSource = GetComponent<AudioSource>();

        _nextPosAction = InputSystem.actions.FindAction("NextPosition");
        _prevPosAction = InputSystem.actions.FindAction("PreviousPosition");
        _grabAction = InputSystem.actions.FindAction("Grab");
        _HandbookAction = InputSystem.actions.FindAction("Handbook");
        _InteractAction = InputSystem.actions.FindAction("Interact");
        _NextDialogAction = InputSystem.actions.FindAction("NextDialogue");

        NextClient();
        hints.showHint("Когда к вам подайдёт клиент", 8, BindKey.E);
    }

    void Update()
    {
        HandleInput();
        UpdateScenario();
        UpdateCounter();
    }

    private void HandleInput()
    {
        if (_nextPosAction.IsPressed()) {_splineMovement.MoveToNextKnot(); } // Добавить звук перемещения

        if (_prevPosAction.IsPressed()) _splineMovement.MoveToPreviousKnot(); // И сюда

        if (_grabAction.IsPressed()) return;
        

        if (_HandbookAction.WasPressedThisFrame()) uiController.toggleUI(UiType.Handbook);

        if (_InteractAction.WasPressedThisFrame()) Interact();

        if (_NextDialogAction.WasPressedThisFrame()) dialogueController.DialogueNext();
    }


    private void Interact() {
        if (_splineMovement.currentKnotIndex == 0 && !uiController.IsOppened(UiType.Handbook) && !uiController.IsOppened(UiType.Dialogue) && _currentNpc.readyForDialog()) 
        {

            switch (_currentNpc.Stage) // Это тригеры при нажтии по клиенту
            {
                case 0:
                    uiController.openUI(UiType.Dialogue);
                    dialogueController.StartDialogue(_currentNpc.getOrderDialogue(), _currentNpc);
                    break;
                case 1:
                    Debug.Log("test");
                    if (CoffeeContoller.IsCoffeeDone()) {
                        timer.Pause();
                        uiController.openUI(UiType.Dialogue);
                        dialogueController.StartDialogue(_currentNpc.getDoneDialogue(), _currentNpc);
                    }
                    break;

            }
        }
    }

    void UpdateScenario() // Это тригеры после окончания диалогов
    {
        if (_currentNpc.stageChanged) {
            switch (_currentNpc.Stage)
            {
                case 1:
                    timer.Begin();
                    CoffeeContoller.StartCoffeeMaking();
                    hints.showHint("Теперь готовьте кофе! \n <i>Перейдите к столу с кофе-машиной", 10, BindKey.A);
                    _currentNpc.stageChanged = false; // Костыль для выполнения 1 раз
                    break;
                case 2:
                    hints.showHint("Запись о клиенте добавленна в блакнот.", 10, BindKey.J);
                    addNpcToHandboook();
                    _currentNpc.Leave();
                    _currentNpc.stageChanged = false;
                    break;
            }
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
        
        GameObject npc = Instantiate(Npcs[_clientsCount - 1], npcPoint);
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

    public void EndGame()
    {
        SceneManager.LoadScene("End");
    }
}
