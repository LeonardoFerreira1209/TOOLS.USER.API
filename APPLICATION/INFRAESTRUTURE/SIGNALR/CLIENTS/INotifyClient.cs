using APPLICATION.INFRAESTRUTURE.SIGNALR.DTOS;

namespace APPLICATION.INFRAESTRUTURE.SIGNALR.CLIENTS;

public interface INotifyClient
{
    Task ReceiveMessage(Notify notify);
}
