namespace CQRS.Application.Helpers
{
    public interface IProducerMessageSender
    {
        void SendMessage(ProducerMessage message);
    }
}