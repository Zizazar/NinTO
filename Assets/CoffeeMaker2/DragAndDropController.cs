using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class GrabController : MonoBehaviour
{
    public CoffeeContoller coffeeContoller;
    public AudioSource audioSource;
    public AudioClip connect;
    public AudioClip grabClip;

    [Header("Settings")]
    public string[] grabableTags;
    public float scrollSensitivity = 2f;
    public float _keyboardSensitivity = 3f;
    public float minDistance = 1f;
    public float maxDistance = 10f;
    public float moveForce = 500f;
    public float maxVelocity = 15f;
    public float rotationSpeed = 10f;

    public Rigidbody grabbedObject;
    private float currentGrabDistance;
    private SnapPoint activeSnapPoint;
    private FixedJoint snapJoint;


    private InputAction _GrabDistanceAction;

    private void Start()
    {
        _GrabDistanceAction = InputSystem.actions.FindAction("GrabDistance");
    }

    void Update()
    {
        HandleMouseInput();
        HandleMouseScroll();
    }

    void FixedUpdate()
    {
        if (grabbedObject)
        {
            MoveGrabbedObject();
        }
    }

    void HandleMouseInput()
    {
            if (Input.GetMouseButtonDown(0) && grabbedObject == null)
            {
                TryGrabObject();
                audioSource.clip = grabClip;
                audioSource.pitch = 1f;
                audioSource.Play();
            }
            if (Input.GetMouseButtonUp(0) && grabbedObject != null)
            {
                ReleaseObject();
                audioSource.clip = grabClip;
                audioSource.pitch = 0.8f;
                audioSource.Play();
        }

    }

    void TryGrabObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (grabableTags.Contains(hit.collider.tag))
            {
                grabbedObject = hit.collider.GetComponent<Rigidbody>();
                if (grabbedObject)
                {
                    currentGrabDistance = Vector3.Distance(transform.position, hit.point)/2; // костыль какой то
                    currentGrabDistance = Mathf.Clamp(currentGrabDistance, minDistance, maxDistance);

                    // Проверяем, был ли объект присоединен
                    if (grabbedObject.TryGetComponent<FixedJoint>(out var joint))
                    {
                        Destroy(joint);
                    }

                    grabbedObject.useGravity = false;
                }
            }
            if (hit.collider.CompareTag("Button"))
            {
                coffeeContoller.OnButtonClick();
            }
            if (hit.collider.CompareTag("Radio"))
            {
                hit.collider.GetComponent<RadioController>().TogglePlay();
            }
        }
    }

    void MoveGrabbedObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.GetPoint(currentGrabDistance);
        Vector3 forceDirection = targetPoint - grabbedObject.position;

        Vector3 velocity = (targetPoint - grabbedObject.position) * 10f;
        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
        grabbedObject.velocity = velocity;

        // Ограничиваем максимальную скорость
        if (grabbedObject.velocity.magnitude > maxVelocity)
        {
            grabbedObject.velocity = grabbedObject.velocity.normalized * maxVelocity;
        }
        grabbedObject.AddForce(forceDirection * moveForce * Time.fixedDeltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, transform.up);
        grabbedObject.angularVelocity = Vector3.zero;
        grabbedObject.MoveRotation(Quaternion.Slerp(
            grabbedObject.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        ));
    }

    void HandleMouseScroll()
    {
        if (grabbedObject)
        {
            float scroll = _GrabDistanceAction.ReadValue<Vector2>().y;
            currentGrabDistance = Mathf.Clamp(
                currentGrabDistance - scroll * scrollSensitivity,
                minDistance,
                maxDistance
            );
        }
    }

    void ReleaseObject()
    {
        if (activeSnapPoint)
        {
            audioSource.clip = connect;
            audioSource.Play();

            grabbedObject.transform.position = activeSnapPoint.transform.position;
            grabbedObject.transform.rotation = activeSnapPoint.transform.rotation;

            // Сбрасываем скорости
            grabbedObject.velocity = Vector3.zero;
            grabbedObject.angularVelocity = Vector3.zero;

            // Создаем соединение
            snapJoint = grabbedObject.gameObject.AddComponent<FixedJoint>();
            snapJoint.connectedBody = activeSnapPoint.rigidbody;
            activeSnapPoint = null;
        }

        grabbedObject.useGravity = true;
        grabbedObject = null;
    }

    public void SetActiveSnapPoint(SnapPoint point)
    {
        activeSnapPoint = point;
    }

    public void ClearActiveSnapPoint(SnapPoint point)
    {
        if (activeSnapPoint == point)
        {
            activeSnapPoint = null;
        }
    }
}