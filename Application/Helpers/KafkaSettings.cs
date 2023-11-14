namespace CQRS.Application.Helpers
{
    public class KafkaSettings
    {
        public string? BrokerUrl { get; set; }
        public string? Topic { get; set; }
    }
}
