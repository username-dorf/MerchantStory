using System;
using Core.Database;
using Core.Runtime.Storage;
using Zenject;

namespace Core.Storage.Receiver
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
                if (item.Amount < 0)
                {
                    throw new Exception("Cant add item with negative amount without record");
                }
                _repository.Add(item);
            }
            else
            {
                var currentValue = _repository.GetById(item.ID);
                var newValue = currentValue.Amount + item.Amount;
                if(newValue<0)
                {
                    throw new Exception("Cant have negative amount of item");
                }
                _repository.Update(new AmountableItem(item.ID, newValue));
            }
           
        }
    }
}