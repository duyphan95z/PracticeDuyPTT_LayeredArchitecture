using DuyPTT_Application.Kafka;
using DuyPTT_Repositories.DbConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DuyPTT_Application
{
	public static class ServiceCollectionExtensions
	{
		public static void AddApplication(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddSingleton<Response<object>>();
			services.AddSingleton<IKafka, KafkaProducer>();
		}
	}
}
