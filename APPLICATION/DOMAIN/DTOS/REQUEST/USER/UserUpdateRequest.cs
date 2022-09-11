using System.ComponentModel.DataAnnotations;

namespace APPLICATION.DOMAIN.DTOS.REQUEST.USER
{
    /// <summary>
    /// Classe de criação de usuário herdada de IdentityUser.
    /// </summary>
    public class UserUpdateRequest
    {
        #region Base
        /// <summary>
        /// Id do usúario
        /// </summary>
        public Guid Id{ get; set; }

        /// <summary>
        /// Nome de usuário
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Email do usuário
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Email do usuário confirmado.
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Senha do usuário atual.
        /// </summary>
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// Senha do usuário
        /// </summary>
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Numero de telefone
        /// </summary>
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Número do usuário confirmado.
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Lockout habilitado.
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// Data final do lockout
        /// </summary>
        public DateTime? LockoutEnd { get; set; }

        /// <summary>
        /// Security stamp
        /// </summary>
        public string SecurityStamp { get; set; }

        /// <summary>
        /// Concurrency Stamp
        /// </summary>
        public string ConcurrencyStamp { get; set; }
        
        /// <summary>
        /// Validação de dois fatores habilitada.
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Quantidade de falhas de acesso.
        /// </summary>
        public int AccessFailedCount { get;set; }
        #endregion
    }
}
