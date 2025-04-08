using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoneDialog", menuName = "Dialogue/NewDoneDialog")]
[System.Serializable]
public class DoneDialog : BasicDialog
{
    public Phrase[] wrongCoffeePhrases;
    public Phrase[] badCoffeeTimePhrases;
    public Phrase[] normCoffeeTimePhrases;
    public Phrase[] perfectCoffeeTimePhrases;


}
