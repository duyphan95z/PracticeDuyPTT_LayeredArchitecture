using System.Diagnostics;

namespace DuyPTT_Repositories.DbConnect
{
	public class Response<T>
	{
		public Response() { }
		public Response(T _data, T _ErrorData, string _exceptionMessage, string _message = null)
		{
			success = true;
			message = _message;
			errorData = _ErrorData;
			exceptionMessage = _exceptionMessage;
			clientRequestId = ((Activity.Current != null) ? Activity.Current.TraceId.ToString() : string.Empty);
			data = _data;
		}

		public Response(string _exceptionMessage)
		{
			success = false;
			message = "";
			exceptionMessage = _exceptionMessage;
			clientRequestId = ((Activity.Current != null) ? Activity.Current.TraceId.ToString() : string.Empty);
		}


		public bool success { get; set; } = false;
		public string message { get; set; }
		public string exceptionMessage { get; set; }
		public string clientRequestId { get; set; }
		public T data { get; set; }
		public T? errorData { get; set; }
	}
}
