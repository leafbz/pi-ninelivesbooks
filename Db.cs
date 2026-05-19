using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xdd
{
    public static class Db
    {
        private static readonly string connectionString =
            "datasource=localhost; username=root; password=; database=ninelivebooks";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}

