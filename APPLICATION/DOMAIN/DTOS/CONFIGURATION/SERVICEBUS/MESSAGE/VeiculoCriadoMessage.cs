namespace APPLICATION.DOMAIN.DTOS.CONFIGURATION.SERVICEBUS.MESSAGE;


public class VeiculoCriadoMessage : MessageBase
{
    public int Id { get; set; }

    public string ApelidoVeiculo { get; set; }

    public int Ano { get; set; }

    public string Modelo { get; set; }

    public decimal Valor { get; set; }
}

