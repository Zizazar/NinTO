using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EndDialog", menuName = "Dialogue/NewEndDialog")]
[System.Serializable]
public class EndDialog : BasicDialog
{

    public Phrase[] phrases;
    
}
