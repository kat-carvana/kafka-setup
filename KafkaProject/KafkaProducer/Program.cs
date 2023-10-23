using Confluent.Kafka;

var config = new ProducerConfig {
    BootstrapServers = "localhost:9092",
    SecurityProtocol = SecurityProtocol.Plaintext,
};

using (var p = new ProducerBuilder<Null, string>(config).Build())
{
    while (true)
    {
        for (var i = 0; i < 100; i++)
        {
            try
            {
                var toSend = $"This is message {i}";
                var dr = await p.ProduceAsync("topic1", new Message<Null, string> { Value=toSend });
                Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
        }

        p.Flush(TimeSpan.FromSeconds(10));
    }
    
}