using DuyPTT_Repositories.User.Interfaces;
using DuyPTT_Repositories.User.Models;
using System.Runtime.Caching;

namespace DuyPTT_Application.Features.User.BUS
{
	public class MemoryCacheUser
	{
		public static async Task<IEnumerable<UserRs>> MemoryInfoUserName(UserNameGetRequest request, IUser _iIUser)
		{
			ObjectCache cache = MemoryCache.Default;
			string cacheKey = "UserNameGetRequest" + request.userName;
			CacheItemPolicy policy = new CacheItemPolicy
			{
				AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10)
			};
			IEnumerable<UserRs>? rs = cache.Get(cacheKey) as IEnumerable<UserRs>;
			if (rs is null)
			{
				rs = await _iIUser.GetUserName<UserRs>(request);
				cache.Set(cacheKey, rs, policy);
			}
			return rs;
		}
	}
}
