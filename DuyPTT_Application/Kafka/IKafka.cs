namespace DuyPTT_Application.Kafka
{
	public interface IKafka
	{
		public Task SendMessage(string message);
		public void ConsumeMessages();
	}
}
