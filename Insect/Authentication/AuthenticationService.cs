using Insect.Domain;
using Insect.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private IAuthStore _authStore;

        public AuthenticationService(IAuthStore authStore)
        {
            _authStore = authStore;
        }

        public Guid? Login(string username, string password)
        {
            var user = _authStore.GetUserByName(username);

            if(user == null)
            {
                return null;
            }

            if(user.IsLocked)
            {
                return null;
            }

            var storedHash = _authStore.GetPasswordHash(user.Id);
            var computedHash = PasswordHasher.GenerateSaltedHash(password, user.Salt);

            if(storedHash != computedHash)
            {
                IncrementFailCount(user);
                return null;
            }

            if(user.FailedLoginCount > 0)
            {
                ResetFailedCount(user);
            }
            
            return _authStore.CreateNewSession(user.Id);
        }

        private void IncrementFailCount(User user)
        {
            user.FailedLoginCount++;
            if (user.FailedLoginCount == 3)
            {
                user.IsLocked = true;
            }

            _authStore.SaveUser(user);
        }

        private void ResetFailedCount(User user)
        {
            user.FailedLoginCount = 0;
            _authStore.SaveUser(user);
        }

        public bool Register(UserRegistrationDetails details)
        {
            throw new NotImplementedException();
        }

        public Guid ForgotPassword(string username, string securityAnswer1, string securityAnswer2)
        {
            throw new NotImplementedException();
        }

        public Tuple<string, string> GetSecurityQuestions(string username)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(Guid sessionId, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public bool TwoFactorVerify(Guid sessionId, string verificationCode, string newPassword)
        {
            throw new NotImplementedException();
        }

        public bool ResetAccount(Guid sessionId, string userName)
        {
            throw new NotImplementedException();
        }
    }
}
