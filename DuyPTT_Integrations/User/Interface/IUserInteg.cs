using DuyPTT_Integrations.User.Models;

namespace DuyPTT_Integrations.User.Interface
{
	public interface IUserInteg
	{
		public Task<string> CallApiGetUserName(GetUserInputInteg input);
	}
}
