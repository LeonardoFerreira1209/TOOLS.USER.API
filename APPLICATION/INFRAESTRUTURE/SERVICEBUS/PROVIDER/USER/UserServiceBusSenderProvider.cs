using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.BASE;
using Microsoft.Extensions.Options;

namespace APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.USER;

public class UserServiceBusSenderProvider : ServiceBusSenderProviderBase, IUserEmailServiceBusSenderProvider
{
    public UserServiceBusSenderProvider(IOptions<AppSettings> configuracoes) : base(configuracoes.Value.ConnectionStrings.ServiceBus, configuracoes.Value.ServiceBus.QueueUserEmail) { }
}
