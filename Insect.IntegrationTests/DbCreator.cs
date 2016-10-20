using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib;
using Insect.IntegrationTests.Properties;
using Insect.Domain;
using System.Data;

namespace Insect.IntegrationTests
{
    public class DbCreator
    {
        public const string TestInstance = "localhost\\bluezinc";
        public static void Create()
        {
            var connection = DbConnectionFactory.CreateAndOpen(TestInstance, "master");

            try
            {
                connection.Execute(Resources.SafeDropTestDb);
            }
            catch { }

            connection.Execute("Create database InsectTest");

            connection.Execute(Resources.DropAndCreateDb);
        }

        public static int CreateUser(Config config, string username, string twofactorcode, string emailVerificationPath)
        {
            using(var connection = DbConnectionFactory.CreateAndOpen(config.Server, config.Database))
                return connection.Execute("insert into users (username, twofactorcode, isverified, VerificationExpiryDate, EmailVerificationPath) values (@username, @twofac, 0, @verifyDate, @verifyPath)",
                        new { 
                            username = username, 
                            twofac = twofactorcode, 
                            verifyDate = DateTime.Now.AddDays(1),
                            verifyPath = emailVerificationPath});
        }
    }
}
