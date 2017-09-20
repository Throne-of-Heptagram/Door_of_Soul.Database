using Door_of_Soul.Core.Protocol;
using Door_of_Soul.Database.DataStructure;

namespace Door_of_Soul.Database.Repository
{
    public abstract class SoulRepository : CrudSubjectRepository<int, SoulData>
    {
        public static SoulRepository Instance { get; private set; }
        public static void Initialize(SoulRepository instance)
        {
            Instance = instance;
        }

        protected abstract OperationReturnCode ReadAnswerId(SoulData sourceSoulData, out string errorMessage, out SoulData resultSoulData);
        protected abstract OperationReturnCode ReadAvatarIds(SoulData sourceSoulData, out string errorMessage, out SoulData resultSoulData);
    }
}
