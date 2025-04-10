using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public NpcData npcData;

    public bool walk = false;

    public string saidPhrases;
    public int Stage = 0; // 1 - после заказа кофе, 2 - после выдачи кофе
    public bool stageChanged = false;

    private Animator _animator;
    private SplineMovement _splineMovement;

    void Start()
    {
        _splineMovement = GetComponent<SplineMovement>();
        _animator = GetComponent<Animator>();
        _splineMovement.StartAutomaticMovement(true);
    }


    void Update()
    {
        _animator.SetBool("Walking", _splineMovement.isMoving);
    }

    public bool readyForDialog()
    {
        return _splineMovement.IsStoppedInEnd(); // Проверяем дошёл ли нпс до стойки
    }

    public OrderDialog getOrderDialogue()
    {
        return npcData.orderDialog;
    }
    public DoneDialog getDoneDialogue()
    {
        return npcData.doneDialog;
    }

}
