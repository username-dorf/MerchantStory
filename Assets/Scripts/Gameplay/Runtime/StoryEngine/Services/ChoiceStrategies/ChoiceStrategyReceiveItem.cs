using System;
using System.Linq;
using Core.Storage;
using Gameplay.StoryEngine.Constructor;
using UniRx;

namespace Gameplay.StoryEngine
{
    public class ChoiceStrategyReceiveItem : IChoiceCommandStrategy<ChoiceWithItemObject>
    {
        private IStorageReceiver _storageReceiver;

        public ChoiceStrategyReceiveItem(IStorageReceiver storageReceiver)
        {
            _storageReceiver = storageReceiver;
        }
        public Action Create(ChoiceWithItemObject choiceObject)
        {
            return () =>
            {
                IItem[] items = choiceObject.Output.Select(item => (IItem)item).ToArray();
                _storageReceiver.Receive(items);
            };
        }
    }
}