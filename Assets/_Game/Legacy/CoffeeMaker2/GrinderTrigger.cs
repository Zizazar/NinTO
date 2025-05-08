using UnityEngine;

namespace _Game.Legacy.CoffeeMaker2
{
    public class GrinderTrigger : MonoBehaviour
    {
        private bool _isSpoonIn;
        public GameObject _coffee;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Spoon"))
            {
                _coffee.SetActive(true);
                _isSpoonIn = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Spoon"))
                _isSpoonIn = false;
        }
        public bool IsSpoonIn() { return _isSpoonIn; }


    }
}
