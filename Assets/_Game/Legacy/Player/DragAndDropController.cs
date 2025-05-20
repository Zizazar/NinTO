using System.Linq;
using _Game.Legacy.CoffeeMaker2;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts.Player
{
    public class DragAndDropController : MonoBehaviour
    {

        [Header("Settings")]
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
        private InputAction _GrabDistanceActionKeyboard;

        private void Start()
        {
            _GrabDistanceAction = InputSystem.actions.FindAction("GrabDistance");
            _GrabDistanceActionKeyboard = InputSystem.actions.FindAction("GrabDistanceKeyboard");
        }

        void Update()
        {
            HandleMouseScroll();
        }

        void FixedUpdate()
        {
            if (grabbedObject)
            {
                MoveGrabbedObject();
            }
        }

        public void GrabObject(RaycastHit hit)
        {
                grabbedObject = hit.collider.GetComponent<Rigidbody>();
                if (grabbedObject)
                {
                    currentGrabDistance = Vector3.Distance(transform.position, hit.point) / 2;
                    currentGrabDistance = Mathf.Clamp(currentGrabDistance, minDistance, maxDistance);

                    if (grabbedObject.TryGetComponent<FixedJoint>(out var joint))
                    {
                        Destroy(joint);
                    }

                    grabbedObject.useGravity = false;
                }
        }

        void MoveGrabbedObject()
        {
            Ray ray = G.camera.ScreenPointToRay(Input.mousePosition);
            Vector3 targetPoint = ray.GetPoint(currentGrabDistance);
            Vector3 forceDirection = targetPoint - grabbedObject.position;

            Vector3 velocity = (targetPoint - grabbedObject.position) * 10f;
            velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
            grabbedObject.velocity = velocity;

            if (grabbedObject.velocity.magnitude > maxVelocity)
            {
                grabbedObject.velocity = grabbedObject.velocity.normalized * maxVelocity;
            }
            grabbedObject.AddForce(forceDirection * (moveForce * Time.fixedDeltaTime));

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
                float input = _GrabDistanceActionKeyboard.ReadValue<Vector2>().y;
                currentGrabDistance = Mathf.Clamp(
                    currentGrabDistance + (scroll * scrollSensitivity + input * _keyboardSensitivity * Time.fixedDeltaTime),
                    minDistance,
                    maxDistance
                );
            }
        }

        public void ReleaseObject()
        {
            if (activeSnapPoint)
            {
                grabbedObject.transform.position = activeSnapPoint.transform.position;
                grabbedObject.transform.rotation = activeSnapPoint.transform.rotation;

                grabbedObject.velocity = Vector3.zero;
                grabbedObject.angularVelocity = Vector3.zero;

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
}