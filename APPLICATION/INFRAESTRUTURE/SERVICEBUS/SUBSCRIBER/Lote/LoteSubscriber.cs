using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using RedeAceitacao.Archetype.Application.Infra.ServiceBus.Subscriber;
using RedeAceitacao.Archetype.Application.Infra.ServiceBus.Subscriber.Base;
using Serilog.Context;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace RedeAceitacao.Archetype.Application.Infra.ServiceBus
{
    [ExcludeFromCodeCoverage]
    public class LoteSubscriber : SubscriberBase, ISubscriberProcess
    {
        public LoteSubscriber(
            IOptions<AppSettings> options
        ) : base(
            options.Value.ConnectionStrings.ServiceBus,
            options.Value.Configuracoes.TopicoExemploName,
            options.Value.Configuracoes.SubscriptionExemploName,
            options.Value.Configuracoes.QuantidadeMaximaDeRetentativas,
            options.Value.Configuracoes.TempoReagendamentoMinutos
        )
        {
        }

        public override async Task ProcessHandlerAsync(ProcessMessageEventArgs args)
        {
            var message = JsonSerializer.Serialize(args.Message.Body.ToString());

            using (LogContext.PushProperty("Processo", base._topicName))
            using (LogContext.PushProperty("Subscription", base._subscriptionName))
            using (LogContext.PushProperty("CorrelationId", Guid.NewGuid()))
            using (LogContext.PushProperty("Payload", JsonSerializer.Serialize(message)))
            {
                try
                {
                    //_logWithMetric.SetTitle("LoteSubscriber");
                    //_logWithMetric.LogStart();

                    //await Task.Run(() => /*_logWithMetric.LogInfo($"{base._nameProcess} : Email Enviado com sucesso: {message}")*/);

                    //_logWithMetric.LogFinish();
                }
                catch (Exception e)
                {
                    await ProcessarErro(args, e);
                }
            }
        }
    }
}
