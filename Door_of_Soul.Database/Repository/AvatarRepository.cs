using Door_of_Soul.Core.Protocol;
using Door_of_Soul.Database.DataStructure;

namespace Door_of_Soul.Database.Repository
{
    public abstract class AvatarRepository : CrudSubjectRepository<int, AvatarData>
    {
        public static AvatarRepository Instance { get; private set; }
        public static void Initialize(AvatarRepository instance)
        {
            Instance = instance;
        }

        protected abstract OperationReturnCode ReadSoulIds(AvatarData sourceAvatarData, out string errorMessage, out AvatarData resultAvatarData);
    }
}
