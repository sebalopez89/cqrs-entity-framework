using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace CQRS.Application.Helpers
{
    public class ProducerMessageSender : IProducerMessageSender
    {
        private readonly IOptions<KafkaSettings> _settings;
        public ProducerMessageSender(IOptions<KafkaSettings> settings)
        {
            _settings = settings;
        }

        public void SendMessage(ProducerMessage message)
        {
            var producerConfig = new Dictionary<string, string> { { "bootstrap.servers", _settings.Value.BrokerUrl! } };

            string kafkaTopic = _settings.Value.Topic!;
            using (var producer = new ProducerBuilder<string, ProducerMessage>(producerConfig)
            .SetKeySerializer(Serializers.Utf8)
            .SetValueSerializer(new CustomerSerializer())
            .Build())
            {
                var result = producer.ProduceAsync(kafkaTopic, new Message<string, ProducerMessage> { Key = message.Id.ToString(), Value = message });
                producer.Flush(TimeSpan.FromSeconds(10));
                Console.WriteLine($"messages were produced to topic {kafkaTopic}");
            }
        }
    }
}
