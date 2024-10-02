using Dapper;
using DuyPTT_Repositories.DbConnect;
using DuyPTT_Repositories.JWT_Token.BUS;
using DuyPTT_Repositories.JWT_Token.Interfaces;
using DuyPTT_Repositories.JWT_Token.Models;

namespace DuyPTT_Repositories.JWT_Token.Repos
{
	public class JWT_TokenRepos: IJWT_Token
	{
		private readonly SqlServerConnect _connect;
		private readonly SQLCommandsDuyPTT _commands;
		public JWT_TokenRepos(SqlServerConnect connect, SQLCommandsDuyPTT commands)
		{
			_connect = connect;
			_commands = commands;
		}

		public async Task<IEnumerable<ValidateUserRs>> GetUser<T>(JWT_TokenInput input)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@username", input.username);
			parameters.Add("@pass", input.password);
			return await _connect.Get<ValidateUserRs>(_commands.StoreGetUser, parameters).ConfigureAwait(false);
		}
		public async Task<IEnumerable<ValidateRoleUserRs>> GetRoleUser<T>(string userid)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userid", userid);
			return await _connect.Get<ValidateRoleUserRs>(_commands.StoreGetRoleUser, parameters).ConfigureAwait(false);
		}
	}
}
