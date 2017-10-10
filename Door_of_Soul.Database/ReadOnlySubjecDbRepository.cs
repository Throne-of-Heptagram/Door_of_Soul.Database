using Door_of_Soul.Core.Protocol;
using System.Collections.Concurrent;

namespace Door_of_Soul.Database
{
    public abstract class ReadOnlySubjectDbRepository<TId, TSubject>
    {
        private ConcurrentDictionary<TId, TSubject> cacheDictionary = new ConcurrentDictionary<TId, TSubject>();
        public OperationReturnCode Read(TId subjectId, out string errorMessage, out TSubject subject)
        {
            if(cacheDictionary.ContainsKey(subjectId))
            {
                errorMessage = "";
                subject = cacheDictionary[subjectId];
                return OperationReturnCode.Successiful;
            }
            else
            {
                OperationReturnCode returnCode = Load(subjectId, out errorMessage, out subject);
                cacheDictionary.TryAdd(subjectId, subject);
                return returnCode;
            }
        }
        protected abstract OperationReturnCode Load(TId subjectId, out string errorMessage, out TSubject subject);
    }
}
