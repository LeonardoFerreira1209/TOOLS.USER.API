using APPLICATION.DOMAIN.DTOS.RESPONSE.PERSON;

namespace APPLICATION.INFRAESTRUTURE.SIGNALR.CLIENTS;

public interface IPersonClient
{
    Task ReceiveMessage(PersonResponse person);
}
