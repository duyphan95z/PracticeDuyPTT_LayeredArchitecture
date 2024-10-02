using DuyPTT_Application.Features.JWT_Token.BUS;
using DuyPTT_Repositories.DbConnect;
using DuyPTT_Repositories.JWT_Token.Interfaces;
using MediatR;

namespace DuyPTT_Application.Kafka
{
	public class KafkaCheckConsumeRequest: IRequest<Response<object>>
	{
		public class QueryHandler : IRequestHandler<KafkaCheckConsumeRequest, Response<object>>
		{
			private readonly IKafka _ikafka;
			public QueryHandler(IJWT_Token iWT_Token, IKafka ikafka)
			{
				_ikafka = ikafka;
			}
			public async Task<Response<object>> Handle(KafkaCheckConsumeRequest request, CancellationToken cancellationToken)
			{
				try
				{
					CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
					_ikafka.ConsumeMessages();
					return new Response<object>(null, null, "", "");
				}
				catch (Exception ex)
				{
					await _ikafka.SendMessage("Exception-KafkaCheckConsumeRequest: " + ex.Message);
					throw new Exception(ex.Message);
				}
				finally
				{
					await _ikafka.SendMessage("Finally-KafkaCheckConsumeRequest: " + JWT_TokenService.GetLocalTime());
				}
			}
		}
	}
}
