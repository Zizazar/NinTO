using UnityEngine;

namespace CoffeeMakerSystem
{
    public class DraggableObject : MonoBehaviour
    {
        public Transform targetZone;
        public string objectTag;
        public float snapDistance = 0.5f;

        private bool isDragging = false;
        private Vector3 originalPosition;
        private CoffeeMakerController coffeeMakerController;
        private Transform cursorTransform;

        [SerializeField] private Camera _camera;
        public float distanceFromCamera = 0.5f;

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
            isDragging = true;
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
            if (isDragging)
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
        }

        bool IsTargetZoneBehindObject()
        {
            if (targetZone == null)
            {
                return false;
            }

            Vector3 rayOrigin = Camera.main.transform.position;
            Vector3 rayDirection = (transform.position - rayOrigin).normalized;

            float distanceToRay = Vector3.Cross(rayDirection, targetZone.position - rayOrigin).magnitude;

            return distanceToRay <= snapDistance;
        }

        void SnapToTargetZone()
        {
            transform.position = targetZone.position;
        }

        void ReturnToOriginalPosition()
        {
            isDragging = false;
            transform.position = originalPosition;
        }
    }
}