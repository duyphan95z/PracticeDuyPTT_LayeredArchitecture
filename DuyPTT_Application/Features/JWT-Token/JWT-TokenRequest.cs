using DuyPTT_Application.Features.JWT_Token.BUS;
using DuyPTT_Application.Kafka;
using DuyPTT_Repositories.DbConnect;
using DuyPTT_Repositories.JWT_Token.Interfaces;
using DuyPTT_Repositories.JWT_Token.Models;
using FluentValidation;
using MediatR;

namespace DuyPTT_Application.Features.JWT_Token
{
	public class JWT_TokenRequest: JWT_TokenInput, IRequest<Response<object>>
	{
		public class QueryValidation : AbstractValidator<JWT_TokenRequest>
		{
			// Mục 11 trong bài tập:  Validation (FluentValidation)
			public QueryValidation()
			{
				RuleFor(x => x.username).NotNull().NotEmpty();
				RuleFor(x => x.password).NotNull().NotEmpty();
			}
		}
		public class QueryHandler : IRequestHandler<JWT_TokenRequest, Response<object>>
		{
			private readonly IJWT_Token _iWT_Token;
			private readonly IKafka _ikafka;
			public QueryHandler(IJWT_Token iWT_Token, IKafka ikafka)
			{
				_iWT_Token = iWT_Token;
				_ikafka= ikafka;
			}
			public async Task<Response<object>> Handle(JWT_TokenRequest request, CancellationToken cancellationToken)
			{
				try
				{
					// Mục 1,2,5,6 trong bài tập: Authentication JWT, Authorization, Response object , Kết nối CSDL 
					await _ikafka.SendMessage("Started-JWT_TokenRequest: " + JWT_TokenService.GetLocalTime());
					string? Secretkey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY_DuyPTT", EnvironmentVariableTarget.Machine);
					if (string.IsNullOrEmpty(Secretkey) != true)
					{
						var Role = await JWT_ValidateUserService.AuthorUser(request, _iWT_Token);
						if (Role != null)
						{
							var tokenauth = JWT_TokenService.GenerateJwtToken(request.username, request.password, Secretkey,Role);
							object rsToken = new
							{
								User = request.username,
								JwtToken = tokenauth,
								Roles = Role,
								Expires = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SE Asia Standard Time").AddHours(1).ToString("dd-MM-yyyy HH:mm:ss")
							};
							await _ikafka.SendMessage("Stoped-JWT_TokenRequest: " + JWT_TokenService.GetLocalTime());
							return new Response<object>(rsToken, null, "", "");
						}
						else
						{
							return new Response<object>("Tài khoản xác thực không tồn tại.");
						}
					}
					else
					{
						return new Response<object>("Secretkey không tồn tại");
					}

				}
				catch (Exception ex)
				{
					await _ikafka.SendMessage("Exception-JWT_TokenRequest: " + ex.Message);
					throw new Exception(ex.Message);
				}
				finally
				{
					await _ikafka.SendMessage("Finally-JWT_TokenRequest: " + JWT_TokenService.GetLocalTime());
				}
			}
		}
	}
}
