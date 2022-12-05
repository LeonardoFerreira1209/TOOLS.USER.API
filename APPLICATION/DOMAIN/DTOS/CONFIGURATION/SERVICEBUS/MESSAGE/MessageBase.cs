using Newtonsoft.Json;

namespace APPLICATION.DOMAIN.DTOS.CONFIGURATION.SERVICEBUS.MESSAGE;

public abstract class MessageBase
{
    public Guid? EventoId { get; set; }
    public string TipoEvento { get; set; }

    [JsonIgnore]
    public Dictionary<string, object> Headers { get; set; }
}
