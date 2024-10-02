using DuyPTT_Application.Features.JWT_Token;
using DuyPTT_Application.Kafka;
using DuyPTT_Repositories.DbConnect;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PracticeDuyPTT_Api.Controllers
{
	[Route("duyptt_auth/api/[controller]")]
	public class AuthController : Controller
	{
		private readonly IConfiguration _configuration;
		private readonly IMediator mediator;

		public AuthController(IConfiguration configuration, IMediator mediator)
		{
			_configuration = configuration;
			this.mediator = mediator;
		}

		[HttpPost("Login")]
		[ProducesResponseType(typeof(Response<object>), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> JWT_Token([FromBody] JWT_TokenRequest request)
		   => Ok(await mediator.Send(request).ConfigureAwait(false));

		[HttpPost("ExceptionGlobal")]
		public IActionResult CauseError()
		{
			throw new Exception("Xử lý exception global.");
		}
		[HttpPost("CheckConsume")]
		[ProducesResponseType(typeof(Response<object>), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> KafkaCheckConsume([FromBody] KafkaCheckConsumeRequest request)
		   => Ok(await mediator.Send(request).ConfigureAwait(false));
	}
}
