using Microsoft.AspNetCore.SignalR;

namespace APPLICATION.INFRAESTRUTURE.SIGNALR;

public class Hubs : Hub
{
    public async IAsyncEnumerable<DateTime> Notifycations(CancellationToken cancellationToken)
    {
        while (true)
        {
            yield return DateTime.UtcNow;
            await Task.Delay(1000, cancellationToken);
        }
    }
}
