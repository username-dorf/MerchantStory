using System;
using System.Linq;
using System.Reflection;
using SQLite4Unity3d;

namespace Core.Database
{
    public interface IRepository<T>
    {
        void Add(T item);
        T GetById(string id);
        void Update(T item);
        void InsertOrReplace(T item);
        void Delete(string id);
        bool Has(string id);
    }

    public class Repository<T> : IRepository<T> where T : new()
    {
        private readonly SQLiteConnection _connection;

        public Repository(IDatabaseConnectionProvider connectionProvider)
        {
            _connection = connectionProvider.GetConnection();
            InitializeTable();
        }

        private void InitializeTable()
        {
            lock (DatabaseLock.LockObject)
            {
                _connection.CreateTable<T>();
            }
        }

        public void Add(T item)
        {
            lock (DatabaseLock.LockObject)
            {
                _connection.Insert(item);
            }
        }

        public T GetById(string id)
        {
            lock (DatabaseLock.LockObject)
            {
                return _connection.Find<T>(id);
            }
        }

        public void InsertOrReplace(T item)
        {
            lock (DatabaseLock.LockObject)
            {
                _connection.InsertOrReplace(item);
            }
        }
        public void Update(T item)
        {
            lock (DatabaseLock.LockObject)
            {
                _connection.Update(item);
            }
        }

        public void Delete(string id)
        {
            lock (DatabaseLock.LockObject)
            {
                _connection.Delete<T>(id);
            }
        }
        
        //this is more efficient than calling GetById and checking for null
        public bool Has(string id)
        {
            lock (DatabaseLock.LockObject)
            {
                string tableName = GetTableName();
                string primaryKeyColumn = GetPrimaryKeyColumnName();

                var cmd = _connection.CreateCommand($"SELECT COUNT(1) FROM \"{tableName}\" WHERE \"{primaryKeyColumn}\" = ?", id);
                int count = cmd.ExecuteScalar<int>();
                return count > 0;
            }
        }

        private string GetTableName()
        {
            var tableAttr = typeof(T).GetCustomAttribute<TableAttribute>();
            return tableAttr != null ? tableAttr.Name : typeof(T).Name;
        }

        private string GetPrimaryKeyColumnName()
        {
            var pkProperty = typeof(T).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<PrimaryKeyAttribute>() != null);

            if (pkProperty != null)
            {
                var columnAttr = pkProperty.GetCustomAttribute<ColumnAttribute>();
                return columnAttr != null ? columnAttr.Name : pkProperty.Name;
            }
            else
            {
                throw new Exception($"Primary key not defined for type {typeof(T).Name}");
            }
        }

    }

}