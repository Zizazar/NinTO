using UnityEngine;

namespace _Game.Legacy.DialogueSystem.data
{
    [CreateAssetMenu(fileName = "EndDialog", menuName = "Dialogue/NewEndDialog")]
    [System.Serializable]
    public class EndDialog : BasicDialog
    {

        public Phrase[] phrases;
    
    }
}
