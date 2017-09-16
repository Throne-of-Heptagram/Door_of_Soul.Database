using Door_of_Soul.Database.Connection;
using MySql.Data.MySqlClient;
using System;

namespace Door_of_Soul.Database.MariaDb.Connection
{
    public class MariaDbThroneDataConnection : ThroneDataConnection<MySqlConnection>
    {
        protected override string DatabaseName { get { return "ThroneData"; } }

        public override bool Connect(string hostName, int port, string userName, string password, string databasePrefix, string charset, out string errorMessage)
        {
            string connectString = $"server={hostName};port={port};uid={userName};pwd={password};database={databasePrefix}.{DatabaseName};charset={charset}";
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
