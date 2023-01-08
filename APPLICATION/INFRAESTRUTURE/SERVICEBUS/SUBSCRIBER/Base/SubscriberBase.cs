using Azure.Messaging.ServiceBus;

namespace APPLICATION.INFRAESTRUTURE.SERVICEBUS.SUBSCRIBER.BASE;

public abstract class SubscriberBase : ISubscriberBase
{
    private readonly ServiceBusSender _sender;
    private readonly ServiceBusProcessor _processor;
    public readonly string _topicName;
    public readonly string _subscriptionName;
    public readonly string _queueName;
    public string _nameProcess;
    public int _maximoTentativas;
    public int _tempoRetry;
    private const int _quantidadeTentativaMinima_ = 1;

    protected SubscriberBase(string serviceBusConnection, string topicName, string subscriptionName, int maximoTentativas, int tempoRetry)
    {
        _topicName = topicName;
        _subscriptionName = subscriptionName;
        _maximoTentativas = maximoTentativas;
        _tempoRetry = tempoRetry;

        var _client = new ServiceBusClient(serviceBusConnection);

        _sender = _client.CreateSender(topicName);

        _processor = _client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());
    }

    protected SubscriberBase(string serviceBusConnection, string queueName, int maximoTentativas, int tempoRetry)
    {
        _queueName = queueName;
        _maximoTentativas = maximoTentativas;
        _tempoRetry = tempoRetry;

        var _client = new ServiceBusClient(serviceBusConnection);

        _sender = _client.CreateSender(queueName);

        _processor = _client.CreateProcessor(queueName, new ServiceBusProcessorOptions());
    }

    public async Task RegisterSubscriberAsync(string nameProcess)
    {
        try
        {
            _nameProcess = nameProcess;
            _processor.ProcessMessageAsync += ProcessHandlerAsync;
            _processor.ProcessErrorAsync += ProcessHandlerErrorAsync;

            //_logWithMetric.LogInfo("Wait for a minute and then press any key to end the processing");

            await this.StartProcessingAsync();
        }
        catch (Exception)
        {
            //_logWithMetric.Error(ex);
        }
    }

    public async Task ProcessarErro(ProcessMessageEventArgs currentMessage, Exception e)
    {
        var message = new ServiceBusMessage
        {
            Body = currentMessage.Message.Body
        };

        foreach (var item in currentMessage.Message.ApplicationProperties)
            message.ApplicationProperties.Add(item);

        if (message.ApplicationProperties.TryGetValue("tentativas", out var tentativas))
        {
            tentativas = (int)tentativas + _quantidadeTentativaMinima_;
            message.ApplicationProperties["tentativas"] = tentativas;
        }
        else
            message.ApplicationProperties.Add("tentativas", _quantidadeTentativaMinima_);

        if ((tentativas is null) || (int)tentativas <= _maximoTentativas)
        {
            //_logWithMetric.LogEmRetentativa($"Agendado em {_tempoRetry} segundos. Mensagem: {e.Message}");

            message.MessageId = Guid.NewGuid().ToString();

            await _sender.ScheduleMessageAsync(message, DateTimeOffset.Now.AddMinutes(_tempoRetry));
        }
        else
        {
            //_logWithMetric.LogError(e, $"Número máximo de tentativas de reprocessamento atingido. Total de ({_maximoTentativas})");
            throw e;
        }
    }

    public virtual async Task ProcessHandlerAsync(ProcessMessageEventArgs args) => await args.CompleteMessageAsync(args.Message);
    
    public virtual async Task ProcessHandlerErrorAsync(ProcessErrorEventArgs args) => await Task.CompletedTask;

    public async Task StartProcessingAsync() => await _processor.StartProcessingAsync();

    public async Task StopProcessingAsync() => await _processor.StopProcessingAsync();
}
