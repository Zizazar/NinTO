using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class CupController : MonoBehaviour
{
    public GameObject cup;
    public GameObject cupFilled;


    public void Fill()
    {
        cup.SetActive(false);
        cupFilled.SetActive(true);
        tag = "CupFilled";
    }
}
