using System;
using Gameplay.Runtime.StoryEngine;
using UniRx;
using Zenject;

namespace Gameplay.StoryEngine
{
    public class SituationModel
    {
        public ReactiveProperty<string> Description { get; private set; }
        public ReactiveProperty<SituationContextObject> Context { get; private set; }

        public SituationModel(string description, SituationContextObject context)
        {
            Description = new ReactiveProperty<string>(description);
            Context = new ReactiveProperty<SituationContextObject>(context);
        }


        public class Factory : IFactory<SituationObject,SituationModel>
        {
            public SituationModel Create(SituationObject situationObject)
            {
                var context = SelectRandom(situationObject.Contexts);
                return new SituationModel(situationObject.Description, context);
            }
            private SituationContextObject SelectRandom(SituationContextObject[] weightedItems)
            {
                float totalWeight = 0;
                foreach (var weightedItem in weightedItems)
                {
                    totalWeight += weightedItem.Chance;
                }

                float randomValue = UnityEngine.Random.Range(0f, totalWeight);

                float cumulativeWeight = 0;
                foreach (var weightedItem in weightedItems)
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
}