using DuyPTT_Repositories.JWT_Token.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DuyPTT_Application.Features.JWT_Token.BUS
{
	public class JWT_TokenService
	{
		public static string GenerateJwtToken(string userId, string username, string secretKey, IEnumerable<ValidateRoleUserRs> roles)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, userId),
				new Claim(JwtRegisteredClaimNames.UniqueName, username)
			};

			// Thêm vai trò vào claims
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role.ROLENAME));
			}

			var token = new JwtSecurityToken(
				issuer: null,
				audience: null,
				claims: claims,
				expires: DateTime.UtcNow.AddHours(1),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
		public static string GetLocalTime()
		{
			return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SE Asia Standard Time").ToString("dd-MM-yyyy HH:mm:ss");
		}
	}
}
