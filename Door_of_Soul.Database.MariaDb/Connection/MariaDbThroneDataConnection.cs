using Door_of_Soul.Database.Connection;
using MySql.Data.MySqlClient;
using System;

namespace Door_of_Soul.Database.MariaDb.Connection
{
    class MariaDbThroneDataConnection : ThroneDataConnection<MySqlConnection>
    {
        static MariaDbThroneDataConnection()
        {
            Instance = new MariaDbThroneDataConnection();
        }
        protected override string DatabaseName { get { return "ThroneData"; } }

        private MariaDbThroneDataConnection() { }

        public override bool Connect(string hostName, string userName, string password, string database, string charset, out string errorMessage)
        {
            string connectString = $"server={hostName};uid={userName};pwd={password};database={3}.{DatabaseName};charset={charset}";
            try
            {
                Connection = new MySqlConnection(connectString);
                errorMessage = "";
                return true;
            }
            catch (Exception exception)
            {
                errorMessage = $"Message:{exception.Message}, StackTrace:{exception.StackTrace}";
                return false;
            }
        }
    }
}
