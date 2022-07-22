using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.ENTITY.COMPANY;
using APPLICATION.DOMAIN.ENTITY.CONTACT;
using APPLICATION.DOMAIN.ENTITY.PERSON;
using APPLICATION.DOMAIN.ENTITY.PROFESSION;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO;

/// <summary>
/// Classe de configuração do banco de dados.
/// </summary>
public class Contexto : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    private readonly IOptions<AppSettings> _appSettings;

    public Contexto(DbContextOptions<Contexto> options, IOptions<AppSettings> appsettings) : base(options)
    {
        _appSettings = appsettings;

        Database.EnsureCreated();
    }

    #region C
    public DbSet<Company> Companies { get; set; }

    public DbSet<Contact> Contacts { get; set; }
    #endregion

    #region P
    public DbSet<Person> Persons { get; set; }

    public DbSet<Profession> Professions { get; set; }
    #endregion
}
