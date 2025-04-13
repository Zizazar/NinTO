using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeGiveTriger : MonoBehaviour
{
    private bool isCoffeeIn;

    private Collider _colliderCurrent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CupFilled"))
        {
            _colliderCurrent = other;
            isCoffeeIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CupFilled"))
        {
            _colliderCurrent = null;

            isCoffeeIn = false;
        }
    }

    public bool IsCoffeeIn() { return isCoffeeIn; }

    public void Disintegrate()
    {
        StartCoroutine(DisitegrateSeq());
    }

    private IEnumerator DisitegrateSeq()
    {
        while (_colliderCurrent.gameObject.transform.localScale.z > 0)
        {
            _colliderCurrent.gameObject.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            yield return null;
        }
        Destroy(_colliderCurrent.gameObject);
        isCoffeeIn = false;
    }

}
