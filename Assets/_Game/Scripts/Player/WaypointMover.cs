using UnityEngine;
using DG.Tweening;

public class WaypointMover : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveDuration = 1f;
    public float rotationDuration = 0.5f; // Длительность поворота
    public Ease moveEase = Ease.InOutQuad;

    private int _currentIndex = 0;
    private Sequence _moveTween;

    private void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints not assigned!");
            enabled = false;
            return;
        }
        
        transform.position = waypoints[0].position;
        transform.LookAt(waypoints[1].position); // Начальная ориентация
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveNext();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MovePrevious();
        }
    }

    private void MoveNext()
    {
        _currentIndex = (_currentIndex + 1) % waypoints.Length;
        MoveToWaypoint(waypoints[_currentIndex]);
    }

    private void MovePrevious()
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