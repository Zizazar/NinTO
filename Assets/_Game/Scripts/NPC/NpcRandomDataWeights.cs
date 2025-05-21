using _Game.Scripts.Utils;

namespace _Game.Scripts.NPC
{
    public class NpcRandomDataWeights
    {
        public static readonly WeightedValue<NpcRole>[] Roles =
        {
            new() {value = NpcRole.Normal, weight = 0.6f},
            new() {value = NpcRole.Killer, weight = 0.4f}
        };
        
        public static readonly WeightedValue<CoffeeType>[] Coffee =
        {
            new() {value = CoffeeType.Americano, weight = 0.3f},
            new() {value = CoffeeType.Cappucino, weight = 0.6f},
            new() {value = CoffeeType.Exspresso, weight = 0.5f},
            new() {value = CoffeeType.Late, weight = 0.5f}
        };
    }
}