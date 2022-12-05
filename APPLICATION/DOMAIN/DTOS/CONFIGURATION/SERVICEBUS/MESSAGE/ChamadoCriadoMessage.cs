namespace APPLICATION.DOMAIN.DTOS.CONFIGURATION.SERVICEBUS.MESSAGE;

public class ChamadoCriadoMessage : MessageBase
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public DateTime Data { get; set; }
}

