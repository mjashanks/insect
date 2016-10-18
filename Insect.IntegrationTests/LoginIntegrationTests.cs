using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Insect.Domain;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using Insect.Authentication;
using Insect.Stores;
using System.Linq;

namespace Insect.IntegrationTests
{
    [TestClass]
    public class LoginIntegrationTests
    {
        IDbConnection conn;
        private const string TestDb = "InsectTest";
        private const string TestUser = "mike";
        private const string TestPassword = "CorrectHorseBatteryStaple";

        private AuthenticationService _authService;
        private User user;
        public LoginIntegrationTests()
        {
            DbCreator.Create();
            conn = DbConnectionFactory.CreateAndOpen(DbCreator.TestInstance, TestDb);

            user = new User
            {
                IsAdministrator = true,
                Username = "mike",
                Salt = "mmmmSalty",
                PasswordExpiryDate = DateTime.Now.AddDays(14)
            };
                        
            user.Id = (int) conn.Insert(user);

            var hash = new PasswordHash
            {
                Hash = PasswordHasher.GenerateSaltedHash(TestPassword, user.Salt),
                UserId = user.Id
            };

            conn.Insert(hash);

            var config = new Config
            {
                Database = TestDb,
                Server = DbCreator.TestInstance
            };
            var authStore = new AuthStore(config);
            _authService = new AuthenticationService(authStore);
        }

        [TestMethod]
        public void Login___when_details_correct___should_create_session()
        {
            var sessionId = _authService.Login(TestUser, TestPassword);

            Assert.IsNotNull(sessionId);
            Assert.IsTrue(sessionId != Guid.Empty);

            var session = conn.GetAll<Session>()
                              .FirstOrDefault(s => s.SessionId == sessionId);

            Assert.IsNotNull(session);
            Assert.IsTrue(session.UserId == user.Id);
        }
    }
}
