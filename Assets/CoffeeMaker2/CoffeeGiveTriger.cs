using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeGiveTriger : MonoBehaviour
{
    private bool isCoffeeIn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CupFilled"))
        {
            isCoffeeIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CupFilled"))
        {
            isCoffeeIn = false;
        }
    }

    public bool IsCoffeeIn() { return isCoffeeIn; }
}
