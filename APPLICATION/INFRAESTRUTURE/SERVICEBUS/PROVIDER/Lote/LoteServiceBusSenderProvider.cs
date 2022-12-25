using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.BASE;
using Microsoft.Extensions.Options;

namespace APPLICATION.INFRAESTRUTURE.SERVICEBUS.PROVIDER.LOTE;

public class LoteServiceBusSenderProvider : ServiceBusSenderProviderBase, ILoteServiceBusSenderProvider
{
    public LoteServiceBusSenderProvider(IOptions<AppSettings> configuracoes) : base(configuracoes.Value.ConnectionStrings.ServiceBus, configuracoes.Value.Configuracoes.TopicoExemploName) { }
}
