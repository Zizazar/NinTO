using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderDialog", menuName = "Dialogue/NewOrderDialog")]
[System.Serializable]
public class OrderDialog : BasicDialog
{

    public Phrase phrase;

    [System.Serializable] // Перенести потом в контроллер игрока или что то такое
    public enum CoffeeType
    {
        Cappuccino
    }
    
}
