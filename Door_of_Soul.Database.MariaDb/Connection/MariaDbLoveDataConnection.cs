using Door_of_Soul.Database.Connection;
using MySql.Data.MySqlClient;
using System;

namespace Door_of_Soul.Database.MariaDb.Connection
{
    public class MariaDbLoveDataConnection : LoveDataConnection<MySqlConnection>
    {
        public override bool Connect(string serverAddress, int port, string username, string password, string databasePrefix, string charset, out string errorMessage)
        {
            string connectString = $"server={serverAddress};port={port};uid={username};pwd={password};database={databasePrefix}.{DatabaseName};charset={charset}";
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
