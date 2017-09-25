using Door_of_Soul.Core.Protocol;
using Door_of_Soul.Database.Connection;
using Door_of_Soul.Database.DataStructure;
using Door_of_Soul.Database.Repository.Eternity;
using MySql.Data.MySqlClient;

namespace Door_of_Soul.Database.MariaDb.Repository.Eternity
{
    public class MariaDbEndPointRepository : EndPointRepository
    {
        public override OperationReturnCode Create(EndPointData subject, out string errorMessage, out int subjectId)
        {
            return EternityDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message, out int avatarId) =>
                {
                    string sqlString = @"INSERT INTO EndPointCollection 
                        (EndPointId, ServerAddresses, ServerPort, ServerApplicationName) VALUES (@endPointId, @serverAddresses, @serverPort, @serverApplicationName);
                        SELECT LAST_INSERT_ID();";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("endPointId", subject.endPointId);
                        command.Parameters.AddWithValue("serverAddresses", subject.serverAddresses);
                        command.Parameters.AddWithValue("serverPort", subject.serverPort);
                        command.Parameters.AddWithValue("serverApplicationName", subject.serverApplicationName);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                avatarId = reader.GetInt32(0);
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                avatarId = 0;
                                message = $"MariaDbEndPointRepository Create DbNoChanged ServerApplicationName:{subject.serverApplicationName}";
                                return OperationReturnCode.DbNoChanged;
                            }
                        }
                    }
                },
                result: out subjectId,
                errorMessage: out errorMessage);
        }

        public override OperationReturnCode Delete(int subjectId, out string errorMessage)
        {
            return EternityDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message) =>
                {
                    using (MySqlCommand command = new MySqlCommand("DELETE FROM EndPointCollection WHERE EndPointId = @endPointId;", connection))
                    {
                        command.Parameters.AddWithValue("endPointId", subjectId);
                        if (command.ExecuteNonQuery() > 0)
                        {
                            message = "";
                            return OperationReturnCode.Successiful;
                        }
                        else
                        {
                            message = $"MariaDbEndPointRepository Delete DbNoChanged EndPointId:{subjectId}";
                            return OperationReturnCode.DbNoChanged;
                        }
                    }
                },
                errorMessage: out errorMessage);
        }

        public override OperationReturnCode Read(int subjectId, out string errorMessage, out EndPointData subject)
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

        public override OperationReturnCode Update(EndPointData subject, out string errorMessage)
        {
            return LifeDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message) =>
                {
                    string sqlString = @"UPDATE EndPointCollection SET
                        ServerAddresses = @serverAddresses, ServerPort = @serverPort, ServerApplicationName = @serverApplicationName
                        WHERE EndPointId = @endPointId;";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("serverAddresses", subject.serverAddresses);
                        command.Parameters.AddWithValue("serverPort", subject.serverPort);
                        command.Parameters.AddWithValue("serverApplicationName", subject.serverApplicationName);
                        command.Parameters.AddWithValue("endPointId", subject.endPointId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (command.ExecuteNonQuery() > 0)
                            {
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                message = $"MariaDbEndPointRepository Update DbNoChanged ServerApplicationName:{subject.serverApplicationName}";
                                return OperationReturnCode.DbNoChanged;
                            }
                        }
                    }
                },
                errorMessage: out errorMessage);
        }
    }
}
