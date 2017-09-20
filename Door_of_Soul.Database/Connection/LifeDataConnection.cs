using System.Data.Common;

namespace Door_of_Soul.Database.Connection
{
    public abstract class LifeDataConnection<TDbConnection> : DatabaseConnection<TDbConnection> where TDbConnection : DbConnection
    {
        public static LifeDataConnection<TDbConnection> Instance { get; private set; }
        public static void Initialize(LifeDataConnection<TDbConnection> instance)
        {
            Instance = instance;
        }

        protected override string DatabaseName { get { return "LifeData"; } }
    }
}
