using APPLICATION.DOMAIN.ENTITY.PLAN;
using APPLICATION.DOMAIN.ENTITY.ROLE;
using APPLICATION.DOMAIN.ENTITY.USER;
using APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.PLAN;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO;

/// <summary>
/// Classe de configuração do banco de dados.
/// </summary>
public class Context : IdentityDbContext<UserEntity, RoleEntity, Guid>
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
        Database.EnsureCreated();
    }

    /// <summary>
    /// Configrações fos datatypes.
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Configutrations
        builder
            // Plan
            .ApplyConfiguration(new PlanTypesConfiguration());

        base.OnModelCreating(builder);
    }

    /// <summary>
    /// Sets de tabelas no banco.
    /// </summary>
    public DbSet<PlanEntity> Plans { get; set; }
}
