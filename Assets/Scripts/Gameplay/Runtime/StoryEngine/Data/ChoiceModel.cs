using Gameplay.StoryEngine.Constructor;
using UniRx;
using Zenject;

namespace Gameplay.StoryEngine
{
    public interface IChoiceModel
    {
        void Execute();
    }
    public sealed class ChoiceModel : IChoiceModel
    {
        public ReactiveCommand Command { get; private set; }

        public ChoiceModel(ChoiceObject choiceObject, ChoiceCommandStrategyFactory choiceCommandStrategyFactory)
        {
            Command = choiceCommandStrategyFactory.Create(choiceObject);
        }
        public void Execute()
        {
            Command.Execute();
        }
        
        public class Factory : PlaceholderFactory<ChoiceObject,ChoiceModel>
        {
            
        }
    }
}