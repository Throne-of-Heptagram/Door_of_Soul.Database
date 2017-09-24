using Door_of_Soul.Core.Protocol;
using Door_of_Soul.Database.Connection;
using Door_of_Soul.Database.DataStructure;
using Door_of_Soul.Database.Relation;
using Door_of_Soul.Database.Repository;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Door_of_Soul.Database.MariaDb.Repository
{
    public class MariaDbAnswerRepository : AnswerRepository
    {
        public override OperationReturnCode Create(AnswerData subject, out string errorMessage, out int subjectId)
        {
            OperationReturnCode returnCode = ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message, out int answerId) =>
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
                                message = $"MariaDbAnswerRepository Create DbNoChanged AnswerName:{subject.answerName}";
                                return OperationReturnCode.DbNoChanged;
                            }
                        }
                    }
                },
                result: out subjectId,
                errorMessage: out errorMessage);
            if (returnCode == OperationReturnCode.Successiful)
            {
                for(int i = 0; i < subject.soulIds.Length; i++)
                {
                    returnCode = TrinityRelation.Instance.LinkAnswerSoul(subject.answerId, subject.soulIds[i], out errorMessage);
                    if (returnCode != OperationReturnCode.Successiful)
                        break;
                }
            }
            return returnCode;
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
                            message = $"MariaDbAnswerRepository Delete DbNoChanged AnswerId:{subjectId}";
                            return OperationReturnCode.DbNoChanged;
                        }
                    }
                },
                errorMessage: out errorMessage);
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
                errorMessage: out errorMessage);
        }

        public override OperationReturnCode Read(int subjectId, out string errorMessage, out AnswerData subject)
        {
            OperationReturnCode returnCode = ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message, out AnswerData answerData) =>
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
                                    answerId = subjectId,
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
                errorMessage: out errorMessage);
            if (returnCode == OperationReturnCode.Successiful)
            {
                return ReadSoulIds(subject, out errorMessage, out subject);
            }
            else
            {
                return returnCode;
            }
        }

        public override OperationReturnCode Register(string answerName, string basicPassword, out string errorMessage, out int answerId)
        {
            lock(registerLock)
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
                            basicPasswordHash = HashPassword(basicPassword),
                            soulIds = new int[0]
                        },
                        subjectId: out answerId,
                        errorMessage: out errorMessage);
                }
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
                                message = $"MariaDbAnswerRepository Update DbNoChanged AnswerId:{subject.answerId}";
                                return OperationReturnCode.DbNoChanged;
                            }
                        }
                    }
                },
                errorMessage: out errorMessage);
        }

        public override OperationReturnCode Login(string answerName, string basicPassword, out string errorMessage, out int answerId)
        {
            return ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message, out int id) =>
                {
                    string sqlString = @"SELECT  
                        AnswerId
                        from AnswerCollection WHERE AnswerName = @answerName AND BasicPasswordHash = @basicPasswordHash;";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("answerName", answerName);
                        command.Parameters.AddWithValue("basicPasswordHash", HashPassword(basicPassword));
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                id = reader.GetInt32(0);
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                id = 0;
                                message = $"MariaDbAnswerRepository Login Failed, AnswerName:{answerName}";
                                return OperationReturnCode.AuthenticationFailed;
                            }
                        }
                    }
                },
                result: out answerId,
                errorMessage: out errorMessage);
        }

        protected override OperationReturnCode ReadSoulIds(AnswerData sourceAnswerData, out string errorMessage, out AnswerData resultAnswerData)
        {
            return LoveDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message, out AnswerData answerData) =>
                {
                    string sqlString = @"SELECT SoulId
                        from AnswerSoulRelations WHERE AnswerId = @answerId;";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("answerId", sourceAnswerData.answerId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            List<int> soulIds = new List<int>();
                            while (reader.Read())
                            {
                                int soulId = reader.GetInt32(0);
                                soulIds.Add(soulId);
                            }
                            sourceAnswerData.soulIds = soulIds.ToArray();
                            answerData = sourceAnswerData;
                            message = "";
                            return OperationReturnCode.Successiful;
                        }
                    }
                },
                result: out resultAnswerData,
                errorMessage: out errorMessage);
        }
    }
}
