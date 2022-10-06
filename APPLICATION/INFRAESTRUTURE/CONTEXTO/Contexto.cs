using APPLICATION.DOMAIN.ENTITY.COMPANY;
using APPLICATION.DOMAIN.ENTITY.CONTACT;
using APPLICATION.DOMAIN.ENTITY.PERSON;
using APPLICATION.DOMAIN.ENTITY.PROFESSION;
using APPLICATION.DOMAIN.ENTITY.ROLE;
using APPLICATION.DOMAIN.ENTITY.USER;
using APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.COMPANY;
using APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.CONTACT;
using APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.PROFESSION;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO;

/// <summary>
/// Classe de configuração do banco de dados.
/// </summary>
public class Contexto : IdentityDbContext<User, Role, Guid>
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
    ////public DbSet<User> Users { get; set; }
    ////public DbSet<Role> Roles{ get; set; }

    public DbSet<Company> Companies { get; set; }

    public DbSet<Contact> Contacts { get; set; }
    #endregion

    #region P
    public DbSet<Person> Persons { get; set; }

    public DbSet<Profession> Professions { get; set; }
    #endregion

    #endregion
}
