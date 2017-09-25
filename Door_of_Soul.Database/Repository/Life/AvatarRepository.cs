using Door_of_Soul.Database.DataStructure;

namespace Door_of_Soul.Database.Repository.Life
{
    public abstract class AvatarRepository : CrudSubjectRepository<int, AvatarData>
    {
        public static AvatarRepository Instance { get; private set; }
        public static void Initialize(AvatarRepository instance)
        {
            Instance = instance;
        }
    }
}
