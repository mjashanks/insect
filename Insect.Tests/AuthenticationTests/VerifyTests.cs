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
    public class VerifyTests : AuthenticationTestBase
    {
        [TestMethod]
        public void Verify___when_valid___should_update_user_with_initial_details()
        {
            var existingUser = TestData.User();
            var emailVerify = existingUser.EmailVerificationPath;
            var twofactor = existingUser.TwoFactorCode;
            var password = "CorrectHorseBatteryStaple";

            _authStore.Setup(a => a.GetUserByEmailVerificationPath(emailVerify))
                      .Returns(existingUser);
            
            _authStore.Setup(a => 
                a.SaveUser(It.Is<User>(u =>
                    u.FailedLoginCount == 0
                    && u.IsLocked == false
                    && u.IsAdministrator == false
                )))
                .Verifiable();
            
            var result = _authenticationService.Verify(emailVerify, password, existingUser.TwoFactorCode);
                       
            Assert.IsTrue(result);
            _authStore.Verify();
        }

        [TestMethod]
        public void Verify___when_valid___should_store_hashed_password_and_generate_salt()
        {
            var existingUser = TestData.User();
            var emailVerify = existingUser.EmailVerificationPath;
            var twofactor = existingUser.TwoFactorCode;
            var password = "CorrectHorseBatteryStaple";

            existingUser.Salt = "";

            _authStore.Setup(a => a.GetUserByEmailVerificationPath(emailVerify))
                      .Returns(existingUser);

            _authStore.Setup(a =>
                a.SaveUser(It.Is<User>(u => u.Salt.Length > 0)))
                .Verifiable();

            // ned to get this lazily, as salt is changed by service...
            Func<byte[]> expectedHash = () => PasswordHasher.GenerateSaltedHash(password, existingUser.Salt);
            
            _authStore.Setup(a => 
                a.SavePasswordHash(existingUser.Id, It.Is<byte[]>(h => PasswordHasher.CompareByteArrays(expectedHash(), h)))
                ).Verifiable();
                      

            var result = _authenticationService.Verify(emailVerify, password, twofactor);

            Assert.IsTrue(result);
            _authStore.Verify();
        }

        [TestMethod]
        public void Verify___when_incorrect_verification_path___should_return_false()
        {
            var emailVerify = "this_url_does_note_exist";
            _authStore.Setup(a => a.GetUserByEmailVerificationPath(emailVerify))
                      .Returns<User>(null);

            var result = _authenticationService.Verify(emailVerify, "password", "tf");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Verify___when_twofactor_code_incorrect___should_return_false_and_not_set_password()
        {
            var existingUser = TestData.User();
            var emailVerify = existingUser.EmailVerificationPath;
            var twofactor = "an incorrect twofactor code";
            var password = "CorrectHorseBatteryStaple";

            _authStore.Setup(a => a.GetUserByEmailVerificationPath(emailVerify))
                      .Returns(existingUser);

            _authStore.Setup(a => a.SavePasswordHash(It.IsAny<int>(), It.IsAny<byte[]>()))
                .Throws(new Exception("should not update password when two factor is incorrect"));

            var result = _authenticationService.Verify(emailVerify, password, twofactor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Verify___when_twofactor_code_incorrect___should_increment_failedcount()
        {
            var existingUser = TestData.User();
            var emailVerify = existingUser.EmailVerificationPath;
            var twofactor = "an incorrect twofactor code";
            var password = "CorrectHorseBatteryStaple";
            existingUser.FailedLoginCount = 0;

            _authStore.Setup(a => a.GetUserByEmailVerificationPath(emailVerify))
                      .Returns(existingUser);

            _authStore.Setup(a => a.SaveUser(It.Is<User>(u => u.FailedLoginCount == 1)))
                      .Verifiable();

            var result = _authenticationService.Verify(emailVerify, password, twofactor);

            _authStore.Verify();
        }

        [TestMethod]
        public void Verify___when_twofactor_code_incorrect_for_third_time___should_lock_user()
        {
            var existingUser = TestData.User();
            var emailVerify = existingUser.EmailVerificationPath;
            var twofactor = "an incorrect twofactor code";
            var password = "CorrectHorseBatteryStaple";
            existingUser.FailedLoginCount = 2;

            _authStore.Setup(a => a.GetUserByEmailVerificationPath(emailVerify))
                      .Returns(existingUser);

            _authStore.Setup(a => a.SaveUser(It.Is<User>(u => u.IsLocked == true)))
                      .Verifiable();

            var result = _authenticationService.Verify(emailVerify, password, twofactor);

            _authStore.Verify();
        }

        [TestMethod]
        public void Verify___when_user_account_locked___should_return_false_and_not_set_password()
        {
            var existingUser = TestData.User();
            var emailVerify = existingUser.EmailVerificationPath;
            var twofactor = "an incorrect twofactor code";
            var password = "CorrectHorseBatteryStaple";
            existingUser.IsLocked = true;

            _authStore.Setup(a => a.GetUserByEmailVerificationPath(emailVerify))
                      .Returns(existingUser);

            _authStore.Setup(a => a.SavePasswordHash(It.IsAny<int>(), It.IsAny<byte[]>()))
                .Throws(new Exception("should not update password when account is locked"));

            var result = _authenticationService.Verify(emailVerify, password, twofactor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Verify___when_user_already_verified___should_return_false_and_not_set_password()
        {
            var existingUser = TestData.User();
            var emailVerify = existingUser.EmailVerificationPath;
            existingUser.IsVerified = true;
            existingUser.TwoFactorCode = "twofactor";
            _authStore.Setup(a => a.GetUserByEmailVerificationPath(emailVerify))
                      .Returns(existingUser);

            _authStore.Setup(a => a.SavePasswordHash(It.IsAny<int>(), It.IsAny<byte[]>()))
                .Throws(new Exception("should not update password when account is already verified"));

            var result = _authenticationService.Verify(emailVerify, "whatever", existingUser.TwoFactorCode);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Verify___when_verification_date_has_passed___should_return_false_and_not_set_password()
        {
            var existingUser = TestData.User();
            var emailVerify = existingUser.EmailVerificationPath;
            existingUser.IsVerified = false;
            existingUser.VerificationExpiryDate = DateTime.Now.AddDays(-1);
            existingUser.TwoFactorCode = "twofactor";

            _authStore.Setup(a => a.GetUserByEmailVerificationPath(emailVerify))
                      .Returns(existingUser);

            _authStore.Setup(a => a.SavePasswordHash(It.IsAny<int>(), It.IsAny<byte[]>()))
                .Throws(new Exception("should not update password when account pas verification date"));

            var result = _authenticationService.Verify(emailVerify, "whatever", existingUser.TwoFactorCode);

            Assert.IsFalse(result);
        }

    }
}
