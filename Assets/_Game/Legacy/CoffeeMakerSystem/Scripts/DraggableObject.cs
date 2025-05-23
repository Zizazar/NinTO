using System.Collections;
using UnityEngine;

namespace _Game.Legacy.CoffeeMakerSystem.Scripts
{
    public class DraggableObject : MonoBehaviour
    {
        public Transform targetZone;
        public Transform dispenserZone;
        public string objectTag;
        public float snapDistance = 50f;

        private bool isDragging = false;
        private Vector3 originalPosition;
        private CoffeeMakerController coffeeMakerController;
        private Transform cursorTransform;

        [SerializeField] private Camera _camera;
        public float distanceFromCamera = 0.5f;
        
        [SerializeField] private GameObject _coffee;
        [SerializeField] private GameObject _coffee2;
        [SerializeField] private GameObject _particl1;
        [SerializeField] private GameObject _particl2;
        [SerializeField] private GameObject _fullCup;
        [SerializeField] private GameObject _cup;

        void Start()
        {
            originalPosition = transform.position;

            coffeeMakerController = FindObjectOfType<CoffeeMakerController>();
            if (coffeeMakerController == null)
            {
                Debug.LogError("CoffeeMakerController не найден!");
            }

            cursorTransform = FindObjectOfType<CoffeeMakerCursor>()?.transform;
            if (cursorTransform == null)
            {
                Debug.LogError("CoffeeMakerCursor не найден!");
            }
        }

        void OnMouseDown()
        {
            if(objectTag != "Button")
                isDragging = true;
            else
            {
                _particl1.SetActive(true);
                _particl2.SetActive(true);
                StartCoroutine(WaitAndExecute());
            }
        }

        void OnMouseUp()
        {
            isDragging = false;
            if (IsTargetZoneBehindObject())
            {
                SnapToTargetZone();
                coffeeMakerController?.OnObjectPlaced(objectTag);
            }
            else
            {
                ReturnToOriginalPosition();
            }
        }

        void Update()
        {
            if (isDragging && objectTag != "Spoon")
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                Vector3 planeNormal = -_camera.transform.forward.normalized;
                Vector3 planePoint = _camera.transform.position + _camera.transform.forward * distanceFromCamera;
                Plane plane = new Plane(planeNormal, planePoint);

                if (plane.Raycast(ray, out float enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);
                    transform.position = hitPoint;
                }
            }
            else if(isDragging && objectTag == "Spoon")
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                Vector3 planeNormal = -_camera.transform.forward.normalized;
                Vector3 planePoint = _camera.transform.position + _camera.transform.forward * distanceFromCamera;
                Plane plane = new Plane(planeNormal, planePoint);

                if (plane.Raycast(ray, out float enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);
                    transform.position = hitPoint;
                }
                if (IsTargetZoneBehindObject())
                {
                    if(!_coffee.activeInHierarchy)
                        FillSpoon();
                }
                if (IsDispenserZoneBehindObject())
                {
                    if(_coffee.activeInHierarchy)
                        FillDispenser();
                    coffeeMakerController?.OnObjectPlaced(objectTag);
                }
            }
        }

        bool IsTargetZoneBehindObject()
        {
            if (targetZone == null)
            {
                return false;
            }

            // Получаем позиции объекта и целевой зоны на экране
            Vector3 objectScreenPos = _camera.WorldToScreenPoint(transform.position);
            Vector3 targetScreenPos = _camera.WorldToScreenPoint(targetZone.position);

            // Проверяем расстояние между ними на экране
            float screenDistance = Vector3.Distance(objectScreenPos, targetScreenPos);

            // Если расстояние меньше порогового значения, считаем проверку успешной
            return screenDistance <= snapDistance;
        }
        bool IsDispenserZoneBehindObject()
        {
            if (dispenserZone == null)
            {
                return false;
            }

            // Получаем позиции объекта и целевой зоны на экране
            Vector3 objectScreenPos = _camera.WorldToScreenPoint(transform.position);
            Vector3 targetScreenPos = _camera.WorldToScreenPoint(dispenserZone.position);

            // Проверяем расстояние между ними на экране
            float screenDistance = Vector3.Distance(objectScreenPos, targetScreenPos);

            // Если расстояние меньше порогового значения, считаем проверку успешной
            return screenDistance <= snapDistance;
        }

        void SnapToTargetZone()
        {
            transform.position = targetZone.position;
        }

        void FillSpoon()
        {
            _coffee.SetActive(true);
        }

        void FillDispenser()
        {
            _coffee.SetActive(false);
            _coffee2.SetActive(true);
        }
        IEnumerator WaitAndExecute()
        {
            yield return new WaitForSeconds(4f);
            _particl1.SetActive(false);
            _particl2.SetActive(false);
            coffeeMakerController?.OnObjectPlaced(objectTag);
            _fullCup.SetActive(true);
            _cup.SetActive(false);
            
        }
        void ReturnToOriginalPosition()
        {
            isDragging = false;
            transform.position = originalPosition;
            if (objectTag == "Dispenser" && _coffee.activeInHierarchy)
            {
                coffeeMakerController?.OnObjectPlaced(objectTag);
            }
        }
    }
}