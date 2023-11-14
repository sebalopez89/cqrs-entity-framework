using Confluent.Kafka;
using System.IO;

namespace CQRS.Application.Helpers
{
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
