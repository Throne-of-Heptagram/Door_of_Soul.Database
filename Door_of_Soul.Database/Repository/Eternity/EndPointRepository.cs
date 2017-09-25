using Door_of_Soul.Database.DataStructure;

namespace Door_of_Soul.Database.Repository.Eternity
{
    public abstract class EndPointRepository : CrudSubjectRepository<int, EndPointData>
    {
        public static EndPointRepository Instance { get; private set; }
        public static void Initialize(EndPointRepository instance)
        {
            Instance = instance;
        }
    }
}
