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
        User GetUser(Guid id);
        Session GetSession(Guid id);
        string GetPasswordHash(Guid userid);

        void LogEvent(Guid? userid, AuthAuditType type);
        Guid CreateNewSession(Guid userId);
        void SaveUser(User session);

    }
}
