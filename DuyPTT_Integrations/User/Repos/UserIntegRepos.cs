using DuyPTT_Integrations.User.Interface;
using DuyPTT_Integrations.User.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace DuyPTT_Integrations.User.Repos
{
	public class UserIntegRepos: IUserInteg
	{
		private readonly HttpClient _httpClient;
		private readonly IHttpClientFactory _httpClientFactory;
		private IConfiguration _configuration;

		public UserIntegRepos(IConfiguration configuration, IHttpClientFactory httpClientFactory, HttpClient httpClient)
		{
			_configuration = configuration;
			_httpClientFactory = httpClientFactory;
			_httpClient = httpClient;
		}

		public async Task<string> CallApiGetUserName(GetUserInputInteg input)
		{
			var PathGetUser = _configuration.GetSection("Services:ApiGetuser:Host").Value;

			var cancelTokenResouce = new CancellationTokenSource();

			var client = _httpClientFactory.CreateClient("APIGETUSER");

			client.BaseAddress = new Uri(PathGetUser);



			var bearerToken = await GetToken();
			var credentials = new
			{
				userName = input.userName,
			};
			var jsonContent = JsonConvert.SerializeObject(credentials);
			var req = new HttpRequestMessage(HttpMethod.Post, _configuration.GetSection("Services:ApiGetuser:Host").Value)
			{
				Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
			};
			req.Headers.Add("Authorization", $"Bearer {bearerToken}");
			var response = await client.SendAsync(req, cancelTokenResouce.Token);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadAsStringAsync();
		}
		public async Task<string> GetToken()
		{
			try
			{
				var credentials = new
				{
					username = "duyptt",
					password = "duyptt@123"
				};

				var jsonContent = JsonConvert.SerializeObject(credentials);

				var req = new HttpRequestMessage(HttpMethod.Post, _configuration.GetSection("Services:ApiGetToken:Host").Value)
				{
					Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
				};

				var httpResponseMessage = await _httpClient.SendAsync(req);
				var responseString = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

			
				TokenModel RsToken = JsonConvert.DeserializeObject<TokenModel>(responseString);
				DataInfo DataFromRsToken = JsonConvert.DeserializeObject<DataInfo>(JsonConvert.SerializeObject(RsToken.Data));

				return DataFromRsToken.jwtToken;
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}
		}

	}
	public class TokenModel
	{
		public string success { get; set; }
		public string message { get; set; }
		public string exceptionMessage { get; set; }
		public string clientRequestId { get; set; }
		public object Data { get; set; }
	}
	public class DataInfo
	{
		public string user { get; set; }
		public string jwtToken { get; set; }
		public string expires { get; set; }
	}
}
