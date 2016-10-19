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
        private const string TestTwoFactorCode = "two_factor_code";

        private AuthenticationService _authService;
        private int userId;
        public LoginIntegrationTests()
        {
            DbCreator.Create();
            conn = DbConnectionFactory.CreateAndOpen(DbCreator.TestInstance, TestDb);

            var config = new Config
            {
                Database = TestDb,
                Server = DbCreator.TestInstance
            };

            userId = DbCreator.CreateUser(config, TestUser, TestTwoFactorCode);

            var authStore = new AuthStore(config);
            _authService = new AuthenticationService(authStore);
        }

        [TestMethod]
        public void Register_then_login___Should_create_session()
        {
            var verifySuccess = _authService.Verify(TestUser, TestPassword, TestTwoFactorCode);

            Assert.IsTrue(verifySuccess);

            var sessionId = _authService.Login(TestUser, TestPassword);

            Assert.IsNotNull(sessionId);
            Assert.IsTrue(sessionId != Guid.Empty);

            var session = conn.GetAll<Session>()
                              .FirstOrDefault(s => s.SessionId == sessionId);

            Assert.IsNotNull(session);
            Assert.IsTrue(session.UserId == userId);
        }
    }
}
