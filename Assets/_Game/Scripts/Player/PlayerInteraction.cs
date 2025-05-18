using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction")]
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private LayerMask interactableLayer;
        
        [Header("Grab")]
        [SerializeField] private float maxVelocity = 15f;
        [SerializeField] private float moveForce = 500f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float maxDistance = 10f;
        [SerializeField] private float minDistance = 0f;
        [Space]
        [SerializeField] private LayerMask groundLayer;
        [Space]
        [SerializeField] private float scrollSensitivity = 0.1f;
        [SerializeField] private float keyboardSensitivity = 0.1f;
        
        
        private Rigidbody grabbedObject;
        private float currentGrabDistance;
        private InputAction _interactAction;
        private InputAction _scrollAction;
        
        private Ray cameraRay => G.camera.ScreenPointToRay(Input.mousePosition);
        
        
        private void OnEnable()
        {
            _interactAction = InputSystem.actions.FindAction("Interact");
            _scrollAction = InputSystem.actions.FindAction("GrabDistance");
            
            // Подключаем обработчики событий
            _interactAction.started += OnInteractStarted;   // Нажатие
            _interactAction.canceled += OnInteractCanceled; // Отпускание
            _scrollAction.performed += OnScrollInput;
            
            _scrollAction.Enable();
            _interactAction.Enable();
        }
        private void OnDisable()
        {
            // Отключаем обработчики
            _interactAction.started -= OnInteractStarted;
            _interactAction.canceled -= OnInteractCanceled;
            
            _scrollAction.Disable();
            _interactAction.Disable();
        }
        
        private void OnInteractStarted(InputAction.CallbackContext context)
        {
            TryInteract(); // Вызывается при нажатии
        }
        
        private void OnInteractCanceled(InputAction.CallbackContext context)
        {
            ReleaseObject(); // Вызывается при отпускании
        }

        private void OnScrollInput(InputAction.CallbackContext context)
        {
            // Перемещение назад вперёд
            float input = context.ReadValue<Vector2>().y;
            currentGrabDistance = Mathf.Clamp(
                currentGrabDistance + (scrollSensitivity * input),
                minDistance,
                GetMaxDistance()
            );
        }

        private void FixedUpdate()
        {
            
            if (grabbedObject != null) 
                MoveGrabbedObject();
            
        }
        
        // ----- Interact -------
        public void TryInteract()
        {
            if (Physics.Raycast(cameraRay, out RaycastHit hit, interactionDistance, interactableLayer))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact();
                }
                else if (hit.collider.TryGetComponent(out BaseGrabbable grabbable))
                {
                    grabbedObject = hit.collider.gameObject.GetComponent<Rigidbody>();
                    GrabObject();
                    grabbable.OnGrab();

                }
            }
        }
        
        // ---------- Grub -----------
        private float GetMaxDistance()
        {
            return Physics.Raycast(cameraRay, 
                out RaycastHit hit, 
                maxDistance, 
                groundLayer.value) 
                ? hit.distance : maxDistance;
        }
        
        void GrabObject()
        {
            currentGrabDistance = Mathf.Clamp(
                (cameraRay.origin - grabbedObject.transform.position).magnitude, 
                minDistance, GetMaxDistance());
            

            if (grabbedObject.TryGetComponent<FixedJoint>(out var joint))
                Destroy(joint);

            grabbedObject.useGravity = false;
            
        }
        
        public void ReleaseObject()
        {
            if (grabbedObject != null)
            {
                grabbedObject.useGravity = true;
                grabbedObject = null;
            }
        }
        
        void MoveGrabbedObject()
        {
            Vector3 targetPoint = cameraRay.GetPoint(currentGrabDistance);
            Vector3 forceDirection = targetPoint - grabbedObject.position;

            // Ограничение ускорения
            Vector3 velocity = targetPoint - grabbedObject.position;
            velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
            grabbedObject.velocity = velocity;
            
            grabbedObject.AddForce(forceDirection * (moveForce * Time.fixedDeltaTime));

            // Поворачиваем вверх
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            grabbedObject.angularVelocity = Vector3.zero;
            
            grabbedObject.MoveRotation(Quaternion.Slerp(
                grabbedObject.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            ));
        }
        
        // ------ Grub gizmos ------
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (grabbedObject != null)
            {
                // Расстояние до захваченого объекта
                Gizmos.color = Color.red;
                Gizmos.DrawLine(
                    cameraRay.origin,
                    grabbedObject.transform.position);
                    
                // Целевое расстояние
                Gizmos.color = Color.green;
                Gizmos.DrawLine(
                    cameraRay.origin,
                    cameraRay.GetPoint(currentGrabDistance));
                
                // Максимальное расстояние
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(
                    cameraRay.origin, 
                    cameraRay.GetPoint(GetMaxDistance()));
                    
                // Текст с дистанцией
                Vector3 midPoint = (cameraRay.origin + grabbedObject.transform.position) / 2;
                Handles.Label(midPoint, 
                    "Distance: " + Vector3.Distance(cameraRay.origin, grabbedObject.transform.position).ToString("F2"));
            }
        }
        #endif
    }
}