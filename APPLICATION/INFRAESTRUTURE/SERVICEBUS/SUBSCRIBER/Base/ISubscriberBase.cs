using Azure.Messaging.ServiceBus;

namespace APPLICATION.INFRAESTRUTURE.SERVICEBUS.SUBSCRIBER.BASE;

public interface ISubscriberBase
{
    Task RegisterSubscriberAsync(string nameProcess);
    Task StartProcessingAsync();
    Task StopProcessingAsync();
    Task ProcessHandlerAsync(ProcessMessageEventArgs args);
    Task ProcessHandlerErrorAsync(ProcessErrorEventArgs args);
}

public interface ISubscriberProcess
{
    Task ProcessHandlerAsync(ProcessMessageEventArgs args);
}
