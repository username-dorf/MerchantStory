using System;
using System.Collections.Generic;

namespace Core.Utils
{
    public interface IContainingProbability
    {
        float Chance { get; }
    }

    public static class ProbabilityExtensions
    {
        public static T ChooseRandom<T>(this T[] array) where T : IContainingProbability
        {
            float totalWeight = 0;
            foreach (var weightedItem in array)
            {
                totalWeight += weightedItem.Chance;
            }

            float randomValue = UnityEngine.Random.Range(0f, totalWeight);

            float cumulativeWeight = 0;
            foreach (var weightedItem in array)
            {
                cumulativeWeight += weightedItem.Chance;
                if (randomValue < cumulativeWeight)
                {
                    return weightedItem;
                }
            }

            throw new InvalidOperationException("Failed to select a random item.");
        }
        
        public static T ChooseRandom<T>(this ICollection<T> array) where T : IContainingProbability
        {
            float totalWeight = 0;
            foreach (var weightedItem in array)
            {
                totalWeight += weightedItem.Chance;
            }

            float randomValue = UnityEngine.Random.Range(0f, totalWeight);

            float cumulativeWeight = 0;
            foreach (var weightedItem in array)
            {
                cumulativeWeight += weightedItem.Chance;
                if (randomValue < cumulativeWeight)
                {
                    return weightedItem;
                }
            }

            throw new InvalidOperationException("Failed to select a random item.");
        }
        
    }
}