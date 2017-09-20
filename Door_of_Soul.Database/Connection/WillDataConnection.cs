using System.Data.Common;

namespace Door_of_Soul.Database.Connection
{
    public abstract class WillDataConnection<TDbConnection> : DatabaseConnection<TDbConnection> where TDbConnection : DbConnection
    {
        public static WillDataConnection<TDbConnection> Instance { get; private set; }
        public static void Initialize(WillDataConnection<TDbConnection> instance)
        {
            Instance = instance;
        }

        protected override string DatabaseName { get { return "WillData"; } }
    }
}
