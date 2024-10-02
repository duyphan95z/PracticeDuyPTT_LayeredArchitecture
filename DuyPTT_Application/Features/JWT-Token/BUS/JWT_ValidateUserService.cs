using DuyPTT_Repositories.JWT_Token.Interfaces;
using DuyPTT_Repositories.JWT_Token.Models;

namespace DuyPTT_Application.Features.JWT_Token.BUS
{
	public class JWT_ValidateUserService
	{
		public static async Task<IEnumerable<ValidateRoleUserRs>>  AuthorUser(JWT_TokenRequest request, IJWT_Token _iWT_Token)
		{
			var RsUser = await _iWT_Token.GetUser<ValidateUserRs>(request);
			if (RsUser != null && RsUser.Count() > 0)
			{
				return await _iWT_Token.GetRoleUser<ValidateRoleUserRs>(RsUser.FirstOrDefault().USERID);
			}
			else
			{
				return null;
			}
        }
	}
}
