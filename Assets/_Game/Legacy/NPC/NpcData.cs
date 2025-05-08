using _Game.Legacy.DialogueSystem.data;
using UnityEngine;

namespace _Game.Legacy.NPC
{
    [CreateAssetMenu(fileName = "NPCdata", menuName = "NPC/NewNpcData")]
    [System.Serializable]
    public class NpcData : ScriptableObject
    {
        public string npcName;
        public CharacterType npcType = CharacterType.Normal;
        public CoffeeType coffeeType;
        public int handbookCardId;

        [Header("Dialogues")]
        public OrderDialog orderDialog;
        public DoneDialog doneDialog;



    }
    [System.Serializable]
    public enum CoffeeType
    {
        Cappuccino,
        Expresso,
        Latte
    }
    [System.Serializable]
    public enum CharacterType
    {
        Normal,
        Player,
        Police
    }
}