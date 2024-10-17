using System;
using System.Collections.Generic;
using Gameplay.StoryEngine.Constructor;
using UniRx;

namespace Gameplay.StoryEngine
{
    public interface IChoiceCommandStrategy
    {
        bool IsApplicable(ChoiceObject choiceObject);
        Action Create(ChoiceObject choiceObject);
    }

    public interface IChoiceCommandStrategy<T> : IChoiceCommandStrategy where T : ChoiceObject
    {
        Action Create(T choiceObject);

        bool IChoiceCommandStrategy.IsApplicable(ChoiceObject choiceObject)
        {
            return choiceObject is T;
        }

        Action IChoiceCommandStrategy.Create(ChoiceObject choiceObject)
        {
            return Create((T) choiceObject);
        }
    }

    public class ChoiceCommandStrategyFactory
    {
        private List<IChoiceCommandStrategy> _strategies;

        public ChoiceCommandStrategyFactory(List<IChoiceCommandStrategy> strategies)
        {
            _strategies = strategies;
        }

        public Action Create(ChoiceObject choiceObject)
        {
            foreach (var choiceCommandStrategy in _strategies)
            {
                if (choiceCommandStrategy.IsApplicable(choiceObject))
                    return choiceCommandStrategy.Create(choiceObject);
            }

            throw new System.InvalidOperationException("No strategy found for choice object");
        }
    }
}