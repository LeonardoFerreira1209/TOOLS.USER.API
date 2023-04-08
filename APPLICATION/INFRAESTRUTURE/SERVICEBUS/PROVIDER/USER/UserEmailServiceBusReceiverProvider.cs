using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.BASE;
using Microsoft.Extensions.Options;

namespace APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.USER;

/// <summary>
/// Classe de recebimento de mensagem de email de usuário para service bus. 
/// </summary>
public class UserEmailServiceBusReceiverProvider : ServiceBusReceiverProviderBase, IUserEmailServiceBusReceiverProvider
{
    /// <summary>
    /// Construtor.
    /// </summary>
    /// <param name="configuracoes"></param>
    public UserEmailServiceBusReceiverProvider(IOptions<AppSettings> configuracoes) : base(configuracoes.Value.ConnectionStrings.ServiceBus, configuracoes.Value.ServiceBus.QueueEmail) { }
}
