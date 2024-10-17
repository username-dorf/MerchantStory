using System;
using Gameplay.StoryEngine.Constructor;
using Zenject;

namespace Gameplay.StoryEngine
{
    public interface IChoiceModel
    {
        void Execute();
    }
    public sealed class ChoiceModel : IChoiceModel
    {
        public Action Command { get; private set; }

        public ChoiceModel(ChoiceObject choiceObject, ChoiceCommandStrategyFactory choiceCommandStrategyFactory)
        {
            Command = choiceCommandStrategyFactory.Create(choiceObject);
        }
        public void Execute()
        {
            Command?.Invoke();
        }
        
        public class Factory : PlaceholderFactory<ChoiceObject,ChoiceModel>
        {
            
        }
    }
}