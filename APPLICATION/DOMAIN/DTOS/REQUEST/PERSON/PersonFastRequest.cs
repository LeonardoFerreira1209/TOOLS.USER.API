using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.ENUMS;

namespace APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE
{
    public class PersonFastRequest
    {
        #region Base
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }
        #endregion

        #region Docs
        public string CPF { get; set; }
        #endregion

        #region User
        public UserRequest User { get; set; }
        #endregion
    }
}
