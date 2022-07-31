using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace APPLICATION.INFRAESTRUTURE.SIGNALR;

public class Hubs : Hub
{
    public async Task Notifycations()
    {
        while (true)
        {
            await Clients.All.SendAsync("ReceiveNotification", new
            {
                message = "Nova mensagem teste 123",
                date = DateTime.Now,
            });

            Log.Information("Adicionado");

           await Task.Delay(5000);
        }
    }
}
