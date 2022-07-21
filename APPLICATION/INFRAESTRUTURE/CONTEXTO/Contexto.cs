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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<IdentityUser<Guid>>().ToTable("AspNetUsers").HasKey(table => table.Id);
        
        builder.Entity<Person>()
            .HasOne(person => person.User);

        builder.Entity<Contact>()
            .HasOne(contact => contact.Person).WithMany(person => person.Contacts).HasForeignKey(contact => contact.PersonId);

        builder.Entity<Profession>()
            .HasOne(profession => profession.Person).WithMany(person => person.Professions).HasForeignKey(profession => profession.PersonId);

        builder.Entity<Profession>()
            .HasOne(profession => profession.Company).WithMany(company => company.Professions).HasForeignKey(profession => profession.CompanyId);

        base.OnModelCreating(builder);
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
