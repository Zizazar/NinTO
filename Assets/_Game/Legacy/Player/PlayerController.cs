using System.Collections;
using _Game.Legacy.CoffeeMaker2;
using _Game.Legacy.DialogueSystem;
using _Game.Legacy.DialogueSystem.data;
using _Game.Legacy.NPC;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DialogueController = _Game.Legacy.DialogueSystem.DialogueController;

namespace _Game.Legacy.Player
{
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
        public EndDialog endDialog;



        private SplineMovement _splineMovement;

        private InputAction _nextPosAction;
        private InputAction _prevPosAction;
        private InputAction _grabAction;
        private InputAction _HandbookAction;
        private InputAction _InteractAction;
        private InputAction _NextDialogAction;
        private InputAction _ExitAction;

        private int _clientsCount;
        [SerializeField] private NpcController _currentNpc;
        private AudioSource _audioSource;

        [Header("Sounds")]
        [SerializeField] private AudioClip moveSound;


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
            _ExitAction = InputSystem.actions.FindAction("Exit");

            NextClient();
            hints.showHint("����� � ��� �������� ������", 8, BindKey.E);
        }

        void Update()
        {
            HandleInput();
            UpdateScenario();
            UpdateCounter();
        }

        private void HandleInput()
        {
            if (_nextPosAction.IsPressed() && !dialogueController.dialogueInProcess) {_splineMovement.MoveToNextKnot(); } // �������� ���� �����������

            if (_prevPosAction.IsPressed() && !dialogueController.dialogueInProcess) _splineMovement.MoveToPreviousKnot(); // � ����

            if (_grabAction.IsPressed()) return;
        

            if (_HandbookAction.WasPressedThisFrame()) uiController.Toggle(UiType.Handbook);

            if (_InteractAction.WasPressedThisFrame()) Interact();

            if (_NextDialogAction.WasPressedThisFrame()) dialogueController.DialogueNext();

            if (_ExitAction.WasPressedThisFrame()) if (uiController.IsOpened(UiType.Handbook)) { uiController.Close(UiType.Handbook); } else { uiController.Toggle(UiType.EscMenu); uiController.Close(UiType.Settings); };
        }


        private void Interact() {
            if (_splineMovement.currentKnotIndex == 0 && !uiController.IsOpened(UiType.Handbook) && !uiController.IsOpened(UiType.Dialogue) && _currentNpc.readyForDialog()) 
            {

                switch (_currentNpc.Stage) // ��� ������� ��� ������ �� �������
                {
                    case 0:
                        uiController.Open(UiType.Dialogue);
                        dialogueController.StartDialogue(_currentNpc.getOrderDialogue(), _currentNpc);
                        break;
                    case 1:
                        if (CoffeeContoller.IsCoffeeDone()) {
                            CoffeeContoller.coffeeGiveTriger.Disintegrate();
                            timer.Pause();
                            uiController.Open(UiType.Dialogue);
                            dialogueController.coffeeQuality = ((timer.GetTimeSeconds() > 40) ? 1:2) ; // !! ����� ������� �������������
                            dialogueController.StartDialogue(_currentNpc.getDoneDialogue(), _currentNpc);
                        }
                        break;

                }
            }
        }

        void UpdateScenario() // ��� ������� ����� ��������� ��������
        {
            if (_currentNpc.stageChanged) {
                switch (_currentNpc.Stage)
                {
                    case 1:
                        timer.ResetTime();
                        timer.Begin();
                        CoffeeContoller.StartCoffeeMaking();
                        hints.showHint("������ �������� ����! \n <i>��������� � ����� � ����-�������", 10, BindKey.A);
                        _currentNpc.stageChanged = false; // ������� ��� ���������� 1 ���
                        break;
                    case 2:
                        hints.showHint("������ � ������� ���������� � �������.", 10, BindKey.J);
                        addNpcToHandboook();
                        _currentNpc.Leave();
                        _currentNpc.stageChanged = false;
                        StartCoroutine(WaitForNextNpc(8)); // ��������� ����� 5 ���
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

        private IEnumerator EndDialogSequence()
        {
            uiController.Open(UiType.Dialogue);
            //dialogueController.StartDialogue(endDialog);
            //yield return new WaitUntil(() => !dialogueController.dialogueInProcess);
            yield return null;
            uiController.Open(UiType.HandbookChoose);
        }

        private void NextClient()
        {
            if (handbookController.openedNpcsCount > 3)
            {
                StartCoroutine(EndDialogSequence());
            }
            else
            {
                _clientsCount++;

                GameObject npc = Instantiate(Npcs[_clientsCount - 1], npcPoint);
                _currentNpc = npc.GetComponent<NpcController>();
            }
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

        private IEnumerator WaitForNextNpc(float delay)
        {
            yield return new WaitForSeconds(delay);
            NextClient();
        }
    }
}
