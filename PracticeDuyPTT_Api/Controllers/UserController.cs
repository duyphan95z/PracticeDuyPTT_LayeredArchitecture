using DuyPTT_Application.Features.User;
using DuyPTT_Repositories.DbConnect;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Net;

namespace PracticeDuyPTT_Api.Controllers
{
	[Authorize]
	[ApiController]
	[ApiVersion("2.0")]
	[Route("duyptt_user/api/v{version:apiVersion}/[controller]")]
	//[Route("duyptt_user/api/[controller]")]
	public class UserController : Controller
	{
		private readonly IConfiguration _configuration;
		private readonly IMediator mediator;

		public UserController(IConfiguration configuration, IMediator mediator)
		{
			_configuration = configuration;
			this.mediator = mediator;
		}

		[HttpPost("GetUserName")]
		[ProducesResponseType(typeof(Response<object>), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[EnableRateLimiting("fixed")]
		public async Task<IActionResult> UserNameGet([FromBody] UserNameGetRequest request)
		{
			if (User.IsInRole("READ") == true)
			{
				return Ok(await mediator.Send(request).ConfigureAwait(false));
			}
            else
            {
				return BadRequest(new Response<object>("Không có quyền READ data nghe hôm !. Liên hệ DUYPTT nhé"));
            }
        }
		[HttpPost("UserInsert")]
		[ProducesResponseType(typeof(Response<object>), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> UserInsert([FromBody] UserInsertRequest request)
		{
			if (User.IsInRole("WRITE") == true)
			{
				return Ok(await mediator.Send(request).ConfigureAwait(false));
			}
			else
			{
				return BadRequest(new Response<object>("Không có quyền WRITE data nghe hôm !. Liên hệ DUYPTT nhé"));
			}
		}
		[HttpPost("UserReadWriteJsonTxt")]
		[ProducesResponseType(typeof(Response<object>), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> UserReadWriteJsonTxt([FromBody] UserReadWriteJsonTxtRequest request)
		{
			if (User.IsInRole("WRITE") == true)
			{
				return Ok(await mediator.Send(request).ConfigureAwait(false));
			}
			else
			{
				return BadRequest(new Response<object>("Không có quyền WRITE data nghe hôm !. Liên hệ DUYPTT nhé"));
			}
		}
	}
}
