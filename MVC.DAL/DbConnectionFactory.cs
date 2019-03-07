using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DAL
{
    public class DbConnectionFactory : IConnectionFactory
    {
        private readonly DbProviderFactory _provider;
        private readonly string _connectionString;
        private readonly string _name;

        public DbConnectionFactory(string connectionName)
        {
            if (connectionName == null) throw new ArgumentNullException("connectionName");

            var conStr = ConfigurationManager.ConnectionStrings[connectionName];
            if (conStr == null)
                throw new ConfigurationErrorsException(string.Format("Failed to find connection string named '{0}' in app/web.config.", connectionName));

            //_name = conStr.ProviderName;
            //_provider = DbProviderFactories.GetFactory(conStr.ProviderName);

            string entityConnectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            _name = ConfigurationManager.ConnectionStrings[connectionName].ProviderName;
            _provider = DbProviderFactories.GetFactory(_name);

            _connectionString = entityConnectionString;
            
            //_connectionString = string.Format(conStr.ConnectionString + "User Id={0};Password={1};", username, password);

        }

        public IDbConnection Create()
        {
            var connection = _provider.CreateConnection();
            if (connection == null)
                throw new ConfigurationErrorsException(string.Format("Failed to create a connection using the connection string named '{0}' in app/web.config.", _name));

            connection.ConnectionString = _connectionString;
            connection.Open();
            return connection;
        }
    }
}
