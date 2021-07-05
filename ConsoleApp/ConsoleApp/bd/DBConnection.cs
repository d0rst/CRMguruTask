using System.Data.SqlClient;
using System.Configuration;

namespace ConsoleApp.bd
{
    class DBConnection
    {
        private static string connectionString =
            ConfigurationManager.ConnectionStrings["worldbd"].ConnectionString;

        public static SqlConnection dbConnect()
        {
            return new SqlConnection(connectionString);
        }

    }
}
