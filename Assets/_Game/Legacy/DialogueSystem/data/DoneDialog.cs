using UnityEngine;

namespace _Game.Legacy.DialogueSystem.data
{
    [CreateAssetMenu(fileName = "DoneDialog", menuName = "Dialogue/NewDoneDialog")]
    [System.Serializable]
    public class DoneDialog : BasicDialog
    {
        public Phrase[] wrongCoffeePhrases;
        public Phrase[] badCoffeeTimePhrases;
        public Phrase[] normCoffeeTimePhrases;
        public Phrase[] perfectCoffeeTimePhrases;


    }
}
