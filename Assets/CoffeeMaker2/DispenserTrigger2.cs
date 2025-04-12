using UnityEngine;

public class DispenserTrigger2 : MonoBehaviour
{
    private bool _isSpoonIn;
    public GameObject _coffee;
    public GameObject _coffee2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spoon"))
        {
            _coffee.SetActive(false);
            _coffee2.SetActive(true);
            _isSpoonIn = true;
        }
    }
    public bool IsSpoonIn() { return _isSpoonIn; }
    public void SetSpoonIn(bool state) { _isSpoonIn = state; }


}
