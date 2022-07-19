using APPLICATION.ENUMS;
using Microsoft.AspNetCore.Identity;

namespace APPLICATION.DOMAIN.ENTITY
{
    public class Person
    {
        #region Base
        public Guid Id { get; set; }

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
        public Guid UserId { get; set; }

        public virtual IdentityUser<Guid> User { get; set; }
        #endregion
    }
}
