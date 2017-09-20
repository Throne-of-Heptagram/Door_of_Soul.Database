using Door_of_Soul.Core.Protocol;
using Door_of_Soul.Database.DataStructure;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Door_of_Soul.Database.Repository
{
    public abstract class AnswerRepository : CrudSubjectRepository<int, AnswerData>
    {
        public static AnswerRepository Instance { get; private set; }
        public static void Initialize(AnswerRepository instance)
        {
            Instance = instance;
        }

        protected object registerLock = new object();

        public abstract OperationReturnCode IsAnswerNameValid(string answerName, out string errorMessage);
        public abstract OperationReturnCode Register(string answerName, string basicPassword, out string errorMessage, out int answerId);
        public abstract OperationReturnCode Login(string answerName, string basicPassword, out string errorMessage, out int answerId);

        protected string HashPassword(string password)
        {
            SHA512 sha512 = new SHA512CryptoServiceProvider();
            string passwordHash = Convert.ToBase64String(sha512.ComputeHash(Encoding.Default.GetBytes(password)));
            return passwordHash;
        }
        protected abstract OperationReturnCode ReadSoulIds(AnswerData sourceAnswerData, out string errorMessage, out AnswerData resultAnswerData);
    }
}
