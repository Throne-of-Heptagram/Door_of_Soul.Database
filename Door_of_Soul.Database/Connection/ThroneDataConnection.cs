using System.Data.Common;

namespace Door_of_Soul.Database.Connection
{
    public abstract class ThroneDataConnection<TDbConnection> : DatabaseConnection<TDbConnection> where TDbConnection : DbConnection
    {
        public static ThroneDataConnection<TDbConnection> Instance { get; private set; }
        public static void Initialize(ThroneDataConnection<TDbConnection> instance)
        {
            Instance = instance;
        }
    }
}
