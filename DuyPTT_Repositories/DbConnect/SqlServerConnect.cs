using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DuyPTT_Repositories.DbConnect
{
	public class SqlServerConnect
	{
		private readonly IConfiguration _configuration;
		public SqlServerConnect(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		private IDbConnection CreateConnection()
		{
			var connectionString = _configuration.GetConnectionString("SqlServer");
			return new SqlConnection(connectionString);
		}
		public async Task<IEnumerable<T>> Get<T>(string store, object parameters)
		{
			using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("SqlServer")))
			{
				try
				{
					dbConnection.Open();
					return await dbConnection.QueryAsync<T>(store, parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
				}
				finally
				{
					dbConnection.Close();
				}
			}
		}
		public async Task<int> Execute(string store, object parameters)
		{
			using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("SqlServer")))
			{
				try
				{
					dbConnection.Open();
					return await dbConnection.ExecuteAsync(store, parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
				}
				finally
				{
					dbConnection.Close();
				}
			}
		}
	}
}
