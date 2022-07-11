using System.ComponentModel.DataAnnotations;

namespace APPLICATION.DOMAIN.DTOS.REQUEST.USER
{
    /// <summary>
    /// Classe de criação de usuário herdada de IdentityUser.
    /// </summary>
    public class UserRequest
    {
        #region Base
        /// <summary>
        /// Nome de usuário
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Email do usuário
        /// </summary>
        [Required][DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Senha do usuário
        /// </summary>
        [Required][DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Numero de telefone
        /// </summary>
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        #endregion

        #region Permissions
        public ICollection<RoleRequest> Roles { get; set; }
        public ICollection<ClaimRequest> Claims { get; set; }
        #endregion
    }
}
