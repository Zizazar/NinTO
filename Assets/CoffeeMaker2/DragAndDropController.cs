using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectDragController : MonoBehaviour
{
    private GameObject _grabbedObject;
    private Rigidbody _grabbedRb;
    private float _currentGrabDistance;
    private Vector3 _grabOffset;

    public CoffeeContoller coffeeContoller;

    [Header("Settings")]
    [SerializeField] private float _scrollSensitivity = 2f;
    [SerializeField] private float _minDistance = 1f;
    [SerializeField] private float _maxDistance = 10f;
    [SerializeField] private string[] _tags;


    void Update()
    {
        HandleMouseInput();
        HandleScrollWheel();
        MoveGrabbedObject();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryGrabObject();
        }


        if (Input.GetMouseButtonUp(0) && _grabbedObject != null)
        {
            ReleaseObject();
        }
    }

    public void TryGrabObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (_tags.Contains(hit.collider.tag))
            {
                _grabbedObject = hit.collider.gameObject;
                _currentGrabDistance = hit.distance;
                _grabOffset = _grabbedObject.transform.position - hit.point;

                _grabbedRb = _grabbedObject.GetComponent<Rigidbody>();
                _grabbedRb.useGravity = false;
                _grabbedRb.freezeRotation = true;
                _grabbedRb.isKinematic = false;
                _grabbedRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }
            if (hit.collider.CompareTag("Button"))
            {
                coffeeContoller.OnButtonClick();
            }
        }
    }


    public void ReleaseObject()
    {
        _grabbedRb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        //_grabbedRb.isKinematic = false;
        _grabbedRb.useGravity = true;
        _grabbedRb.freezeRotation = false;

        _grabbedRb = null;
        _grabbedObject = null;
    }

    private void HandleScrollWheel()
    {
        if (_grabbedObject != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            _currentGrabDistance = Mathf.Clamp(
                _currentGrabDistance - scroll * _scrollSensitivity,
                _minDistance,
                _maxDistance
            );
        }
    }

    private void MoveGrabbedObject()
    {
        if (_grabbedObject == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPosition = ray.GetPoint(_currentGrabDistance) + _grabOffset;


        _grabbedRb.MovePosition(Vector3.Lerp(
            _grabbedRb.position,
            targetPosition,
            Time.fixedDeltaTime * 1f));
    }
}