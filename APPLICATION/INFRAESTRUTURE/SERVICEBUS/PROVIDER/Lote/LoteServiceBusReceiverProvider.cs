using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using Microsoft.Extensions.Options;
using RedeAceitacao.Archetype.Application.Domain.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace RedeAceitacao.Archetype.Application.Infra.ServiceBus.Provider.Lote
{
    [ExcludeFromCodeCoverage]
    public class LoteServiceBusReceiverProvider : ServiceBusReceiverProviderBase, ILoteServiceBusReceiverProvider
    {
        public LoteServiceBusReceiverProvider(IOptions<AppSettings> configuracoes) : base(configuracoes.Value.ConnectionStrings.ServiceBus, configuracoes.Value.Configuracoes.TopicoExemploName, "lote")
        {
        }
    }
}
