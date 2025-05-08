using _Game.Legacy.NPC;
using UnityEngine;

namespace _Game.Legacy.DialogueSystem.data
{
    public class BasicDialog : ScriptableObject
    {

    }

    [System.Serializable]
    public struct Phrase
    {
        public CharacterType speaker;
        [TextArea]
        public string text;
    }
}