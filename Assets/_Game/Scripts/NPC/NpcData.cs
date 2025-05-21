using NaughtyAttributes;
using UnityEngine;

namespace _Game.Scripts.NPC
{
    [CreateAssetMenu(menuName = "NPC/Data")]
    public class NpcData : ScriptableObject
    {
        public string npcName;
        [Required]
        public GameObject prefab;
        public Sprite npcPhoto;
        
        public NpcMood mood = NpcMood.Neutral;
        public NpcRole role;
        public CoffeeType order;
    }

    public enum NpcMood
    {
        Neutral,
        Happy,
        Angry
    }

    public enum NpcRole
    {
        Normal,
        Killer
    }

    public enum CoffeeType
    {
        Americano,
        Cappucino,
        Late,
        Exspresso
    }
}