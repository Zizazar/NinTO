using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public GameObject UI;


    private SplineMovement _splineMovement;

    private InputAction _nextPosAction;
    private InputAction _prevPosAction;
    private InputAction _grabAction;
    private InputAction _HandbookAction;

    private bool _handbookOpened;


    void Start()
    {
        _splineMovement = GetComponent<SplineMovement>();

        _nextPosAction = InputSystem.actions.FindAction("NextPosition");
        _prevPosAction = InputSystem.actions.FindAction("PreviousPosition");
        _grabAction = InputSystem.actions.FindAction("PreviousPosition");
        _HandbookAction = InputSystem.actions.FindAction("Handbook");
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (_nextPosAction.IsPressed()) _splineMovement.MoveToNextKnot();

        if (_prevPosAction.IsPressed()) _splineMovement.MoveToPreviousKnot();

        if (_grabAction.IsPressed()) ;

        if (_HandbookAction.IsPressed()) OpenHandbook();
    }

    public void OpenHandbook()
    {
        _handbookOpened = !_handbookOpened;
        UI.SetActive(!_handbookOpened);
    }

}
