using UnityEngine;

namespace CoffeeMakerSystem
{
    public class CoffeeMakerCursor : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public float distanceFromCamera = 1f; 
        
        void Update()
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
}