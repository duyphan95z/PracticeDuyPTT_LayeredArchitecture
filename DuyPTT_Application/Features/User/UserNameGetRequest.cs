using DuyPTT_Application.Features.JWT_Token.BUS;
using DuyPTT_Application.Features.User.BUS;
using DuyPTT_Application.Kafka;
using DuyPTT_Repositories.DbConnect;
using DuyPTT_Repositories.User.Interfaces;
using DuyPTT_Repositories.User.Models;
using FluentValidation;
using MediatR;

namespace DuyPTT_Application.Features.User
{
	public class UserNameGetRequest : GetUserInput, IRequest<Response<object>>
	{
		public class QueryValidation : AbstractValidator<UserNameGetRequest>
		{
			// Mục 11 trong bài tập:  Validation (FluentValidation)
			public QueryValidation()
			{
				RuleFor(x => x.userName).NotNull().NotEmpty();
			}
		}
		public class QueryHandler : IRequestHandler<UserNameGetRequest, Response<object>>
		{
			private readonly IUser _iIUser;
			private readonly IKafka _ikafka;
			public QueryHandler(IUser iIUser, IKafka ikafka)
			{
				_iIUser = iIUser;
				_ikafka = ikafka;
			}
			public async Task<Response<object>> Handle(UserNameGetRequest request, CancellationToken cancellationToken)
			{
				try
				{
					await _ikafka.SendMessage("Started-UserNameGetRequest: " + JWT_TokenService.GetLocalTime());
					// Mục 13 trong bài tập: Caching
					var rs = await MemoryCacheUser.MemoryInfoUserName(request, _iIUser);

					await _ikafka.SendMessage("Stoped-UserNameGetRequest: " + JWT_TokenService.GetLocalTime());
					return new Response<object>(rs, null, "", "");
				}
				catch (Exception ex)
				{
					await _ikafka.SendMessage("Exception-UserNameGetRequest: " + ex.Message);
					throw new Exception(ex.Message);
				}
				finally
				{
					await _ikafka.SendMessage("Finally-UserNameGetRequest: " + JWT_TokenService.GetLocalTime());
				}
			}
		}
	}
}
