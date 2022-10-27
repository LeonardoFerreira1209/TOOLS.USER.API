﻿using APPLICATION.ENUMS;
using System.ComponentModel.DataAnnotations;

namespace APPLICATION.DOMAIN.DTOS.REQUEST.USER
{
    /// <summary>
    /// Classe de criação de usuário herdada de IdentityUser.
    /// </summary>
    public class UserCreateRequest
    {
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

        /// <summary>
        /// RG.
        /// </summary>
        public string RG { get; set; }

        /// <summary>
        /// CPF.
        /// </summary>
        public string CPF { get; set; }

        /// <summary>
        /// Id da empresa.
        /// </summary>
        public Guid CompanyId { get; set; }

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
        /// Senha do usuário
        /// </summary>
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Numero de telefone
        /// </summary>
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

    }
}
