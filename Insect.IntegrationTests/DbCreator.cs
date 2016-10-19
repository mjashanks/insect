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

        public static void CreateUser(Config config, string username, string twofactorcode)
        {
            using(var connection = DbConnectionFactory.CreateAndOpen(config.Server, config.Database))
                connection.Execute("insert into users (username, twofactorcode) values (@username, @twofac)", new { username = username, twofac = twofactorcode });
        }
    }
}
