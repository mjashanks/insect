using Insect.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.Stores
{
    public interface IAuthStore
    {
        User GetUserByName(string username);
        User GetUser(int id);
        Session GetSession(Guid id);
        byte[] GetPasswordHash(int userid);

        void LogEvent(Guid? userid, AuthAuditType type);
        Guid CreateNewSession(int userId, UserLevel level);
        void SaveUser(User session);
        void CreateUser(User session);
        void SavePasswordHash(int userid, byte[] hashedPassword);

    }
}
