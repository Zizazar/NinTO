using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class WaypointMover : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveDuration = 1f;
    public float rotationDuration = 0.5f; // Длительность поворота
    public Ease moveEase = Ease.InOutQuad;

    private int _currentIndex = 0;
    private Sequence _moveTween;
    private InputAction _moveNextAction;
    private InputAction _movePrevAction;

    private void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints not assigned!");
            enabled = false;
            return;
        }
        
        transform.position = waypoints[0].position;
        transform.rotation = waypoints[0].rotation; // Начальная ориентация
    }
    
    private void OnEnable()
    {
        _moveNextAction = InputSystem.actions.FindAction("NextPosition");
        _movePrevAction = InputSystem.actions.FindAction("PreviousPosition");
        
        _moveNextAction.performed += OnMoveNext;
        _movePrevAction.performed += OnMovePrevious;
        
        _moveNextAction.Enable();
        _movePrevAction.Enable();
    }
    private void OnDisable()
    {
        _moveNextAction.performed -= OnMoveNext;
        _movePrevAction.performed -= OnMovePrevious;
        
        _moveNextAction.Disable();
        _movePrevAction.Disable();
    }

    private void OnMoveNext(InputAction.CallbackContext ctx)
    {
        _currentIndex = (_currentIndex + 1) % waypoints.Length;
        MoveToWaypoint(waypoints[_currentIndex]);
    }

    private void OnMovePrevious(InputAction.CallbackContext ctx)
    {
        _currentIndex--;
        if (_currentIndex < 0) _currentIndex = waypoints.Length - 1;
        MoveToWaypoint(waypoints[_currentIndex]);
    }

    public void MoveToWaypoint(Transform target)
    {
        if (_moveTween != null && _moveTween.IsActive())
        {
            _moveTween.Kill();
        }

        
        Sequence moveSequence = DOTween.Sequence();
        
        // Анимация поворота
        moveSequence.Append(transform.DORotate(
            target.rotation.eulerAngles, 
            rotationDuration
        ).SetEase(moveEase));

        // Анимация перемещения
        moveSequence.Join(transform.DOMove(
            target.position, 
            moveDuration
        ).SetEase(moveEase));

        _moveTween = moveSequence;
    }
}