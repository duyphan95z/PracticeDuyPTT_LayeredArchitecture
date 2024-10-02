using DuyPTT_Integrations.User.Interface;
using DuyPTT_Integrations.User.Repos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DuyPTT_Integrations
{
	public static class ServiceCollectionExtensions
	{
		public static void AddIntegration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<IUserInteg, UserIntegRepos>();
			services.AddHttpClient();
		}
	}
}
