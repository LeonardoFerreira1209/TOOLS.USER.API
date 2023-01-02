using APPLICATION.DOMAIN.DTOS.CONFIGURATION.SERVICEBUS.MESSAGE;
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.BASE;

public abstract class ServiceBusSenderProviderBase
{
    private readonly ServiceBusSender _clientSender;

    protected ServiceBusSenderProviderBase(string servicebusconexao, string queueOrTopic)
    {
        var client = new ServiceBusClient(servicebusconexao);

        _clientSender = client.CreateSender(queueOrTopic);
    }

    public virtual async Task SendAsync(List<MessageBase> messageList, DateTimeOffset ScheduledEnqueueTime = default)
    {
        var splitList = await SplitList(messageList, 100);

        Parallel.ForEach(splitList, async filterList =>
        {
            var result = ComposeMessageBase(filterList);

            if (ScheduledEnqueueTime.Equals(default)) await _clientSender.SendMessagesAsync(result); else await _clientSender.ScheduleMessagesAsync(result, ScheduledEnqueueTime);
        });
    }

    public virtual async Task SendAsync(MessageBase item, DateTimeOffset ScheduledEnqueueTime = default)
    {
        if (ScheduledEnqueueTime.Equals(default)) await _clientSender.SendMessageAsync(ItemToMessage(item, item.Headers)); else await _clientSender.ScheduleMessageAsync(ItemToMessage(item, item.Headers), ScheduledEnqueueTime);
    }

    #region private methods

    private List<ServiceBusMessage> ComposeMessageBase(List<MessageBase> messageList)
    {
        var composeMessage = new List<ServiceBusMessage>();

        foreach (var item in messageList) composeMessage.Add(ItemToMessage(item, item.Headers));

        return composeMessage;
    }

    private Task<List<List<T>>> SplitList<T>(List<T> listMessage, int size = 30)
    {
        var list = new List<List<T>>();

        for (int i = 0; i < listMessage.Count; i += size) list.Add(listMessage.GetRange(i, Math.Min(size, listMessage.Count - i)));

        return Task.FromResult(list);
    }

    private ServiceBusMessage ItemToMessage(object item, Dictionary<string, object> headers = null)
    {
        var jsonMessage = item.SerializeIgnoreNullValues();

        var bytesMessage = Encoding.UTF8.GetBytes(jsonMessage);
        var message = new ServiceBusMessage(bytesMessage) { SessionId = Guid.NewGuid().ToString() };

        if (!(headers is null))
        {
            foreach (var prop in headers)
            {

                if (!prop.Key.Equals("_CUSTOM_PARAMETER_"))
                    message.ApplicationProperties.Add(prop.Key, prop.Value.ToString());
                else
                {
                    dynamic _item = JsonConvert.DeserializeObject<dynamic>(jsonMessage);

                    var key = prop.Value.ToString();
                    var value = _item[prop.Value].Value;

                    message.ApplicationProperties.Add(key, value);
                }
            }
        }

        return message;
    }

    #endregion
}
