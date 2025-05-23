using _Game.Scripts.Utils;
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
        [ShowAssetPreview]
        public Sprite npcPhoto;
        public WeightedValue<DialogueGraph>[] dialogueGraphs;
        
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
        Witness,
        Involved,
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