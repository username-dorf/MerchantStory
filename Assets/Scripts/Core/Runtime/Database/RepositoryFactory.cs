namespace Core.Database
{
    public interface IRepositoryFactory
    {
        IRepository<T> GetRepository<T>() where T : new();
    }

    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IDatabaseConnectionProvider _connectionProvider;

        public RepositoryFactory(IDatabaseConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public IRepository<T> GetRepository<T>() where T : new()
        {
            return new Repository<T>(_connectionProvider);
        }
    }

}