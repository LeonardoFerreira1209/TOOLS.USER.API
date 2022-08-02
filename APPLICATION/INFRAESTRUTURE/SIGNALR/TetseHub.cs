using APPLICATION.DOMAIN.CONTRACTS.SIGNALR;
using Microsoft.AspNetCore.SignalR;

namespace APPLICATION.INFRAESTRUTURE.SIGNALR
{
    public class TetseHub : Hub<ISignalR>
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.ReceiveMessage(message);
        }
    }
}
