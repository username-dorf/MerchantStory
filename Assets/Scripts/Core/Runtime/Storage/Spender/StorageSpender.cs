using System.Collections.Generic;
using UniRx;

namespace Core.Storage
{
    public interface IStorageSpender
    {
        void Spend(IItem item);
        bool CanSpend(IItem item);
        bool TrySpend(IItem item);
    }
    public class StorageSpender : IStorageSpender
    {
        public ReactiveCommand<IItem> OnSpend { get; }
        private List<ISpendStrategy> _strategies;

        public StorageSpender(List<ISpendStrategy> strategies)
        {
            _strategies = strategies;
            OnSpend = new ReactiveCommand<IItem>();
        }

        public void Spend(IItem item)
        {
            foreach (var strategy in _strategies)
            {
                if (strategy.IsApplicable(item))
                {
                    strategy.Spend(item);
                    OnSpend.Execute(item);
                    return;
                }
            }
        }

        public bool CanSpend(IItem item)
        {
            throw new System.NotImplementedException();
        }

        public bool TrySpend(IItem item)
        {
            throw new System.NotImplementedException();
        }
    }
    public interface ISpendStrategy
    {
        bool IsApplicable(IItem item);
        void Spend(IItem item);
        bool CanSpend(IItem item);
        bool TrySpend(IItem item);
    }
    
    public interface ISpendStrategy<T>: ISpendStrategy where T: IItem
    {
        bool ISpendStrategy.IsApplicable(IItem item)
        {
            return item is T;
        }

        void ISpendStrategy.Spend(IItem item)
        {
            Spend((T) item);
        }
        
        bool ISpendStrategy.CanSpend(IItem item)
        {
            return CanSpend((T) item);
        }
        
        bool ISpendStrategy.TrySpend(IItem item)
        {
            return TrySpend((T) item);
        }
        
        
        void Spend(T item);
        bool CanSpend(T item);
        bool TrySpend(T item);
    }
}