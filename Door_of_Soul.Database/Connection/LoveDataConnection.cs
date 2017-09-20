using System.Data.Common;

namespace Door_of_Soul.Database.Connection
{
    public abstract class LoveDataConnection<TDbConnection> : DatabaseConnection<TDbConnection> where TDbConnection : DbConnection
    {
        public static LoveDataConnection<TDbConnection> Instance { get; private set; }
        public static void Initialize(LoveDataConnection<TDbConnection> instance)
        {
            Instance = instance;
        }

        protected override string DatabaseName { get { return "LoveData"; } }
    }
}
