using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Mimo.Framework.Data
{
    /// <summary>
    /// Summary description for DataAccessFactory.
    /// </summary>
    public class DataAccessFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionKey"></param>
        /// <returns></returns>
        public static IDbConnection CreateConnection(string connectionKey)
        {
            IDbConnection connection = new SqlConnection();

            if (ConfigurationManager.ConnectionStrings[connectionKey] != null)
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings[connectionKey].ConnectionString;
            }

            return connection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IDbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IDbDataParameter CreateDataParameter()
        {
            return new SqlParameter();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static object CreateCommandBuilder()
        {
            return new SqlCommandBuilder();
        }
    }
}