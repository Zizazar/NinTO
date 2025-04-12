using UnityEngine;

public class CupTrigger : MonoBehaviour
{
    private bool _isCupIn;
    private Collider _currentCup;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cup"))
        _currentCup = other;
            _isCupIn = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cup"))
        _currentCup = null;
            _isCupIn = false;
    }
    public bool IsCupIn() { return _isCupIn; }

    public GameObject FillCup()
    {
        if (_currentCup)
        {
            _currentCup.GetComponent<CupController>().Fill();
            return _currentCup.gameObject;
        }
        return null;
    }

}
