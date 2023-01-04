using Azure.Messaging.ServiceBus;

namespace APPLICATION.DOMAIN.ENTITY.MESSAGE;

/// <summary>
/// Classe de mensagem do serviceBus
/// </summary>
/// <typeparam name="T"></typeparam>
public class MessageEntity<T>
{
    /// <summary>
    /// Construtor onde recebe dois parametros de mensagem
    /// </summary>
    /// <param name="_mappedMessage"></param>
    /// <param name="_originalMessage"></param>
    public MessageEntity(T _mappedMessage, ServiceBusReceivedMessage _originalMessage)
    {
        MappedMessage = _mappedMessage; OriginalMessage = _originalMessage;
    }

    /// <summary>
    /// Construtor vazio
    /// </summary>
    public MessageEntity() { }

    /// <summary>
    /// Mensagem mapeada
    /// </summary>
    public T MappedMessage { get; set; }

    /// <summary>
    /// Mensagem do formato ServiceBus
    /// </summary>
    public ServiceBusReceivedMessage OriginalMessage { get; set; }
}
