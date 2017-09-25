using System.Data.Common;

namespace Door_of_Soul.Database.Connection
{
    public abstract class EternityDataConnection<TDbConnection> : DatabaseConnection<TDbConnection> where TDbConnection : DbConnection
    {
        public static EternityDataConnection<TDbConnection> Instance { get; private set; }
        public static void Initialize(EternityDataConnection<TDbConnection> instance)
        {
            Instance = instance;
        }

        protected override string DatabaseName { get { return "EternityData"; } }
    }
}
