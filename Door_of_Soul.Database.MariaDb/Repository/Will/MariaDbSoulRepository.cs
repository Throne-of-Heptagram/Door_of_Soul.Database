using Door_of_Soul.Core.Protocol;
using Door_of_Soul.Database.Connection;
using Door_of_Soul.Database.DataStructure;
using Door_of_Soul.Database.Relation.Throne;
using Door_of_Soul.Database.Repository.Will;
using MySql.Data.MySqlClient;

namespace Door_of_Soul.Database.MariaDb.Repository.Will
{
    public class MariaDbSoulRepository : SoulRepository
    {
        public override OperationReturnCode Create(SoulData subject, out string errorMessage, out int subjectId)
        {
            OperationReturnCode returnCode = WillDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message, out int soulId) =>
                {
                    string sqlString = @"INSERT INTO SoulCollection 
                        (SoulName) VALUES (@soulName);
                        SELECT LAST_INSERT_ID();";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("soulName", subject.soulName);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                soulId = reader.GetInt32(0);
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                soulId = 0;
                                message = $"MariaDbSoulRepository Create DbNoChanged AnswerId:{subject.answerId}, SoulName:{subject.soulName}";
                                return OperationReturnCode.DbNoChanged;
                            }
                        }
                    }
                },
                result: out subjectId,
                errorMessage: out errorMessage);
            if (returnCode == OperationReturnCode.Successiful)
            {
                returnCode = TrinityRelation.Instance.LinkAnswerSoul(subject.answerId, subject.soulId, out errorMessage);
            }
            if (returnCode == OperationReturnCode.Successiful)
            {
                for (int i = 0; i < subject.avatarIds.Length; i++)
                {
                    returnCode = TrinityRelation.Instance.LinkSoulAvatar(subject.soulId, subject.avatarIds[i], out errorMessage);
                    if (returnCode != OperationReturnCode.Successiful)
                        break;
                }
            }
            return returnCode;
        }

        public override OperationReturnCode Delete(int subjectId, out string errorMessage)
        {
            return WillDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message) =>
                {
                    using (MySqlCommand command = new MySqlCommand("DELETE FROM SoulCollection WHERE SoulId = @soulId;", connection))
                    {
                        command.Parameters.AddWithValue("soulId", subjectId);
                        if (command.ExecuteNonQuery() > 0)
                        {
                            message = "";
                            return OperationReturnCode.Successiful;
                        }
                        else
                        {
                            message = $"MariaDbSoulRepository Delete DbNoChanged SoulId:{subjectId}";
                            return OperationReturnCode.DbNoChanged;
                        }
                    }
                },
                errorMessage: out errorMessage);
        }

        public override OperationReturnCode Read(int subjectId, out string errorMessage, out SoulData subject)
        {
            OperationReturnCode returnCode = WillDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message, out SoulData soulData) =>
                {
                    string sqlString = @"SELECT SoulName
                        from SoulCollection WHERE SoulId = @soulId;";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("soulId", subjectId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string soulName = reader.GetString(0);
                                soulData = new SoulData
                                {
                                    soulId = subjectId,
                                    soulName = soulName
                                };
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                soulData = default(SoulData);
                                message = $"MariaDbSoulRepository Read NotExisted, SoulId:{subjectId}";
                                return OperationReturnCode.NotExisted;
                            }
                        }
                    }
                },
                result: out subject,
                errorMessage: out errorMessage);
            if (returnCode != OperationReturnCode.Successiful)
            {
                return returnCode;
            }
            returnCode = TrinityRelation.Instance.SoulReadAnswerId(subject, out errorMessage, out subject);
            if (returnCode != OperationReturnCode.Successiful)
            {
                return returnCode;
            }
            return TrinityRelation.Instance.SoulReadAvatarIds(subject, out errorMessage, out subject);
        }

        public override OperationReturnCode Update(SoulData subject, out string errorMessage)
        {
            return WillDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message) =>
                {
                    string sqlString = @"UPDATE SoulCollection SET
                        SoulName = @soulName,
                        WHERE SoulId = @soulId;";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("soulName", subject.soulName);
                        command.Parameters.AddWithValue("soulId", subject.soulId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (command.ExecuteNonQuery() > 0)
                            {
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                message = $"MariaDbSoulRepository Update DbNoChanged SoulId:{subject.soulId}";
                                return OperationReturnCode.DbNoChanged;
                            }
                        }
                    }
                },
                errorMessage: out errorMessage);
        }
    }
}
