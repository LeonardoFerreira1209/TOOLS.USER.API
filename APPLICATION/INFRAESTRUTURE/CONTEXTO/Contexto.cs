using APPLICATION.DOMAIN.ENTITY.COMPANY;
using APPLICATION.DOMAIN.ENTITY.CONTACT;
using APPLICATION.DOMAIN.ENTITY.PERSON;
using APPLICATION.DOMAIN.ENTITY.PROFESSION;
using APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.COMPANY;
using APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.CONTACT;
using APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.PROFESSION;
using APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.USER;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO;

/// <summary>
/// Classe de configuração do banco de dados.
/// </summary>
public class Contexto : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    public Contexto(DbContextOptions<Contexto> options) : base(options)
    {
        Database.EnsureCreated();
    }

    /// <summary>
    /// Configrações fos datatypes.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configutrations
        modelBuilder
            // User
            .ApplyConfiguration(new UsersTypeConfiguration())
            .ApplyConfiguration(new UserTokensTypeConfiguration())
            .ApplyConfiguration(new UserLoginsTypeConfiguration())
            .ApplyConfiguration(new UserClaimsTypeConfiguration())
            .ApplyConfiguration(new UserRolesTypeConfiguration())
            // Roles
            .ApplyConfiguration(new RolesTypeConfiguration())
            .ApplyConfiguration(new RoleClaimsTypeConfiguration())
            // Profession
            .ApplyConfiguration(new ProfessionTypesConfiguration())
            // Contact
            .ApplyConfiguration(new ContactTypesConfiguration())
            // Company
            .ApplyConfiguration(new CompanyTypesConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Sets de tabelas no banco.
    /// </summary>
    #region Dbset's

    #region C
    public DbSet<Company> Companies { get; set; }

    public DbSet<Contact> Contacts { get; set; }
    #endregion

    #region P
    public DbSet<Person> Persons { get; set; }

    public DbSet<Profession> Professions { get; set; }
    #endregion

    #endregion
}
