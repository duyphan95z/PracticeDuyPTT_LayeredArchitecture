using DuyPTT_Application.Features.JWT_Token.BUS;
using DuyPTT_Application.Kafka;
using DuyPTT_Integrations.User.Interface;
using DuyPTT_Integrations.User.Models;
using DuyPTT_Repositories.DbConnect;
using FluentValidation;
using MediatR;

namespace DuyPTT_Application.Features.CallApi
{
	public class CallApiRequest : GetUserInputInteg, IRequest<Response<object>>
	{
		public class QueryValidation : AbstractValidator<CallApiRequest>
		{
			// Mục 11 trong bài tập:  Validation (FluentValidation)
			public QueryValidation()
			{
				RuleFor(x => x.userName).NotNull().NotEmpty();
			}
		}
		public class QueryHandler : IRequestHandler<CallApiRequest, Response<object>>
		{
			private readonly IUserInteg _IUserInteg;
			private readonly IKafka _ikafka;
			public QueryHandler(IUserInteg iIUserInteg, IKafka ikafka)
			{
				_IUserInteg = iIUserInteg;
				_ikafka = ikafka;
			}
			public async Task<Response<object>> Handle(CallApiRequest request, CancellationToken cancellationToken)
			{
				try
				{
					//UserRsInterg
					await _ikafka.SendMessage("Started-CallApiRequest: " + JWT_TokenService.GetLocalTime());

					var rs =await _IUserInteg.CallApiGetUserName(request);

					await _ikafka.SendMessage("Stoped-CallApiRequest: " + JWT_TokenService.GetLocalTime());
					return new Response<object>(rs, null, "", "");
				}
				catch (Exception ex)
				{
					await _ikafka.SendMessage("Exception-CallApiRequest: " + ex.Message);
					throw new Exception(ex.Message);
				}
				finally
				{
					await _ikafka.SendMessage("Finally-CallApiRequest: " + JWT_TokenService.GetLocalTime());
				}
			}
		}
	}
}
