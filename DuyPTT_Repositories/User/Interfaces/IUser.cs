using DuyPTT_Repositories.User.Models;

namespace DuyPTT_Repositories.User.Interfaces
{
	public interface IUser
	{
		public Task<IEnumerable<UserRs>> GetUserName<T>(GetUserInput input);
		public Task<IEnumerable<InsertUserRs>> InsertUser<T>(InsertUserInput input);
	}
}
