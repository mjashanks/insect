using Insect.Authentication;
using Insect.Domain;
using Insect.Stores;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.Tests.AuthenticationTests
{

    [TestClass]
    public class RegisterTests : AuthenticationTestBase
    {
        [TestMethod]
        public void Register___when_valid___should_update_user_with_initial_details()
        {
            var existingUser = TestData.User();
            var username = existingUser.Username;
            var twofactor = existingUser.TwoFactorCode;
            var password = "CorrectHorseBatteryStaple";

            _authStore.Setup(a => a.GetUserByName(username))
                      .Returns(existingUser);
            
            _authStore.Setup(a => 
                a.SaveUser(It.Is<User>(u =>
                    u.FailedLoginCount == 0
                    && u.IsLocked == false
                    && u.IsAdministrator == false
                )))
                .Verifiable();
            
            var result = _authenticationService.Register(username, password, existingUser.TwoFactorCode);
                       
            Assert.IsTrue(result);
            _authStore.Verify();
        }

        [TestMethod]
        public void Register___when_valid___should_store_hashed_password_and_generate_salt()
        {
            var existingUser = TestData.User();
            var username = existingUser.Username;
            var twofactor = existingUser.TwoFactorCode;
            var password = "CorrectHorseBatteryStaple";

            existingUser.Salt = "";

            _authStore.Setup(a => a.GetUserByName(username))
                      .Returns(existingUser);

            _authStore.Setup(a =>
                a.SaveUser(It.Is<User>(u => u.Salt.Length > 0)))
                .Verifiable();

            // ned to get this lazily, as salt is changed by service...
            Func<byte[]> expectedHash = () => PasswordHasher.GenerateSaltedHash(password, existingUser.Salt);
            
            _authStore.Setup(a => 
                a.SavePasswordHash(existingUser.Id, It.Is<byte[]>(h => PasswordHasher.CompareByteArrays(expectedHash(), h)))
                ).Verifiable();
                      

            var result = _authenticationService.Register(username, password, twofactor);

            Assert.IsTrue(result);
            _authStore.Verify();
        }

        [TestMethod]
        public void Register___when_username_not_exists___should_return_false()
        {
            var username = "non_existent_user";
            _authStore.Setup(a => a.GetUserByName(username))
                      .Returns<User>(null);

            var result = _authenticationService.Register(username, "password", "tf");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Register___when_twofactor_code_incorrect___should_return_false_and_not_set_password()
        {
            var existingUser = TestData.User();
            var username = existingUser.Username;
            var twofactor = "an incorrect twofactor code";
            var password = "CorrectHorseBatteryStaple";

            _authStore.Setup(a => a.GetUserByName(username))
                      .Returns(existingUser);

            _authStore.Setup(a => a.SavePasswordHash(It.IsAny<int>(), It.IsAny<byte[]>()))
                .Throws(new Exception("should not update password when two factor is incorrect"));

            var result = _authenticationService.Register(username, password, twofactor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Register___when_twofactor_code_incorrect___should_increment_failedcount()
        {
            var existingUser = TestData.User();
            var username = existingUser.Username;
            var twofactor = "an incorrect twofactor code";
            var password = "CorrectHorseBatteryStaple";
            existingUser.FailedLoginCount = 0;

            _authStore.Setup(a => a.GetUserByName(username))
                      .Returns(existingUser);

            _authStore.Setup(a => a.SaveUser(It.Is<User>(u => u.FailedLoginCount == 1)))
                      .Verifiable();

            var result = _authenticationService.Register(username, password, twofactor);

            _authStore.Verify();
        }

        [TestMethod]
        public void Register___when_twofactor_code_incorrect_for_third_time___should_lock_user()
        {
            var existingUser = TestData.User();
            var username = existingUser.Username;
            var twofactor = "an incorrect twofactor code";
            var password = "CorrectHorseBatteryStaple";
            existingUser.FailedLoginCount = 2;

            _authStore.Setup(a => a.GetUserByName(username))
                      .Returns(existingUser);

            _authStore.Setup(a => a.SaveUser(It.Is<User>(u => u.IsLocked == true)))
                      .Verifiable();

            var result = _authenticationService.Register(username, password, twofactor);

            _authStore.Verify();
        }

        [TestMethod]
        public void Register___when_user_account_locked___should_return_false_and_not_set_password()
        {
            var existingUser = TestData.User();
            var username = existingUser.Username;
            var twofactor = "an incorrect twofactor code";
            var password = "CorrectHorseBatteryStaple";
            existingUser.IsLocked = true;

            _authStore.Setup(a => a.GetUserByName(username))
                      .Returns(existingUser);

            _authStore.Setup(a => a.SavePasswordHash(It.IsAny<int>(), It.IsAny<byte[]>()))
                .Throws(new Exception("should not update password when account is locked"));

            var result = _authenticationService.Register(username, password, existingUser.TwoFactorCode);

            Assert.IsFalse(result);
        }
    }
}
