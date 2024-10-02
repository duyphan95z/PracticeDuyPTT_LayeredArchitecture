using DuyPTT_Repositories.DbConnect;
using DuyPTT_Repositories.JWT_Token.BUS;
using DuyPTT_Repositories.JWT_Token.Interfaces;
using DuyPTT_Repositories.JWT_Token.Repos;
using DuyPTT_Repositories.User.Interfaces;
using DuyPTT_Repositories.User.Repos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DuyPTT_Repositories
{
	public static class ServiceCollectionExtensions
	{
		public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<IJWT_Token, JWT_TokenRepos>();
			services.AddSingleton<SqlServerConnect>();
			services.AddSingleton<SQLCommandsDuyPTT>();
			services.AddSingleton<IUser, UserRepos>();
		}
	}
}
