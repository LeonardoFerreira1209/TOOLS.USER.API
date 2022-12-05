using Azure.Messaging.ServiceBus;
using RedeAceitacao.Archetype.Application.Domain.Dtos.Entity;
using RedeAceitacao.Archetype.Application.Domain.Dtos.Message;

namespace RedeAceitacao.Archetype.Application.Infra.ServiceBus.Provider.Base;

/// <summary>
/// Interface de Sender.
/// </summary>
public interface IServiceBusSenderProvider
{
    /// <summary>
    /// Envia uma lista de mensagens para o barramento.
    /// </summary>
    /// <param name="messageList"></param>
    /// <param name="ScheduledEnqueueTime"></param>
    /// <returns></returns>
    Task SendAsync(List<MessageBase> messageList, DateTimeOffset ScheduledEnqueueTime = default);

    /// <summary>
    /// Envia uma mensagem para o barramento.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="ScheduledEnqueueTime"></param>
    /// <returns></returns>
    Task SendAsync(MessageBase item, DateTimeOffset ScheduledEnqueueTime = default);
}

/// <summary>
/// Interface de Receiver.
/// </summary>
public interface IServiceBusReceiverProvider
{
    /// <summary>
    /// Retorna os tipos das mensagens por quantidade.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="quantity"></param>
    /// <returns></returns>
    Task<List<T>> GetMessagesTypedAsync<T>(int quantity);

    /// <summary>
    /// Retorna todas as mensagens.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<MessageEntity<T>> GetMessageAsync<T>();

    /// <summary>
    /// Retorna mensagens por quantidade.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="quantity"></param>
    /// <returns></returns>
    Task<List<MessageEntity<T>>> GetMessagesAsync<T>(int quantity);

    /// <summary>
    /// Retorna mensagens recebidas por quantidade.
    /// </summary>
    /// <param name="quantity"></param>
    /// <returns></returns>
    Task<List<ServiceBusReceivedMessage>> GetMessagesAsync(int quantity);

    /// <summary>
    /// Completa uma mensagem.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task CompleteMessageAsync(ServiceBusReceivedMessage message);

    /// <summary>
    /// Completa uma lista de mensagens.
    /// </summary>
    /// <param name="listMessage"></param>
    /// <returns></returns>
    Task CompleteMessagesAsync(List<ServiceBusReceivedMessage> listMessage);
}
