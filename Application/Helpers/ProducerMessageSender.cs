using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.IO;

namespace CQRS.Application.Helpers
{
    public static class ProducerMessageSender
    {
         public static void SendMessage(ProducerMessage message) {
            var producerConfig = new Dictionary<string, string> { { "bootstrap.servers", "localhost:62616" } };

            string kafkaTopic = "permissions";
            using (var producer = new ProducerBuilder<string, ProducerMessage>(producerConfig)
            .SetKeySerializer(Serializers.Utf8)
            .SetValueSerializer(new CustomerSerializer())
            .Build())
            {
                var result = producer.ProduceAsync(kafkaTopic, new Message<string, ProducerMessage> { Key = message.Id.ToString(), Value = message }).GetAwaiter().GetResult();
                Console.WriteLine($"Event sent on Partition: {result.Partition} with Offset: {result.Offset}");

                producer.Flush(TimeSpan.FromSeconds(10));
                Console.WriteLine($"messages were produced to topic {kafkaTopic}");
            }
        }

    }
    public class CustomerSerializer : ISerializer<ProducerMessage>
    {
        public byte[] Serialize(ProducerMessage data, SerializationContext context)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(data.Id.ToString());
                    writer.Write(data.OperationName);
                }
                return m.ToArray();
            }
        }
    }
}
