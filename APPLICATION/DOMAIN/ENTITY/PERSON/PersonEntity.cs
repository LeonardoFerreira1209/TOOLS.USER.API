using APPLICATION.DOMAIN.ENTITY.BASE;
using APPLICATION.DOMAIN.ENTITY.CONTACT;
using APPLICATION.DOMAIN.ENTITY.PROFESSION;
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
        public byte[] Image { get; set; }
        #endregion

        #region Docs
        /// <summary>
        /// RG.
        /// </summary>
        public string RG { get; set; }

        /// <summary>
        /// CPF.
        /// </summary>
        public string CPF { get; set; }
        #endregion

        #region User
        /// <summary>
        /// Id do usuário.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Dados do usuário;
        /// </summary>
        public virtual UserEntity User { get; set; }
        #endregion

        #region Profession
        /// <summary>
        /// Profissões vinculadas a pessoa.
        /// </summary>
        public virtual ICollection<ProfessionEntity> Professions { get; set; } = new List<ProfessionEntity>();
        #endregion

        #region Contact
        public virtual ICollection<ContactEntity> Contacts { get; set; } = new HashSet<ContactEntity>();
        #endregion
    }
}
