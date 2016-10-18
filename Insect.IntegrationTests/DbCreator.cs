using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib;
using Insect.IntegrationTests.Properties;
using Insect.Domain;

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
    }
}
