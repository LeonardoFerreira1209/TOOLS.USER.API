using APPLICATION.INFRAESTRUTURE.SIGNALR.CLIENTS;
using Microsoft.AspNetCore.SignalR;

namespace APPLICATION.INFRAESTRUTURE.SIGNALR.HUBS;

public class HubNotify : Hub<INotifyClient> { }