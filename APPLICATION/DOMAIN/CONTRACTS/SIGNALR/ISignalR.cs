namespace APPLICATION.DOMAIN.CONTRACTS.SIGNALR
{
    public interface ISignalR
    {
        Task ReceiveMessage(string message);
    }
}
