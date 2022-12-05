using RedeAceitacao.Archetype.Application.Domain.Dtos;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;

namespace RedeAceitacao.Archetype.Application.Infra.ServiceBus.Provider.Lote
{
    [ExcludeFromCodeCoverage]
    public class LoteServiceBusSenderProvider : ServiceBusSenderProviderBase, ILoteServiceBusSenderProvider
    {
        public LoteServiceBusSenderProvider(IOptions<AppSettings> configuracoes): base(configuracoes.Value.ConnectionStrings.ServiceBus, configuracoes.Value.Configuracoes.TopicoExemploName)
        {
        }
    }
}
