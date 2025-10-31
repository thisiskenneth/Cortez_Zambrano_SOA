using System.Configuration;
using System.Data.SqlClient;

namespace FerreteriaSoap.Repositories
{
    public static class SqlConnectionFactory
    {
        public static SqlConnection Create()
        {
            var cs = ConfigurationManager.ConnectionStrings["FerreteriaDb"].ConnectionString;
            return new SqlConnection(cs);
        }
    }
}
