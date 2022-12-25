using Azure.Messaging.ServiceBus;

namespace RedeAceitacao.Archetype.Application.Domain.Dtos.Entity
{
    public class MessageEntity<T>
    {
        public MessageEntity(T _mappedMessage, ServiceBusReceivedMessage _originalMessage)
        {
            MappedMessage = _mappedMessage; OriginalMessage = _originalMessage;
        }

        public MessageEntity() { }

        public T MappedMessage { get; set; }
        public ServiceBusReceivedMessage OriginalMessage { get; set; }
    }
}
