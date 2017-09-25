using Door_of_Soul.Core.Protocol;
using Door_of_Soul.Database.DataStructure;

namespace Door_of_Soul.Database.Relation.Throne
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

        public abstract OperationReturnCode AnswerReadSoulIds(AnswerData sourceAnswerData, out string errorMessage, out AnswerData resultAnswerData);
        public abstract OperationReturnCode SoulReadAnswerId(SoulData sourceSoulData, out string errorMessage, out SoulData resultSoulData);
        public abstract OperationReturnCode SoulReadAvatarIds(SoulData sourceSoulData, out string errorMessage, out SoulData resultSoulData);
        public abstract OperationReturnCode AvatarReadSoulIds(AvatarData sourceAvatarData, out string errorMessage, out AvatarData resultAvatarData);
    }
}
