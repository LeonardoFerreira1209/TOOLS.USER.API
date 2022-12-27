using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Newtonsoft.Json;
using RedeAceitacao.Archetype.Application.Domain.Dtos.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.BASE;

[ExcludeFromCodeCoverage]
public abstract class ServiceBusReceiverProviderBase
{
    private readonly ServiceBusReceiver _serviceBusReceiver;

    private readonly ServiceBusReceiveMode _receiveMode;

    private readonly ServiceBusClient _serviceBusClient;

    private readonly ServiceBusAdministrationClient _serviceBusAdministrationClient;

    private readonly string _topicPath;

    private readonly string _subscriber;

    private readonly TimeSpan _LOCK_AWAIT_ = TimeSpan.FromMinutes(5);

    public ServiceBusReceiverProviderBase(string servicebusconexao, string topicoName, string subscriptionName, ServiceBusReceiveMode receiveMode = ServiceBusReceiveMode.PeekLock)
    {
        _serviceBusClient = new ServiceBusClient(servicebusconexao);

        _serviceBusAdministrationClient = new ServiceBusAdministrationClient(servicebusconexao);

        _receiveMode = receiveMode;

        _topicPath = topicoName;

        _subscriber = subscriptionName;

        _serviceBusReceiver = _serviceBusClient.CreateReceiver(topicoName, subscriptionName, new ServiceBusReceiverOptions { ReceiveMode = receiveMode });
        
    }

    /// <summary>
    /// Obtem uma lista de mensagens do topico no Servicebus:
    ///   Este método não garante o retorno de mensagens `quantity` exatas, mesmo
    ///   se houver mensagens `quantity` disponíveis na fila ou tópico.
    /// </summary>
    /// <param name="quantity">Quantidade de mensagens que vai retornar</param>
    /// <returns>Retorna uma lista de (T) convertida</returns>
    public virtual async Task<List<T>> GetMessagesTypedAsync<T>(int quantity)
    {
        var messages = new List<T>();

        var listMessage = new List<ServiceBusReceivedMessage>();

        var count = Convert.ToInt32(await ActiveMessageCount());

        var quantidade = this.CountQuantity(count, quantity);

        var counter = this.SetCounter(quantidade, listMessage.Count);

        if (count.Equals(0)) return messages;

        do
        {
            listMessage.AddRange(await _serviceBusReceiver.ReceiveMessagesAsync(counter, _LOCK_AWAIT_) as List<ServiceBusReceivedMessage>);

            counter = this.SetCounter(quantidade, listMessage.Count);

        } while (counter != 0);

        foreach (var item in listMessage)
        {
            var mappedMessage = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(item.Body));

            messages.Add(mappedMessage);
        }

        return messages;
    }

    /// <summary>
    /// Obtem a ultima mensagem do serviceBus
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>retorna uma tupla com o valor convertido e o valor original</returns>
    public virtual async Task<MessageEntity<T>> GetMessageAsync<T>()
    {
        var receiveMessage = await _serviceBusReceiver.ReceiveMessageAsync(_LOCK_AWAIT_);

        var receiveMessageConvert = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(receiveMessage.Body));

        return new MessageEntity<T> { MappedMessage = receiveMessageConvert, OriginalMessage = receiveMessage };
    }

    /// <summary>
    /// Obtem uma lista de mensagens do topico no Servicebus:
    ///   Este método não garante o retorno de mensagens `quantity` exatas, mesmo
    ///   se houver mensagens `quantity` disponíveis na fila ou tópico.
    /// </summary>
    /// <param name="quantity">Quantidade de mensagens que vai retornar</param>
    /// <returns>Retorna uma lista de (T) convertida</returns>
    public virtual async Task<List<MessageEntity<T>>> GetMessagesAsync<T>(int quantity)
    {
        var messages = new List<MessageEntity<T>>();

        var listMessage = new List<ServiceBusReceivedMessage>();

        var count = Convert.ToInt32(await ActiveMessageCount());

        var quantidade = this.CountQuantity(count, quantity);

        var counter = this.SetCounter(quantidade, listMessage.Count);

        if (count.Equals(0)) return messages;

        do
        {
            listMessage.AddRange(await _serviceBusReceiver.ReceiveMessagesAsync(counter, _LOCK_AWAIT_) as List<ServiceBusReceivedMessage>);

            counter = this.SetCounter(quantidade, listMessage.Count);

        } while (counter != 0);

        foreach (var item in listMessage)
        {
            var mappedMessage = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(item.Body));

            messages.Add(new MessageEntity<T>(mappedMessage, item));
        }

        return messages;
    }

    /// <summary>
    /// Obtem uma lista de mensagens do topico no Servicebus:
    ///   Este método não garante o retorno de mensagens `quantity` exatas, mesmo
    ///   se houver mensagens `quantity` disponíveis na fila ou tópico.
    /// </summary>
    /// <param name="quantity">Quantidade de mensagens que vai retornar</param>
    /// <returns>Retorna uma lista de (T) convertida</returns>
    public virtual async Task<List<ServiceBusReceivedMessage>> GetMessagesAsync(int quantity)
    {
        var listMessage = new List<ServiceBusReceivedMessage>();

        var count = Convert.ToInt32(await ActiveMessageCount());

        var quantidade = this.CountQuantity(count, quantity);

        var counter = this.SetCounter(quantidade, listMessage.Count);

        if (count.Equals(0))  return listMessage;

        do
        {
            listMessage.AddRange(await _serviceBusReceiver.ReceiveMessagesAsync(counter, _LOCK_AWAIT_) as List<ServiceBusReceivedMessage>);

            counter = this.SetCounter(quantidade, listMessage.Count);

        } while (counter != 0);

        return listMessage;
    }

    /// <summary>
    /// Completa e retira uma mensagem do serviceBus
    /// </summary>
    /// <param name="message">Mensagem que vai ser completada no serviceBus</param>
    /// <returns>none</returns>
    public virtual async Task CompleteMessageAsync(ServiceBusReceivedMessage message)
    {
        if (_receiveMode != ServiceBusReceiveMode.ReceiveAndDelete)
            await _serviceBusReceiver.CompleteMessageAsync(message);
        else
            throw new Exception("ServiceBusReceiveMode configurado como ReceiveAndDelete, não é necessário completar a mensagem.");
    }

    /// <summary>
    /// Completa e retira uma lista de mensagens do serviceBus
    /// </summary>
    /// <param name="listMessage">Lista de Mensagens que vai ser completada no serviceBus</param>
    /// <returns>none</returns>
    public virtual async Task CompleteMessagesAsync(List<ServiceBusReceivedMessage> listMessage)
    {
        if (_receiveMode != ServiceBusReceiveMode.ReceiveAndDelete)
            listMessage.ForEach(async message => await _serviceBusReceiver.CompleteMessageAsync(message));
        else
            throw new Exception("ServiceBusReceiveMode configurado como ReceiveAndDelete, não é necessário completar a mensagem.");
    }

    /// <summary>
    /// busca a quantidade de mensagens no topico
    /// </summary>
    /// <returns>Quantidade de mensagens no topico</returns>
    private async Task<long> ActiveMessageCount()
    {
        var runtimeProps = await _serviceBusAdministrationClient.GetSubscriptionRuntimePropertiesAsync(_topicPath, _subscriber);

        return runtimeProps.Value.ActiveMessageCount;
    }

    /// <summary>
    /// Retorna qual parametro passado é maior
    /// </summary>
    /// <param name="count"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    private int CountQuantity(int count, int quantity)
    {
        return (count > quantity) ? quantity : count;
    }

    /// <summary>
    /// Retorna o valor subtraido 
    /// </summary>
    /// <param name="quantity"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    int SetCounter(int quantity, int count)
    {
        return (quantity - count);
    }
}
