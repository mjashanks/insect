using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nant.Authentication;
using Moq;
using nant.Stores;
using nant.Domain;

namespace Nant.Tests.AuthenticationTests
{
    [TestClass]
    public class LoginTests
    {
        private AuthenticationService _authenticationService;
        private Mock<IAuthStore> _authStore;
 
        public LoginTests()
        {
            _authStore = new Mock<IAuthStore>();
            _authenticationService = new AuthenticationService(_authStore.Object);
        }

        [TestMethod]
        public void Login___should_return_sessionId_from_store___when_details_correct()
        {
            var password = "CorrectHorseBatteryStaple";
            var user = TestData.CreateUser();
            user.Username = "mjashanks@hotmail.com";
            var hash = PasswordHasher.GenerateSaltedHash(password, user.Salt);
            var expectedSessionId = Guid.NewGuid();

            _authStore.Setup(a => a.GetUserByName(user.Username))
                      .Returns(user)
                      .Verifiable();

            _authStore.Setup(a => a.GetPasswordHash(user.Id))
                      .Returns(hash)
                      .Verifiable();

            _authStore.Setup(a => a.CreateNewSession(user.Id))
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
            var user = TestData.CreateUser();
            user.Username = "mjashanks@hotmail.com";
            var hash = PasswordHasher.GenerateSaltedHash(password, user.Salt);
            var expectedSessionId = Guid.NewGuid();

            _authStore.Setup(a => a.GetUserByName("an_incorrect_username"))
                      .Returns<User>(null);

            _authStore.Setup(a => a.GetPasswordHash(It.IsAny<Guid>()))
                      .Throws(new Exception("should not try and fetch password hash"));

            _authStore.Setup(a => a.CreateNewSession(user.Id))
                      .Throws(new Exception("should not create session"));

            var actualSessionId = _authenticationService.Login(user.Username, password);

            Assert.AreEqual(null, actualSessionId);

            _authStore.Verify();
        }

        [TestMethod]
        public void Login___should_return_null___when_password_is_incorrect()
        {
            var password = "CorrectHorseBatteryStaple";
            var user = TestData.CreateUser();
            user.Username = "mjashanks@hotmail.com";
            var hash = PasswordHasher.GenerateSaltedHash(password, user.Salt);
            var storedHash = PasswordHasher.GenerateSaltedHash("an_incorrect_password", user.Salt);

            var expectedSessionId = Guid.NewGuid();
            
            _authStore.Setup(a => a.GetUserByName(user.Username))
                      .Returns(user);

            _authStore.Setup(a => a.GetPasswordHash(user.Id))
                      .Returns(storedHash)
                      .Verifiable();

            _authStore.Setup(a => a.CreateNewSession(user.Id))
                      .Throws(new Exception("should not create session"));

            var actualSessionId = _authenticationService.Login(user.Username, password);

            Assert.AreEqual(null, actualSessionId);

            _authStore.Verify();
        }

        [TestMethod]
        public void Login___should_increment_failed_count___when_password_is_incorrect()
        {
            var password = "CorrectHorseBatteryStaple";
            var user = TestData.CreateUser();
            user.Username = "mjashanks@hotmail.com";
            var hash = PasswordHasher.GenerateSaltedHash(password, user.Salt);
            var storedHash = PasswordHasher.GenerateSaltedHash("an_incorrect_password", user.Salt);

            var expectedSessionId = Guid.NewGuid();

            _authStore.Setup(a => a.GetUserByName(user.Username))
                      .Returns(user);

            _authStore.Setup(a => a.GetPasswordHash(user.Id))
                      .Returns(storedHash)
                      .Verifiable();

            _authStore.Setup(a => a.CreateNewSession(user.Id))
                      .Throws(new Exception("should not create session"));

            var actualSessionId = _authenticationService.Login(user.Username, password);

            Assert.AreEqual(null, actualSessionId);

            _authStore.Verify();
        }

    }
}
