using Microsoft.AspNetCore.Identity;

namespace APPLICATION.DOMAIN.DTOS.ENTITIES.USER;

/// <summary>
/// Classe de Usuários que será armazenada no banco, herdado de IdentityUser.
/// </summary>
public class UserEntity : IdentityUser<Guid> { }
