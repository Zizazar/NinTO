using System;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Interaction")]
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private LayerMask interactableLayer;
        [Tag]
        [SerializeField] private string[] grabableTags;
        
        [Header("Grab")]
        [SerializeField] private float maxVelocity = 15f;
        [SerializeField] private float moveForce = 500f;
        [SerializeField] private float rotationSpeed = 10f;
        [Space]
        [SerializeField] private float scrollSensitivity = 0.1f;
        [SerializeField] private float keyboardSensitivity = 0.1f;
        
        
        private Rigidbody grabbedObject;
        private float currentGrabDistance;
        
        private InputAction _A_GrabDistance;
        private InputAction _A_GrabDistanceKB;
        
        private DialogueGraph _dialogueGraph;
        
        private Ray cameraRay => G.camera.ScreenPointToRay(Input.mousePosition);
        
        
        private void Start()
        {
            _A_GrabDistance = InputSystem.actions.FindAction("GrabDistance");
            _A_GrabDistanceKB = InputSystem.actions.FindAction("GrabDistanceKeyboard");
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryInteract();
                
            }
            else if (Input.GetMouseButtonUp(0) && grabbedObject != null)
            {
                ReleaseObject();
                AudioManager.Instance.PlaySoundRandomPitch("grab", 0.5f, 0.8f);
            }
            
            if (grabbedObject != null) MoveGrabbedObject();
            ZoomLookHandler();
            if (Input.GetKeyDown(KeyCode.V))
            {
                _dialogueGraph = Resources.Load<DialogueGraph>("Dialogues/TestDialogue");
                _dialogueGraph.Start();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _dialogueGraph?.NextPhrase();
            }
        }

        private void ZoomLookHandler()
        {
            
            // Приближение камеры к обьектам которые находяться на слое ZoomLook
            if (Physics.SphereCast(G.camera.transform.position, 1, G.camera.transform.forward, out RaycastHit hit, 100, LayerMask.GetMask("ZoomLook")))
            {
                G.camera.fieldOfView = Mathf.Lerp(G.camera.fieldOfView, 30, 0.01f);
            }
            else
            {
                G.camera.fieldOfView = Mathf.Lerp(G.camera.fieldOfView, 60, 0.01f);
            }
        }

        private void TryInteract()
        {
        
            if (Physics.Raycast(cameraRay, out RaycastHit hit, interactionDistance, interactableLayer))
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact();
                } 
                else if (grabableTags.Contains(hit.collider.tag)) // Если обьект перетаскиваемый
                {
                    grabbedObject = hit.collider.gameObject.GetComponent<Rigidbody>();
                    GrabObject();
                    
                }
        }
        
        void GrabObject()
        {
            AudioManager.Instance.PlaySoundRandomPitch("grab", 0.9f, 1.2f);
            currentGrabDistance = Mathf.Clamp(
                Vector3.Distance(grabbedObject.position, transform.position), 
                0f, interactionDistance);


            if (grabbedObject.TryGetComponent<FixedJoint>(out var joint))
                Destroy(joint);

            grabbedObject.useGravity = false;
            
        }
        
        public void ReleaseObject()
        {
            grabbedObject.useGravity = true;
            grabbedObject = null;
        }
        
        void MoveGrabbedObject()
        {
            Vector3 targetPoint = cameraRay.GetPoint(currentGrabDistance);
            Vector3 forceDirection = targetPoint - grabbedObject.position;

            // Ограничение ускорения
            Vector3 velocity = (targetPoint - grabbedObject.position) * 10f;
            velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
            grabbedObject.velocity = velocity;

            // Перемещение назад вперёд
            float scroll = _A_GrabDistance.ReadValue<Vector2>().y;
            float input = _A_GrabDistanceKB.ReadValue<Vector2>().y;
            currentGrabDistance = Mathf.Clamp(
                currentGrabDistance + (scroll * scrollSensitivity + input * keyboardSensitivity * Time.fixedDeltaTime),
                0,
                interactionDistance
            );
            
            if (grabbedObject.velocity.magnitude > maxVelocity)
            {
                grabbedObject.velocity = grabbedObject.velocity.normalized * maxVelocity;
            }
            grabbedObject.AddForce(forceDirection * (moveForce * Time.fixedDeltaTime));

            Quaternion targetRotation = Quaternion.LookRotation(grabbedObject.transform.forward, grabbedObject.transform.up);
            grabbedObject.angularVelocity = Vector3.zero;
            
            grabbedObject.MoveRotation(Quaternion.Slerp(
                grabbedObject.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            ));
        }
        
        
        
    }
}