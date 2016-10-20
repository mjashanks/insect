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

        public bool Verify(string emailVerify, string password, string twofactor)
        {
            if (!IsRegisterValid(emailVerify, password, twofactor)) 
                return false;

            var user =_authStore.GetUserByEmailVerificationPath(emailVerify);

            if (user == null
                || user.IsLocked
                || user.IsVerified
                || user.VerificationExpiryDate < DateTime.Now)
            {
                return false;
            }

            if (user.TwoFactorCode != twofactor)
            {
                IncrementFailCount(user);
                return false;
            }

            InitialiseUser(user);
            user.Salt = PasswordHasher.GenerateSalt();
            _authStore.SaveUser(user);

            var hashedPw = PasswordHasher.GenerateSaltedHash(password, user.Salt);
            _authStore.SavePasswordHash(user.Id, hashedPw);

            return true;

        }

        private bool IsRegisterValid(string username, string password, string twofactor)
        {
            return username.NotEmpty() && password.NotEmpty() && twofactor.NotEmpty();
        }

        private void InitialiseUser(User user)
        {
            user.FailedLoginCount = 0;
            user.IsAdministrator = false;
            user.IsLocked = false;
            user.PasswordExpiryDate = DateTime.Now.AddDays(30);
            user.TwoFactorCode = "";
        }

        public void ForgotPassword(string username)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(Guid sessionId, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
