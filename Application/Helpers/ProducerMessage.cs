using System;

namespace CQRS.Application.Helpers
{
    public class ProducerMessage
    {
        public ProducerMessage(string operationMethod)
        {
            OperationName = operationMethod;
        }
        public Guid Id { get => Guid.NewGuid();}
        public string OperationName { get; set; }
    }
}
