using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.BASE;
using Microsoft.Extensions.Options;

namespace APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.USER;

/// <summary>
/// Classe de envio de mensagem de email de usuário para service bus. 
/// </summary>
public class UserEmailServiceBusSenderProvider : ServiceBusSenderProviderBase, IUserEmailServiceBusSenderProvider
{
    /// <summary>
    /// construtor.
    /// </summary>
    /// <param name="configuracoes"></param>
    public UserEmailServiceBusSenderProvider(IOptions<AppSettings> configuracoes) : base(configuracoes.Value.ConnectionStrings.ServiceBus, configuracoes.Value.ServiceBus.QueueEmail) { }
}
