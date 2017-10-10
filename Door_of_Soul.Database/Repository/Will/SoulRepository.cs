using Door_of_Soul.Database.DataStructure;

namespace Door_of_Soul.Database.Repository.Will
{
    public abstract class SoulRepository : CrudSubjectDbRepository<int, SoulData>
    {
        public static SoulRepository Instance { get; private set; }
        public static void Initialize(SoulRepository instance)
        {
            Instance = instance;
        }
    }
}
