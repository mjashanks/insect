using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect
{
    public class DbConnectionFactory
    {
        public static IDbConnection CreateAndOpen(string instance, string db)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = instance;
            builder.InitialCatalog = db;
            builder.IntegratedSecurity = true;
            var conn = new SqlConnection(builder.ToString());

            conn.Open();

            return conn;
        }
    }
}
