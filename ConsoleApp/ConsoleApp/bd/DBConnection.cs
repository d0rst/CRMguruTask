using System.Data.SqlClient;

namespace ConsoleApp.bd
{
    class DBConnection
    {
        private static string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=worldbd;Integrated Security=True";

        public static SqlConnection dbConnect()
        {
            return new SqlConnection(connectionString);
        }

    }
}
