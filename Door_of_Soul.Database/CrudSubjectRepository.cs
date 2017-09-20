using Door_of_Soul.Core.Protocol;

namespace Door_of_Soul.Database
{
    public abstract class CrudSubjectRepository<TId, TSubject>
    {
        public abstract OperationReturnCode Create(TSubject subject, out string errorMessage, out TId subjectId);
        public abstract OperationReturnCode Read(TId subjectId, out string errorMessage, out TSubject subject);
        public abstract OperationReturnCode Update(TSubject subject, out string errorMessage);
        public abstract OperationReturnCode Delete(TId subjectId, out string errorMessage);
    }
}
