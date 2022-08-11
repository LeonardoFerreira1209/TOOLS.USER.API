namespace APPLICATION.INFRAESTRUTURE.SIGNALR.DTOS;

public class Notify
{
    public Notify(string theme, string message) { Theme = theme;  Message = message; }

    public int Id { get; set; } = new Random().Next(10000000);

    public string Theme { get; set; }

    public string Message { get; set; }

    public string Date { get; set; } = DateTime.Now.ToString();
}
