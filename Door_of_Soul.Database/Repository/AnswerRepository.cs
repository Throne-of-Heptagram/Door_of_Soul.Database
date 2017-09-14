using Door_of_Soul.Core.Protocol;
using Door_of_Soul.Database.DataStructure;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Door_of_Soul.Database.Repository
{
    public abstract class AnswerRepository : CrudSubjectRepository<int, AnswerData>
    {
        public static AnswerRepository Instance { get; protected set; }
        public abstract OperationReturnCode IsAnswerNameValid(string answerName, out string errorMessage);
        public abstract OperationReturnCode Register(string answerName, string basicPassword, out int answerId, out string errorMessage);

        public string HashPassword(string password)
        {
            SHA512 sha512 = new SHA512CryptoServiceProvider();
            string passwordHash = Convert.ToBase64String(sha512.ComputeHash(Encoding.Default.GetBytes(password)));
            return passwordHash;
        }
    }
}
