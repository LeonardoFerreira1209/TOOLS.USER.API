using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace APPLICATION.INFRAESTRUTURE.SIGNALR;

public class Hubs : Hub
{
    #region Person
    public async Task GetPersonToView(PersonResponse personResponse)
    {
        await Clients.All.SendAsync("ReceivePersonToView", personResponse);
    }
    #endregion

    public async Task Notifications(string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", new
        {
            message = message,
            date = DateTime.Now,
        });

        Log.Information("Adicionado");
    }
}
