using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Insect.Authentication;
using Moq;
using Insect.Stores;
using Insect.Domain;

namespace Insect.Tests.AuthenticationTests
{
    [TestClass]
    public class LoginTests : AuthenticationTestBase
    {       

        [TestMethod]
        public void Login___should_return_sessionId_from_store___when_details_correct()
        {
            var password = "CorrectHorseBatteryStaple";
            var user = TestData.User();
            user.Username = "mjashanks@hotmail.com";
            var hash = PasswordHasher.GenerateSaltedHash(password, user.Salt);
            var expectedSessionId = Guid.NewGuid();

            _authStore.Setup(a => a.GetUserByName(user.Username))
                      .Returns(user)
                      .Verifiable();

            _authStore.Setup(a => a.GetPasswordHash(user.Id))
                      .Returns(hash)
                      .Verifiable();

            _authStore.Setup(a => a.CreateNewSession(user.Id, user.UserLevel))
                      .Returns(expectedSessionId)
                      .Verifiable();

            var actualSessionId = _authenticationService.Login(user.Username, password);

            Assert.AreEqual(expectedSessionId, actualSessionId);

            _authStore.VerifyAll();
        }

        [TestMethod]
        public void Login___should_return_null___when_username_does_not_exist()
        {
            var password = "CorrectHorseBatteryStaple";
            var user = TestData.User();
            user.Username = "mjashanks@hotmail.com";
            var hash = PasswordHasher.GenerateSaltedHash(password, user.Salt);
            var expectedSessionId = Guid.NewGuid();

            _authStore.Setup(a => a.GetUserByName("an_incorrect_username"))
                      .Returns<User>(null);

            _authStore.Setup(a => a.GetPasswordHash(It.IsAny<int>()))
                      .Throws(new Exception("should not try and fetch password hash"));

            _authStore.Setup(a => a.CreateNewSession(user.Id, user.UserLevel))
                      .Throws(new Exception("should not create session"));

            var actualSessionId = _authenticationService.Login(user.Username, password);

            Assert.AreEqual(null, actualSessionId);

            _authStore.Verify();
        }

        [TestMethod]
        public void Login___should_return_null___when_password_is_incorrect()
        {
            var password = "CorrectHorseBatteryStaple";
            var user = TestData.User();
            user.Username = "mjashanks@hotmail.com";
            var hash = PasswordHasher.GenerateSaltedHash(password, user.Salt);
            var storedHash = PasswordHasher.GenerateSaltedHash("an_incorrect_password", user.Salt);

            var expectedSessionId = Guid.NewGuid();
            
            _authStore.Setup(a => a.GetUserByName(user.Username))
                      .Returns(user);

            _authStore.Setup(a => a.GetPasswordHash(user.Id))
                      .Returns(storedHash)
                      .Verifiable();

            _authStore.Setup(a => a.CreateNewSession(user.Id, user.UserLevel))
                      .Throws(new Exception("should not create session"));

            var actualSessionId = _authenticationService.Login(user.Username, password);

            Assert.AreEqual(null, actualSessionId);

            _authStore.Verify();
        }

        [TestMethod]
        public void Login___should_increment_failed_count___when_password_is_incorrect()
        {
            var password = "CorrectHorseBatteryStaple";
            var user = TestData.User();
            user.Username = "mjashanks@hotmail.com";
            var hash = PasswordHasher.GenerateSaltedHash(password, user.Salt);
            var storedHash = PasswordHasher.GenerateSaltedHash("an_incorrect_password", user.Salt);

            var expectedSessionId = Guid.NewGuid();

            user.FailedLoginCount = 0;

            _authStore.Setup(a => a.GetUserByName(user.Username))
                      .Returns(user);

            _authStore.Setup(a => a.GetPasswordHash(user.Id))
                      .Returns(storedHash)
                      .Verifiable();

            _authStore.Setup(a => a.SaveUser(It.Is<User>(s => s.FailedLoginCount == 1)))
                      .Verifiable();

             _authenticationService.Login(user.Username, password);

            _authStore.Verify();
        }

        [TestMethod]
        public void Login___should_lock_user___when_password_is_incorrect_for_the_third_time()
        {
            var password = "CorrectHorseBatteryStaple";
            var user = TestData.User();
            user.Username = "mjashanks@hotmail.com";
            var hash = PasswordHasher.GenerateSaltedHash(password, user.Salt);
            var storedHash = PasswordHasher.GenerateSaltedHash("an_incorrect_password", user.Salt);

            var expectedSessionId = Guid.NewGuid();

            user.FailedLoginCount = 2;
            user.IsLocked = false;

            _authStore.Setup(a => a.GetUserByName(user.Username))
                      .Returns(user);

            _authStore.Setup(a => a.GetPasswordHash(user.Id))
                      .Returns(storedHash)
                      .Verifiable();

            _authStore.Setup(a => a.SaveUser(It.Is<User>(s => s.FailedLoginCount == 3 && s.IsLocked == true)))
                      .Verifiable();

            _authenticationService.Login(user.Username, password);

            _authStore.Verify();
        }

        [TestMethod]
        public void Login___should_return_null___when_user_record_islocked()
        {
            var password = "CorrectHorseBatteryStaple";
            var user = TestData.User();
            user.Username = "mjashanks@hotmail.com";

            user.IsLocked = true;

            _authStore.Setup(a => a.GetUserByName(user.Username))
                      .Returns(user);

            _authStore.Setup(a => a.GetPasswordHash(user.Id))
                      .Throws(new Exception("should never fetch the hash when user is locked"));


            var sessionId = _authenticationService.Login(user.Username, password);

            Assert.IsNull(sessionId);            
        }

        [TestMethod]
        public void Login___should_reset_failed_count___when_details_correct_and_fail_count_is_greaterthan_zero()
        {
            var password = "CorrectHorseBatteryStaple";
            var user = TestData.User();
            user.Username = "mjashanks@hotmail.com";
            var hash = PasswordHasher.GenerateSaltedHash(password, user.Salt);
            var expectedSessionId = Guid.NewGuid();

            user.FailedLoginCount = 1;

            _authStore.Setup(a => a.GetUserByName(user.Username))
                      .Returns(user)
                      .Verifiable();

            _authStore.Setup(a => a.GetPasswordHash(user.Id))
                      .Returns(hash)
                      .Verifiable();

            _authStore.Setup(a => a.CreateNewSession(user.Id, user.UserLevel))
                      .Returns(expectedSessionId)
                      .Verifiable();

            _authStore.Setup(a => a.SaveUser(It.Is<User>(u => u.FailedLoginCount == 0)))
                      .Verifiable();

            var actualSessionId = _authenticationService.Login(user.Username, password);

            Assert.AreEqual(expectedSessionId, actualSessionId);

            _authStore.VerifyAll();
        }
    }
}
