using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Insect.Domain;

namespace Insect.Stores
{
    public class AuthStore : IAuthStore
    {
        private IDbConnection conn;

        public AuthStore(Config config)
        {
            conn = DbConnectionFactory.CreateAndOpen(config.Server, config.Database);
        }

        public User GetUserByName(string username)
        {
           return conn.GetAll<User>()
                      .FirstOrDefault(u => u.Username == username);
        }

        public Domain.User GetUser(int id)
        {
            return conn.Get<User>(id);
        }

        public Domain.Session GetSession(Guid id)
        {
            return conn.GetAll<Session>().FirstOrDefault(s => s.SessionId == id);
        }

        public byte[] GetPasswordHash(int userid)
        {
            return conn.GetAll<PasswordHash>()
                       .Where(h => h.UserId == userid)
                       .Select(h => h.Hash)
                       .FirstOrDefault();
        }

        public void LogEvent(Guid? userid, Domain.AuthAuditType type)
        {
            throw new NotImplementedException();
        }

        public Guid CreateNewSession(int userId, UserLevel level)
        {
            var sess = new Session
            {
                ExpiryDate = DateTime.Now.AddDays(1),
                SessionId = Guid.NewGuid(),
                Level = level,
                UserId = userId
            };

            var id = conn.Insert(sess);

            return sess.SessionId;
        }

        public void SaveUser(Domain.User user)
        {
            conn.Update(user);
        }

        public void CreateUser(Domain.User user)
        {
            conn.Insert(user);
        }

        public void SavePasswordHash(int userid, byte[] hashedPassword)
        {
            conn.Insert(new PasswordHash { UserId = userid, Hash = hashedPassword });
        }
    }
}
