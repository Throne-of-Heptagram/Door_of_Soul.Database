using Door_of_Soul.Core.Protocol;

namespace Door_of_Soul.Database.Relation
{
    public abstract class TrinityRelation
    {
        public static TrinityRelation Instance { get; private set; }
        public static void Initialize(TrinityRelation instance)
        {
            Instance = instance;
        }

        public abstract OperationReturnCode LinkAnswerSoul(int answerId, int soulId, out string errorMessage);
        public abstract OperationReturnCode UnlinkAnswerSoul(int answerId, int soulId, out string errorMessage);
        public abstract OperationReturnCode LinkSoulAvatar(int soulId, int avatarId, out string errorMessage);
        public abstract OperationReturnCode UnlinkSoulAvatar(int soulId, int avatarId, out string errorMessage);
    }
}
