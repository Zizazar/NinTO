using _Game.Legacy.DialogueSystem.data;
using _Game.Legacy.Player;
using UnityEngine;

namespace _Game.Legacy.NPC
{
    public class NpcController : MonoBehaviour
    {
        public NpcData npcData;

        public bool walk = false;

        public string saidPhrases;
        public int Stage = 0; // 1 - ����� ������ ����, 2 - ����� ������ ����
        public bool stageChanged = false;

        private Animator _animator;
        public SplineMovement _splineMovement;
        public bool IsLeaving = false;

        void Start()
        {
            _splineMovement = GetComponentInChildren<SplineMovement>();
            _animator = GetComponent<Animator>();
            _splineMovement.StartAutomaticMovement(true);
        }


        void Update()
        {
            _animator.SetBool("Walking", _splineMovement.isMoving);
            if (IsLeaving && _splineMovement.IsStoppedOnStart())
                Destroy(gameObject); // ����������)
        }

        public bool readyForDialog()
        {
            return _splineMovement.IsStoppedInEnd(); // ��������� ����� �� ��� �� ������
        }

        public OrderDialog getOrderDialogue()
        {
            return npcData.orderDialog;
        }
        public DoneDialog getDoneDialogue()
        {
            return npcData.doneDialog;
        }
        public void Leave()
        {
        
            _splineMovement.StartAutomaticMovement(false);
            IsLeaving = true;
        }

    }
}
