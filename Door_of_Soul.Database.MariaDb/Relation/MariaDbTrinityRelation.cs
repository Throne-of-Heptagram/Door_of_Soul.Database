using Door_of_Soul.Core.Protocol;
using Door_of_Soul.Database.Connection;
using Door_of_Soul.Database.Relation;
using MySql.Data.MySqlClient;

namespace Door_of_Soul.Database.MariaDb.Relation
{
    public class MariaDbTrinityRelation : TrinityRelation
    {
        public override OperationReturnCode LinkAnswerSoul(int answerId, int soulId, out string errorMessage)
        {
            return LoveDataConnection<MySqlConnection>.Instance.SendQuery(
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
                errorMessage: out errorMessage,
                useLock: true);
        }

        public override OperationReturnCode LinkSoulAvatar(int soulId, int avatarId, out string errorMessage)
        {
            return LoveDataConnection<MySqlConnection>.Instance.SendQuery(
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
                errorMessage: out errorMessage,
                useLock: true);
        }

        public override OperationReturnCode UnlinkAnswerSoul(int answerId, int soulId, out string errorMessage)
        {
            return LoveDataConnection<MySqlConnection>.Instance.SendQuery(
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
                errorMessage: out errorMessage,
                useLock: true);
        }

        public override OperationReturnCode UnlinkSoulAvatar(int soulId, int avatarId, out string errorMessage)
        {
            return LoveDataConnection<MySqlConnection>.Instance.SendQuery(
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
                errorMessage: out errorMessage,
                useLock: true);
        }
    }
}
