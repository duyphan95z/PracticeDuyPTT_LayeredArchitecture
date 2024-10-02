using DuyPTT_Application.Features.CallApi;
using DuyPTT_Repositories.DbConnect;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PracticeDuyPTT_Api.Controllers
{
	[ApiController]
	[ApiVersion("2.0")]
	[Route("duyptt_call/api/v{version:apiVersion}/[controller]")]
	public class CallApiController : Controller
	{
		private readonly IConfiguration _configuration;
		private readonly IMediator mediator;

		public CallApiController(IConfiguration configuration, IMediator mediator)
		{
			_configuration = configuration;
			this.mediator = mediator;
		}

		[HttpPost("Call")]
		[ProducesResponseType(typeof(Response<object>), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> CallApi([FromBody] CallApiRequest request)
		   => Ok(await mediator.Send(request).ConfigureAwait(false));
	}
}
