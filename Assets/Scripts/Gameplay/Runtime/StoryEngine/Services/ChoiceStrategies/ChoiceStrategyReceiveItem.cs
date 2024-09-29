using Gameplay.StoryEngine.Constructor;
using UniRx;

namespace Gameplay.StoryEngine
{
    public class ChoiceStrategyReceiveItem : IChoiceCommandStrategy<ChoiceWithItemObject>
    {
        public ChoiceStrategyReceiveItem()
        {
            
        }
        public ReactiveCommand Create(ChoiceWithItemObject choiceObject)
        {
            return null;
        }
    }
}