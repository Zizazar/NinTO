using System.Linq;
using UnityEngine;

namespace _Game.Scripts.Utils
{
    [System.Serializable]
    public struct WeightedValue<T>
    {
        public T value;
        public float weight;
    }
    
    public static class WeightedRandomizer
    {
        // Выбор элемента с учётом весов
        public static T GetWeightedValue<T>(WeightedValue<T>[] weightedValues)
        {
            float totalWeight = weightedValues.Sum(w => w.weight);
            
            float randomValue = Random.Range(0f, totalWeight);

            // Ищем подходящий элемент
            foreach (var entry in weightedValues)
            {
                if (randomValue < entry.weight)
                {
                    return entry.value;
                }
                randomValue -= entry.weight;
            }

            // На случай ошибок (возвращаем последний элемент)
            return weightedValues[^1].value;
        }
    }
}