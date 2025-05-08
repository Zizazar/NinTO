using UnityEngine;

namespace _Game.Legacy.Player
{
    public class ParallaxCamera : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float sensitivity = 1f;
        [SerializeField] private float maxAngle = 10f;
        [SerializeField] private float smoothTime = 0.2f;

        private Vector3 _initialRotation;
        private Vector3 _currentRotation;
        private Vector3 _velocity;

        private void Start()
        {
            _initialRotation = transform.localEulerAngles;
            _currentRotation = _initialRotation;
        }

        private void Update()
        {
            // �������� ������� ���� ������������ ������ ������
            Vector2 mousePosition = Input.mousePosition;
            Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            Vector2 offset = mousePosition - screenCenter;

            // ����������� ��������
            offset.x /= screenCenter.x;
            offset.y /= screenCenter.y;

            // ������������ ������� �������
            Vector3 targetRotation = new Vector3(
                Mathf.Clamp(-offset.y * maxAngle * sensitivity, -maxAngle, maxAngle),
                Mathf.Clamp(offset.x * maxAngle * sensitivity, -maxAngle, maxAngle),
                0
            );

            // ������ ������������� ������� �������
            _currentRotation = Vector3.SmoothDamp(_currentRotation, targetRotation, ref _velocity, smoothTime);

            // ��������� ������� � ������ ������������ ���������� ���������
            transform.localEulerAngles = _initialRotation + _currentRotation;
        }
    }
}