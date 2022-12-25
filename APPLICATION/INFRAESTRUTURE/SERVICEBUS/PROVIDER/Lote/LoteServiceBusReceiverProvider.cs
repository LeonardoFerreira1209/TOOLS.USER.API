using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.LOTE;
using Microsoft.Extensions.Options;

namespace APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.BASE;

public class LoteServiceBusReceiverProvider : ServiceBusReceiverProviderBase, ILoteServiceBusReceiverProvider
{
    public LoteServiceBusReceiverProvider(IOptions<AppSettings> configuracoes) : base(configuracoes.Value.ConnectionStrings.ServiceBus, configuracoes.Value.Configuracoes.TopicoExemploName, "lote") { }
}
