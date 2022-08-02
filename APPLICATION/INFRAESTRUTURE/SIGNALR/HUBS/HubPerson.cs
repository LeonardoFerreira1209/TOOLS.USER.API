using APPLICATION.INFRAESTRUTURE.SIGNALR.CLIENTS;
using Microsoft.AspNetCore.SignalR;

namespace APPLICATION.INFRAESTRUTURE.SIGNALR.HUBS;

public class HubPerson : Hub<IPersonClient> { }
