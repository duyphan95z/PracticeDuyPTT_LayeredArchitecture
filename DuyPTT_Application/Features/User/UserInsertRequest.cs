using DuyPTT_Application.Features.JWT_Token.BUS;
using DuyPTT_Application.Kafka;
using DuyPTT_Repositories.DbConnect;
using DuyPTT_Repositories.User.Interfaces;
using DuyPTT_Repositories.User.Models;
using FluentValidation;
using MediatR;

namespace DuyPTT_Application.Features.User
{
	public class UserInsertRequest : InsertUserInput, IRequest<Response<object>>
	{
		public class QueryValidation : AbstractValidator<UserInsertRequest>
		{
			// Mục 11 trong bài tập:  Validation (FluentValidation)
			public QueryValidation()
			{
				RuleFor(x => x.userName).NotNull().NotEmpty();
			}
		}
		public class QueryHandler : IRequestHandler<UserInsertRequest, Response<object>>
		{
			private readonly IUser _iIUser;
			private readonly IKafka _ikafka;
			public QueryHandler(IUser iIUser, IKafka ikafka)
			{
				_iIUser = iIUser;
				_ikafka = ikafka;
			}
			public async Task<Response<object>> Handle(UserInsertRequest request, CancellationToken cancellationToken)
			{
				try
				{
					await _ikafka.SendMessage("Started-UserInsertRequest: " + JWT_TokenService.GetLocalTime());

					var rs = await _iIUser.InsertUser<InsertUserRs>(request);

					await _ikafka.SendMessage("Stoped-UserInsertRequest: " + JWT_TokenService.GetLocalTime());
					return new Response<object>(rs, null, "", "");
				}
				catch (Exception ex)
				{
					await _ikafka.SendMessage("Exception-UserInsertRequest: " + ex.Message);
					throw new Exception(ex.Message);
				}
				finally
				{
					await _ikafka.SendMessage("Finally-UserInsertRequest: " + JWT_TokenService.GetLocalTime());
				}
			}
		}
	}
}
