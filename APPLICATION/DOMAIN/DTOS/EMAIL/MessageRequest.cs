using MimeKit;

namespace APPLICATION.DOMAIN.DTOS.EMAIL;

public class Message
{
    public Message(IEnumerable<string> receiver, string subject, Guid userId, string activateCode)
    {
        Receiver = new List<MailboxAddress>();

        Receiver.AddRange(receiver.Select(r => new MailboxAddress("receiver", r)));

        Subject = subject;

        Content = $"https://localhost:44345/Ativa?UsuarioId={userId}&Codigo={activateCode}";
    }

    public List<MailboxAddress> Receiver { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
}
