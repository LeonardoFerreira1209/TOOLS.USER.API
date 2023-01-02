using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.USER;
using Microsoft.Extensions.Options;

namespace APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.BASE;

public class UserEmailServiceBusReceiverProvider : ServiceBusReceiverProviderBase, IUserEmailServiceBusReceiverProvider
{
    public UserEmailServiceBusReceiverProvider(IOptions<AppSettings> configuracoes) : base(configuracoes.Value.ConnectionStrings.ServiceBus, configuracoes.Value.ServiceBus.QueueUserEmail) { }
}
