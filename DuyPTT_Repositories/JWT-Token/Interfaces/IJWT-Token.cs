using DuyPTT_Repositories.JWT_Token.Models;

namespace DuyPTT_Repositories.JWT_Token.Interfaces
{
	public interface IJWT_Token
	{
		public  Task<IEnumerable<ValidateUserRs>> GetUser<T>(JWT_TokenInput input);
		public Task<IEnumerable<ValidateRoleUserRs>> GetRoleUser<T>(string userid);
	}
}
