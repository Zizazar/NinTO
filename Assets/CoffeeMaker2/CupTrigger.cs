using UnityEngine;

public class CupTrigger : MonoBehaviour
{
    private bool _isCupIn;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cup"))
            _isCupIn = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cup"))
            _isCupIn = false;
    }
    public bool IsCupIn() { return _isCupIn; }


}
