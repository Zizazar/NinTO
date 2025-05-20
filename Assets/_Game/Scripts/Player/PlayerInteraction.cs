using System;
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
        
        
        private Rigidbody _grabbedObject;
        private float _currentGrabDistance;
        private InputAction _interactAction;
        private InputAction _scrollAction;
        private InputAction _keyboardScrollAction;
        
        private IGrabbable _currentGrabbable;
        
        private Ray cameraRay => G.camera.ScreenPointToRay(Input.mousePosition);
        
        
        private void OnEnable()
        {
            _interactAction = InputSystem.actions.FindAction("Interact");
            _scrollAction = InputSystem.actions.FindAction("GrabDistance");
            _keyboardScrollAction = InputSystem.actions.FindAction("GrabDistanceKb");
            
            // Подключаем обработчики событий
            _interactAction.started += OnInteractStarted;   // Нажатие
            _interactAction.canceled += OnInteractCanceled; // Отпускание
            
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

        private void HandleScrollInput()
        {
            if (!_grabbedObject) return;
            
            // Перемещение назад вперёд
            // input [-1; 1]
            float input = _scrollAction.ReadValue<Vector2>().y;
            float kbInput = _keyboardScrollAction.ReadValue<Vector2>().y;
            
            if (input == 0 && kbInput == 0) return;
            
            _currentGrabDistance = Mathf.Clamp(
                _currentGrabDistance + (scrollSensitivity * input * Time.deltaTime) + (keyboardSensitivity * kbInput * Time.deltaTime),
                minDistance,
                GetMaxDistance()
                
            );
        }

        private void FixedUpdate()
        {
            // В апдейте потому что ивент из InputSystem не может вызываться каждый кадр
            HandleScrollInput(); 
            if (_grabbedObject) 
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
                else if (hit.collider.TryGetComponent(out _currentGrabbable))
                {
                    _grabbedObject = hit.collider.gameObject.GetComponent<Rigidbody>();
                    
                    GrabObject();
                    _currentGrabbable.OnGrab();

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
            _currentGrabDistance = Mathf.Clamp(
                (cameraRay.origin - _grabbedObject.transform.position).magnitude, 
                minDistance, GetMaxDistance());
            

            if (_grabbedObject.TryGetComponent<FixedJoint>(out var joint))
                Destroy(joint);

            _grabbedObject.useGravity = false;
            
        }
        

        private void ReleaseObject()
        {
            if (_grabbedObject)
            {
                _grabbedObject.useGravity = true;
                _grabbedObject = null;
                _currentGrabbable.OnRelease();
                _currentGrabbable = null;
            }
        }
        
        private void MoveGrabbedObject()
        {
            Vector3 targetPoint = cameraRay.GetPoint(_currentGrabDistance);
            Vector3 forceDirection = targetPoint - _grabbedObject.position;

            // Ограничение ускорения
            Vector3 velocity = targetPoint - _grabbedObject.position;
            velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
            _grabbedObject.velocity = velocity;
            
            _grabbedObject.AddForce(forceDirection * (moveForce * Time.fixedDeltaTime));

            // Поворачиваем вверх
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            _grabbedObject.angularVelocity = Vector3.zero;
            
            _grabbedObject.MoveRotation(Quaternion.Slerp(
                _grabbedObject.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            ));
        }
        
        // ------ Grub gizmos ------
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_grabbedObject != null)
            {
                // Расстояние до захваченого объекта
                Gizmos.color = Color.red;
                Gizmos.DrawLine(
                    cameraRay.origin,
                    _grabbedObject.transform.position);
                    
                // Целевое расстояние
                Gizmos.color = Color.green;
                Gizmos.DrawLine(
                    cameraRay.origin,
                    cameraRay.GetPoint(_currentGrabDistance));
                
                // Максимальное расстояние
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(
                    cameraRay.origin, 
                    cameraRay.GetPoint(GetMaxDistance()));
                    
                // Текст с дистанцией
                Vector3 midPoint = (cameraRay.origin + _grabbedObject.transform.position) / 2;
                Handles.Label(midPoint, 
                    "Distance: " + Vector3.Distance(cameraRay.origin, _grabbedObject.transform.position).ToString("F2"));
            }
        }
        #endif
    }
}