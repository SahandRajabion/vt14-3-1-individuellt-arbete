using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Individuella.Model.DAL
{
    public abstract class DALBase
    {
        private static string _connectionString;

        protected static SqlConnection CreateConnection()
        {
            // Returnerar en referens till anslutnings-objekt.
            return new SqlConnection(_connectionString);
        }

        static DALBase()
        {
            // Hämtar anslutningssträngen från web.config filen för en anslutning.
            _connectionString = WebConfigurationManager.ConnectionStrings["UD13_sr222hn_ProjektConnectionString"].ConnectionString;
        }
    }
}