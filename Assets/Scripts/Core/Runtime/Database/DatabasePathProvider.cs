using System.IO;
using UnityEngine;

namespace Core.Database
{
    public interface IDatabasePathProvider
    {
        string GetDatabasePath();
    }

    public class DatabasePathProvider : IDatabasePathProvider
    {
        public string GetDatabasePath()
        {
            return Path.Combine(Application.streamingAssetsPath, "database.db");
        }
    }

}