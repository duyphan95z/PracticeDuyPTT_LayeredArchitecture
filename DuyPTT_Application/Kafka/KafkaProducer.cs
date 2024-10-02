using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace DuyPTT_Application.Kafka
{
	public  class KafkaProducer: IKafka
	{
		// Mục 16 trong bài tập:  Connect kafka (Producer/Consumer)
		// Mục 3b trong bài tập Ghi log và kafka
		private readonly IConfiguration _configuration;
		public KafkaProducer(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		// Producer
		public async Task SendMessage(string message)
		{
			var config = new ProducerConfig { BootstrapServers = _configuration.GetSection("KafkaLogger:BootstrapServers").Value };

			using (var producer = new ProducerBuilder<Null, string>(config).Build())
			{
				try
				{
					var result = await producer.ProduceAsync(_configuration.GetSection("KafkaLogger:Topic").Value, new Message<Null, string> { Value = message });
					Console.WriteLine($"[Producer] - Message sent to {result.TopicPartitionOffset}");
				}
				catch (ProduceException<Null, string> e)
				{
					Console.WriteLine($"[Producer] - Error: {e.Error.Reason}");
				}
			}
		}

		// Consumer
		public void ConsumeMessages()
		{
			var config = new ConsumerConfig
			{
				GroupId = "test-consumer-group",
				BootstrapServers = _configuration.GetSection("KafkaLogger:BootstrapServers").Value,
				AutoOffsetReset = AutoOffsetReset.Earliest
			};

			using (var consumer = new ConsumerBuilder<Null, string>(config).Build())
			{
				consumer.Subscribe(_configuration.GetSection("KafkaLogger:Topic").Value);

				try
				{
					while (true)
					{
						var consumeResult = consumer.Consume();
						Console.WriteLine($"[Consume] - Received message: {consumeResult.Message.Value}");
					}
				}
				catch (ConsumeException e)
				{
					Console.WriteLine($"[Consume] - Error: {e.Error.Reason}");
				}
			}
		}
	}
}
