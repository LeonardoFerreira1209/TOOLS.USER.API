using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.ENUMS;

namespace APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE
{
    public class PersonRequest
    {
        #region Base
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Age { get; set; }

        public string Birthday { get; set; }

        public Gender Gender { get; set; }

        public Uri ImageUrl { get; set; }

        public string Profession { get; set; }
        #endregion

        #region Docs
        public string RG { get; set; }

        public string CPF { get; set; }
        #endregion

        #region Address
        public string Cep { get; set; }
        #endregion

        #region User

        public UserRequest userRequest { get; set; }

        #endregion
    }
}
