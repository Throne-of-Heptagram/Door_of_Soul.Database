using Door_of_Soul.Core.Protocol;
using Door_of_Soul.Database.Connection;
using Door_of_Soul.Database.DataStructure;
using Door_of_Soul.Database.Repository;
using MySql.Data.MySqlClient;

namespace Door_of_Soul.Database.MariaDb.Repository
{
    class MariaDbAnswerRepository : AnswerRepository
    {
        static MariaDbAnswerRepository()
        {
            Instance = new MariaDbAnswerRepository();
        }

        public override OperationReturnCode Create(AnswerData subject, out int subjectId, out string errorMessage)
        {
            return ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out int answerId, out string message) =>
                {
                    string sqlString = @"INSERT INTO AnswerCollection 
                        (AnswerName, BasicPasswordHash) VALUES (@answerName, @basicPasswordHash);
                        SELECT LAST_INSERT_ID();";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("answerName", subject.answerName);
                        command.Parameters.AddWithValue("basicPasswordHash", subject.basicPasswordHash);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                answerId = reader.GetInt32(0);
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                answerId = 0;
                                message = "MariaDbAnswerRepository Create DbNoChanged";
                                return OperationReturnCode.DbNoChanged;
                            }
                        }
                    }
                },
                result: out subjectId,
                errorMessage: out errorMessage,
                useLock: true);
        }

        public override OperationReturnCode Delete(int subjectId, out string errorMessage)
        {
            return ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message) => 
                {
                    using (MySqlCommand command = new MySqlCommand("DELETE FROM AnswerCollection WHERE AnswerId = @answerId;", connection))
                    {
                        command.Parameters.AddWithValue("answerId", subjectId);
                        if (command.ExecuteNonQuery() > 0)
                        {
                            message = "";
                            return OperationReturnCode.Successiful;
                        }
                        else
                        {
                            message = "MariaDbAnswerRepository Delete DbNoChanged";
                            return OperationReturnCode.DbNoChanged;
                        }
                    }
                },
                errorMessage: out errorMessage,
                useLock: true);
        }

        public override OperationReturnCode IsAnswerNameValid(string answerName, out string errorMessage)
        {
            if (answerName.Length < 1 || answerName.Length > 20)
            {
                errorMessage = $"AnswerName length should be 1~20, your submit length:{answerName.Length}";
                return OperationReturnCode.ParameterFormateError;
            }
            return ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message) =>
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT COUNT(AnswerId) FROM AnswerCollection WHERE AnswerName = @answerName;", connection))
                    {
                        command.Parameters.AddWithValue("answerName", answerName);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            if (reader.GetInt32(0) == 0)
                            {
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                message = $"AnswerName already be used, your submit:{answerName}";
                                return OperationReturnCode.Duplicated;
                            }
                        }
                    }
                },
                errorMessage: out errorMessage,
                useLock: true);
        }

        public override OperationReturnCode Read(int subjectId, out AnswerData subject, out string errorMessage)
        {
            return ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out AnswerData answerData, out string message) =>
                {
                    string sqlString = @"SELECT  
                        AnswerName
                        from AnswerCollection WHERE AnswerId = @answerId;";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("answerId", subjectId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string answerNamed = reader.GetString(0);
                                answerData = new AnswerData
                                {
                                    answerName = answerNamed
                                };
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                answerData = default(AnswerData);
                                message = $"MariaDbAnswerRepository Read NotExisted, AnswerId:{subjectId}";
                                return OperationReturnCode.NotExisted;
                            }
                        }
                    }
                },
                result: out subject,
                errorMessage: out errorMessage,
                useLock: true);
        }

        public override OperationReturnCode Register(string answerName, string basicPassword, out int answerId, out string errorMessage)
        {
            OperationReturnCode answerNameCheckReturn = IsAnswerNameValid(answerName, out errorMessage);
            if (answerNameCheckReturn != OperationReturnCode.Successiful)
            {
                answerId = 0;
                return answerNameCheckReturn;
            }
            else
            {
                return Create(
                    subject: new AnswerData
                    {
                        answerName = answerName,
                        basicPasswordHash = HashPassword(basicPassword)
                    },
                    subjectId: out answerId,
                    errorMessage: out errorMessage);
            }
        }

        public override OperationReturnCode Update(AnswerData subject, out string errorMessage)
        {
            return ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message) =>
                {
                    string sqlString = @"UPDATE AnswerCollection SET
                        AnswerName = @answerName,
                        WHERE AnswerId = @answerId;";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("answerName", subject.answerName);
                        command.Parameters.AddWithValue("answerId", subject.answerId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (command.ExecuteNonQuery() > 0)
                            {
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                message = $"MariaDbAnswerRepository Update DbNoChanged";
                                return OperationReturnCode.DbNoChanged;
                            }
                        }
                    }
                },
                errorMessage: out errorMessage,
                useLock: true);
        }
    }
}
