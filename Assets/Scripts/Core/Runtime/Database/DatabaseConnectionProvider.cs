using System;
using SQLite4Unity3d;
using Zenject;

namespace Core.Database
{
    public interface IDatabaseConnectionProvider
    {
        SQLiteConnection GetConnection();
    }

    public class DatabaseConnectionProvider : IDatabaseConnectionProvider, IInitializable, IDisposable
    {
        private readonly IDatabasePathProvider _pathProvider;
        private SQLiteConnection _connection;

        public DatabaseConnectionProvider(IDatabasePathProvider pathProvider)
        {
            _pathProvider = pathProvider;
        }

        public void Initialize()
        {
            string dbPath = _pathProvider.GetDatabasePath();
            _connection = new SQLiteConnection(dbPath);
        }

        public SQLiteConnection GetConnection()
        {
            return _connection;
        }

        public void Dispose()
        {
            _connection?.Close();
        }
    }

}