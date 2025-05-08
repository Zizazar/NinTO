using UnityEngine;

namespace _Game.Legacy.CoffeeMaker2
{
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
}
