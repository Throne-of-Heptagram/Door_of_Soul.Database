using Door_of_Soul.Core.Protocol;
using Door_of_Soul.Database.Connection;
using Door_of_Soul.Database.DataStructure;
using Door_of_Soul.Database.Repository.Eternity;
using MySql.Data.MySqlClient;

namespace Door_of_Soul.Database.MariaDb.Repository.Eternity
{
    public class MariaDbEndPointRepository : EndPointRepository
    {
        protected override OperationReturnCode Load(int subjectId, out string errorMessage, out EndPointData subject)
        {
            return EternityDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message, out EndPointData endPointData) =>
                {
                    string sqlString = @"SELECT ServerAddresses, ServerPort, ServerApplicationName
                        from EndPointCollection WHERE EndPointId = @endPointId;";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("endPointId", subjectId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string serverAddresses = reader.GetString(0);
                                int serverPort = reader.GetInt32(1);
                                string serverApplicationName = reader.GetString(2);

                                endPointData = new EndPointData
                                {
                                    endPointId = subjectId,
                                    serverAddresses = serverAddresses,
                                    serverPort = serverPort,
                                    serverApplicationName = serverApplicationName
                                };
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                endPointData = default(EndPointData);
                                message = $"MariaDbEndPointRepository Read NotExisted, EndPointId:{subjectId}";
                                return OperationReturnCode.NotExisted;
                            }
                        }
                    }
                },
                result: out subject,
                errorMessage: out errorMessage);
        }
    }
}
