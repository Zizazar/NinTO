using UnityEngine;

namespace _Game.Legacy.CoffeeMaker2
{
    public class DispenserTrigger : MonoBehaviour
    {
        private bool _isDispenserIn;
        [SerializeField] private Transform _point;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Dispenser"))
            {

                Rigidbody rb = other.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                other.transform.position = _point.position;
                other.transform.rotation = _point.rotation;
                _isDispenserIn = true;
            }
        }


        public bool IsDispenserIn() { return _isDispenserIn; }


    }
}
