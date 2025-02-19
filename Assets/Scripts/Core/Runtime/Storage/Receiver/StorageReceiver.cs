using System.Collections.Generic;
using UniRx;

namespace Core.Storage
{
    public interface IStorageReceiver
    {
        void Receive(IItem item);
        void Receive(IEnumerable<IItem> items);
    }
    
    public class StorageReceiver : IStorageReceiver
    {
        public ReactiveCommand<IItem> OnReceive { get; }
        private List<IReceiveStrategy> _strategies;

        public StorageReceiver(List<IReceiveStrategy> strategies)
        {
            _strategies = strategies;
            OnReceive = new ReactiveCommand<IItem>();
        }

        public void Receive(IItem item)
        {
            foreach (var strategy in _strategies)
            {
                if (strategy.IsApplicable(item))
                {
                    strategy.Receive(item);
                    OnReceive.Execute(item);
                    return;
                }
            }
        }

        public void Receive(IEnumerable<IItem> items)
        {
            foreach (var item in items)
            {
                Receive(item);
            }
        }
    }
    
    public interface IReceiveStrategy
    {
        bool IsApplicable(IItem item);
        void Receive(IItem item);
    }
    
    public interface IReceiveStrategy<T>: IReceiveStrategy where T: IItem
    {
        bool IReceiveStrategy.IsApplicable(IItem item)
        {
            return item is T;
        }

        void IReceiveStrategy.Receive(IItem item)
        {
            Receive((T) item);
        }
        
        void Receive(T item);
    }
}