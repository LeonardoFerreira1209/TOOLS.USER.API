using APPLICATION.DOMAIN.ENTITY.BASE;
using APPLICATION.DOMAIN.ENTITY.COMPANY;
using APPLICATION.DOMAIN.ENTITY.CONTACT;
using APPLICATION.DOMAIN.ENTITY.USER;
using APPLICATION.ENUMS;

namespace APPLICATION.DOMAIN.ENTITY.PERSON
{
    public class PersonEntity : BaseEntity
    {
        #region Base
        /// <summary>
        /// Primeiro nome.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Ultimo nome.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Idade.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Data de aniversário.
        /// </summary>
        public string BirthDay { get; set; }

        /// <summary>
        /// Gênero.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Imagem de perfil.
        /// </summary>
        public string ImageUri { get; set; }
        #endregion

        /// <summary>
        /// RG.
        /// </summary>
        public string RG { get; set; }

        /// <summary>
        /// CPF.
        /// </summary>
        public string CPF { get; set; }

        /// <summary>
        /// Id do usuário.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Dados do usuário.
        /// </summary>
        public virtual UserEntity User { get; set; }

        /// <summary>
        /// Id da empresa.
        /// </summary>
        public Guid? CompanyId { get; set; }

        /// <summary>
        /// Empresa em que o usuário é vinculado.
        /// </summary>
        public virtual CompanyEntity Company { get; set; }

        /// <summary>
        /// Coleção de contatos.
        /// </summary>
        public virtual ICollection<ContactEntity> Contacts { get; set; } = new HashSet<ContactEntity>();
    }
}
