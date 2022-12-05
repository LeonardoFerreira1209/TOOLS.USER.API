using Azure.Messaging.ServiceBus;
using System.Diagnostics.CodeAnalysis;

namespace RedeAceitacao.Archetype.Application.Domain.Dtos.Entity
{
    [ExcludeFromCodeCoverage]
    public class MessageEntity<T>
    {
        public MessageEntity(T _mappedMessage, ServiceBusReceivedMessage _originalMessage)
        {
            mappedMessage = _mappedMessage;
            originalMessage = _originalMessage;
        }

        public MessageEntity()
        {
        }

        public T mappedMessage { get; set; }
        public ServiceBusReceivedMessage originalMessage { get; set; }
    }
}
