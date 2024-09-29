using Core.Database;
using Zenject;

namespace Core.Storage
{
    public class SpendStrategyAmountableItem : ISpendStrategy<AmountableItem>, IInitializable
    {
        private IRepositoryFactory _repositoryFactory;
        private IRepository<AmountableItem> _repository;

        public SpendStrategyAmountableItem(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public void Initialize()
        {
            _repository = _repositoryFactory.GetRepository<AmountableItem>();
        }
        public void Spend(AmountableItem item)
        {
            if(!CanSpend(item))
                return;
            var currentValue = _repository.GetById(item.ID);
            var newValue = currentValue.Amount - item.Amount;
            _repository.Update(new AmountableItem(item.ID, newValue));
        }

        public bool CanSpend(AmountableItem item)
        {
            var hasRecord = _repository.Has(item.ID);
            if (!hasRecord)
                return false;
            
            var currentValue = _repository.GetById(item.ID);
            return item.Amount <= currentValue.Amount;
        }

        public bool TrySpend(AmountableItem item)
        {
            if (!CanSpend(item))
                return false;
            
            Spend(item);
            return true;
        }
    }
}