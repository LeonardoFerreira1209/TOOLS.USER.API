namespace APPLICATION.INFRAESTRUTURE.SIGNALR.DTOS;

public class Notify
{
    public Notify(string message) { Message = message; }

    public int Id { get; set; } = new Random().Next(10000000);

    public string Message { get; set; }

    public string Date { get; set; } = DateTime.Now.ToShortDateString();
}
