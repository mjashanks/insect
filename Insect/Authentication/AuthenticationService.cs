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

            if(!PasswordHasher.CompareByteArrays(storedHash, computedHash))
            {
                IncrementFailCount(user);
                return null;
            }

            if(user.FailedLoginCount > 0)
            {
                ResetFailedCount(user);
            }
            
            return _authStore.CreateNewSession(user.Id, user.UserLevel);
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
            if (!UserRegistrationDetails.IsValid(details)) 
                return false;

            var user =_authStore.GetUserByName(details.Username);

            if (user == null)
                return false;

            MapUser(details, user);
            user.Salt = PasswordHasher.GenerateSalt();

            _authStore.SaveUser(user);

            var hashedPw = PasswordHasher.GenerateSaltedHash(details.Password, user.Salt);
            _authStore.SavePasswordHash(user.Id, hashedPw);

            return true;

        }

        private void MapUser(UserRegistrationDetails details, User user)
        {
            user.MobileFor2Factor = details.MobileFor2Factor;
            user.SecurityAnswer1 = details.SecurityAnswer1;
            user.SecurityAnswer2 = details.SecurityAnswer2;
            user.SecurityQuestion1 = details.SecurityQuestion1;
            user.SecurityQuestion2 = details.SecurityQuestion2;
            user.PasswordExpiryDate = DateTime.Now.AddDays(14);

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
