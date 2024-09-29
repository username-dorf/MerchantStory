using Core.Database;
using Zenject;

namespace Core.Storage
{
    public class ReceiveStrategyAmountableItem : IReceiveStrategy<AmountableItem>, IInitializable
    {
        private IRepository<AmountableItem> _repository;
        private IRepositoryFactory _repositoryFactory;

        public ReceiveStrategyAmountableItem(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public void Initialize()
        {
            _repository = _repositoryFactory.GetRepository<AmountableItem>();
        }

        public void Receive(AmountableItem item)
        {
            var hasRecord = _repository.Has(item.ID);
            if (!hasRecord)
            {
                _repository.Add(item);
            }
            else
            {
                var currentValue = _repository.GetById(item.ID);
                var newValue = currentValue.Amount + item.Amount;
                _repository.Update(new AmountableItem(item.ID, newValue));
            }
        }
    }
}