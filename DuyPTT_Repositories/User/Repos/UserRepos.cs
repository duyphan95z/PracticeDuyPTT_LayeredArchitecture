using Dapper;
using DuyPTT_Repositories.DbConnect;
using DuyPTT_Repositories.JWT_Token.BUS;
using DuyPTT_Repositories.User.Interfaces;
using DuyPTT_Repositories.User.Models;

namespace DuyPTT_Repositories.User.Repos
{
	public class UserRepos: IUser
	{
		private readonly SqlServerConnect _connect;
		private readonly SQLCommandsDuyPTT _commands;
		public UserRepos(SqlServerConnect connect, SQLCommandsDuyPTT commands)
		{
			_connect = connect;
			_commands = commands;
		}
		public async Task<IEnumerable<UserRs>> GetUserName<T>(GetUserInput input)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@username", input.userName);
			return await _connect.Get<UserRs>(_commands.StoreGetUserName, parameters).ConfigureAwait(false);
		}
		public async Task<IEnumerable<InsertUserRs>> InsertUser<T>(InsertUserInput input)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userid", input.userId);
			parameters.Add("@username", input.userName);
			parameters.Add("@pass", input.pass);
			return await _connect.Get<InsertUserRs>(_commands.StoreInsertUser, parameters).ConfigureAwait(false);
		}
	}
}
