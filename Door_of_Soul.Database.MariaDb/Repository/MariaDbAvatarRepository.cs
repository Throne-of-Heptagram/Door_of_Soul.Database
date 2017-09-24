using Door_of_Soul.Core.Protocol;
using Door_of_Soul.Database.Connection;
using Door_of_Soul.Database.DataStructure;
using Door_of_Soul.Database.Relation;
using Door_of_Soul.Database.Repository;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Door_of_Soul.Database.MariaDb.Repository
{
    public class MariaDbAvatarRepository : AvatarRepository
    {
        public override OperationReturnCode Create(AvatarData subject, out string errorMessage, out int subjectId)
        {
            OperationReturnCode returnCode = LifeDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message, out int avatarId) =>
                {
                    string sqlString = @"INSERT INTO AvatarCollection 
                        (EntityId, AvatarName) VALUES (@entityId, @avatarName);
                        SELECT LAST_INSERT_ID();";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("entityId", subject.entityId);
                        command.Parameters.AddWithValue("avatarName", subject.avatarName);
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
                                message = $"MariaDbAvatarRepository Create DbNoChanged AvatarName:{subject.avatarName}";
                                return OperationReturnCode.DbNoChanged;
                            }
                        }
                    }
                },
                result: out subjectId,
                errorMessage: out errorMessage);
            if (returnCode == OperationReturnCode.Successiful)
            {
                for (int i = 0; i < subject.soulIds.Length; i++)
                {
                    returnCode = TrinityRelation.Instance.LinkSoulAvatar(subject.avatarId, subject.soulIds[i], out errorMessage);
                    if (returnCode != OperationReturnCode.Successiful)
                        break;
                }
            }
            return returnCode;
        }

        public override OperationReturnCode Delete(int subjectId, out string errorMessage)
        {
            return LifeDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message) =>
                {
                    using (MySqlCommand command = new MySqlCommand("DELETE FROM AvatarCollection WHERE AvatarId = @avatarId;", connection))
                    {
                        command.Parameters.AddWithValue("avatarId", subjectId);
                        if (command.ExecuteNonQuery() > 0)
                        {
                            message = "";
                            return OperationReturnCode.Successiful;
                        }
                        else
                        {
                            message = $"MariaDbAvatarRepository Delete DbNoChanged AvatarId:{subjectId}";
                            return OperationReturnCode.DbNoChanged;
                        }
                    }
                },
                errorMessage: out errorMessage);
        }

        public override OperationReturnCode Read(int subjectId, out string errorMessage, out AvatarData subject)
        {
            OperationReturnCode returnCode = LifeDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message, out AvatarData avatarData) =>
                {
                    string sqlString = @"SELECT EntityId, AvatarName
                        from AvatarCollection WHERE AvatarId = @avatarId;";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("avatarId", subjectId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int entityId = reader.GetInt32(0);
                                string avatarName = reader.GetString(1);
                                avatarData = new AvatarData
                                {
                                    avatarId = subjectId,
                                    entityId = entityId,
                                    avatarName = avatarName
                                };
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                avatarData = default(AvatarData);
                                message = $"MariaDbAvatarRepository Read NotExisted, AvatarId:{subjectId}";
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

        public override OperationReturnCode Update(AvatarData subject, out string errorMessage)
        {
            return LifeDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message) =>
                {
                    string sqlString = @"UPDATE AvatarCollection SET
                        EntityId = @entityId, AvatarName = @avatarName,
                        WHERE AvatarId = @avatarId;";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("entityId", subject.entityId);
                        command.Parameters.AddWithValue("avatarName", subject.avatarName);
                        command.Parameters.AddWithValue("avatarId", subject.avatarId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (command.ExecuteNonQuery() > 0)
                            {
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                message = $"MariaDbAvatarRepository Update DbNoChanged AvatarId:{subject.avatarId}";
                                return OperationReturnCode.DbNoChanged;
                            }
                        }
                    }
                },
                errorMessage: out errorMessage);
        }

        protected override OperationReturnCode ReadSoulIds(AvatarData sourceAvatarData, out string errorMessage, out AvatarData resultAvatarData)
        {
            return LoveDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message, out AvatarData avatarData) =>
                {
                    string sqlString = @"SELECT SoulId
                        from SoulAvatarRelations WHERE AvatarId = @avatarId;";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("avatarId", sourceAvatarData.avatarId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            List<int> soulIds = new List<int>();
                            while (reader.Read())
                            {
                                int soulId = reader.GetInt32(0);
                                soulIds.Add(soulId);
                            }
                            sourceAvatarData.soulIds = soulIds.ToArray();
                            avatarData = sourceAvatarData;
                            message = "";
                            return OperationReturnCode.Successiful;
                        }
                    }
                },
                result: out resultAvatarData,
                errorMessage: out errorMessage);
        }
    }
}
