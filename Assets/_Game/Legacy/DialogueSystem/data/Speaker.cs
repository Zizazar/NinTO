using UnityEngine;

namespace _Game.Legacy.DialogueSystem.data
{
    [CreateAssetMenu(fileName ="NewSpeaker", menuName ="Dialogue/NewSpeaker")]
    [System.Serializable]
    public class Speaker : ScriptableObject
    {
        public string speakerName;
        public Color nameColor;
        public Sprite sprite;

    }
}
