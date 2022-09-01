using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.ENUMS;
using System.Text.Json.Serialization;

namespace APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;

public class PersonFastRequest
{
    #region Base
    public string FirstName { get; set; }

    public string LastName { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender { get; set; }
    #endregion

    #region Docs
    public string CPF { get; set; }
    #endregion

    #region User
    public UserRequest User { get; set; }
    #endregion
}