using DuyPTT_Application.Features.JWT_Token.BUS;
using DuyPTT_Application.Kafka;
using DuyPTT_Repositories.DbConnect;
using DuyPTT_Repositories.User.Interfaces;
using DuyPTT_Repositories.User.Models;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;

namespace DuyPTT_Application.Features.User
{
	public class UserReadWriteJsonTxtRequest : UserReadWriteJsonTxtInput, IRequest<Response<object>>
	{
		public class QueryValidation : AbstractValidator<UserReadWriteJsonTxtRequest>
		{
			// Mục 11 trong bài tập:  Validation (FluentValidation)
			public QueryValidation()
			{
				RuleFor(x => x.userName).NotNull().NotEmpty();
			}
		}
		public class QueryHandler : IRequestHandler<UserReadWriteJsonTxtRequest, Response<object>>
		{
			private readonly IUser _iIUser;
			private readonly IKafka _ikafka;
			public QueryHandler(IUser iIUser, IKafka ikafka)
			{
				_iIUser = iIUser;
				_ikafka = ikafka;
			}
			public async Task<Response<object>> Handle(UserReadWriteJsonTxtRequest request, CancellationToken cancellationToken)
			{
				try
				{
					string filePath = @"C:\Users\duyph\OneDrive\Desktop\PracticeUp\File\Write.txt";
					await _ikafka.SendMessage("Started-UserReadWriteJsonTxtRequest: " + JWT_TokenService.GetLocalTime());
					List<UserReadWriteJsonTxtInput> _data = new List<UserReadWriteJsonTxtInput>();
					if (request.type == 0) // Write
					{
						
						_data.Add(new UserReadWriteJsonTxtInput()
						{
							userId = request.userId,
							userName = request.userName,
							type =request.type,
						});
						using (StreamWriter file = File.CreateText(filePath))
						{
							JsonSerializer serializer = new JsonSerializer();
							serializer.Serialize(file, _data);
						}
					}
					else 
					{
						if (!File.Exists(filePath))
						{
							Console.WriteLine("File does not exist.");
							return null;
						}

						using (StreamReader file = File.OpenText(filePath))
						{
							JsonSerializer serializer = new JsonSerializer();
							_data = (List<UserReadWriteJsonTxtInput>)serializer.Deserialize(file, typeof(List<UserReadWriteJsonTxtInput>));
						}
					}

					await _ikafka.SendMessage("Stoped-UserReadWriteJsonTxtRequest: " + JWT_TokenService.GetLocalTime());
					return new Response<object>(_data, null, "", "");
				}
				catch (Exception ex)
				{
					await _ikafka.SendMessage("Exception-UserReadWriteJsonTxtRequest: " + ex.Message);
					throw new Exception(ex.Message);
				}
				finally
				{
					await _ikafka.SendMessage("Finally-UserReadWriteJsonTxtRequest: " + JWT_TokenService.GetLocalTime());
				}
			}
		}
	}
}
