using Door_of_Soul.Core.Protocol;
using Door_of_Soul.Database.Connection;
using Door_of_Soul.Database.DataStructure;
using Door_of_Soul.Database.Relation.Throne;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Door_of_Soul.Database.MariaDb.Relation.Throne
{
    public class MariaDbTrinityRelation : TrinityRelation
    {
        public override OperationReturnCode LinkAnswerSoul(int answerId, int soulId, out string errorMessage)
        {
            return ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message) =>
                {
                    string sqlString = @"INSERT IGNORE INTO AnswerSoulRelations 
                        (AnswerId, SoulId) VALUES (@answerId, @soulId);";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("answerId", answerId);
                        command.Parameters.AddWithValue("soulId", soulId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (command.ExecuteNonQuery() > 0)
                            {
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                message = $"MariaDbTrinityRelation LinkAnswerSoul DbNoChanged AnswerId:{answerId}, SoulId:{soulId}";
                                return OperationReturnCode.DbNoChanged;
                            }
                        }
                    }
                },
                errorMessage: out errorMessage);
        }

        public override OperationReturnCode LinkSoulAvatar(int soulId, int avatarId, out string errorMessage)
        {
            return ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message) =>
                {
                    string sqlString = @"INSERT IGNORE INTO SoulAvatarRelations 
                        (SoulId, AvatarId) VALUES (@soulId, @avatarId);";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("soulId", soulId);
                        command.Parameters.AddWithValue("avatarId", avatarId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (command.ExecuteNonQuery() > 0)
                            {
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                message = $"MariaDbTrinityRelation LinkSoulAvatar DbNoChanged SoulId:{soulId}, AvatarId:{avatarId}";
                                return OperationReturnCode.DbNoChanged;
                            }
                        }
                    }
                },
                errorMessage: out errorMessage);
        }

        public override OperationReturnCode UnlinkAnswerSoul(int answerId, int soulId, out string errorMessage)
        {
            return ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message) =>
                {
                    string sqlString = @"DELETE FROM AnswerSoulRelations WHERE AnswerId = @answerId AND SoulId = @soulId;";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("answerId", answerId);
                        command.Parameters.AddWithValue("soulId", soulId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (command.ExecuteNonQuery() > 0)
                            {
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                message = $"MariaDbTrinityRelation UnlinkAnswerSoul DbNoChanged AnswerId:{answerId}, SoulId:{soulId}";
                                return OperationReturnCode.DbNoChanged;
                            }
                        }
                    }
                },
                errorMessage: out errorMessage);
        }

        public override OperationReturnCode UnlinkSoulAvatar(int soulId, int avatarId, out string errorMessage)
        {
            return ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message) =>
                {
                    string sqlString = @"DELETE FROM SoulAvatarRelations WHERE SoulId = @soulId AND AvatarId = @avatarId;";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("soulId", soulId);
                        command.Parameters.AddWithValue("avatarId", avatarId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (command.ExecuteNonQuery() > 0)
                            {
                                message = "";
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                message = $"MariaDbTrinityRelation UnlinkSoulAvatar DbNoChanged SoulId:{soulId}, AvatarId:{avatarId}";
                                return OperationReturnCode.DbNoChanged;
                            }
                        }
                    }
                },
                errorMessage: out errorMessage);
        }

        public override OperationReturnCode AnswerReadSoulIds(AnswerData sourceAnswerData, out string errorMessage, out AnswerData resultAnswerData)
        {
            return ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
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

        public override OperationReturnCode SoulReadAnswerId(SoulData sourceSoulData, out string errorMessage, out SoulData resultSoulData)
        {
            return ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message, out SoulData soulData) =>
                {
                    string sqlString = @"SELECT AnswerId
                        from AnswerSoulRelations WHERE SoulId = @soulId;";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("soulId", sourceSoulData.soulId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int answerId = reader.GetInt32(0);
                                sourceSoulData.answerId = answerId;
                                message = "";
                                soulData = sourceSoulData;
                                return OperationReturnCode.Successiful;
                            }
                            else
                            {
                                message = $"MariaDbSoulRepository ReadAnswerId cannot find any AnswerId, SoulId:{sourceSoulData.soulId}";
                                soulData = sourceSoulData;
                                return OperationReturnCode.NotExisted;
                            }
                        }
                    }
                },
                result: out resultSoulData,
                errorMessage: out errorMessage);
        }

        public override OperationReturnCode SoulReadAvatarIds(SoulData sourceSoulData, out string errorMessage, out SoulData resultSoulData)
        {
            return ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
                query: (MySqlConnection connection, out string message, out SoulData soulData) =>
                {
                    string sqlString = @"SELECT AvatarId
                        from SoulAvatarRelations WHERE SoulId = @soulId;";
                    using (MySqlCommand command = new MySqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("soulId", sourceSoulData.soulId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            List<int> avatarIds = new List<int>();
                            while (reader.Read())
                            {
                                int avatarId = reader.GetInt32(0);
                                avatarIds.Add(avatarId);
                            }
                            sourceSoulData.avatarIds = avatarIds.ToArray();
                            soulData = sourceSoulData;
                            message = "";
                            return OperationReturnCode.Successiful;
                        }
                    }
                },
                result: out resultSoulData,
                errorMessage: out errorMessage);
        }

        public override OperationReturnCode AvatarReadSoulIds(AvatarData sourceAvatarData, out string errorMessage, out AvatarData resultAvatarData)
        {
            return ThroneDataConnection<MySqlConnection>.Instance.SendQuery(
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
