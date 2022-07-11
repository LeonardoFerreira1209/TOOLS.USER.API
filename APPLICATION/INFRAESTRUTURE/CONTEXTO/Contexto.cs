using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO;

/// <summary>
/// Classe de configuração do banco de dados.
/// </summary>
public class Contexto : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    //private readonly IOptions<AppSettings> _appSettings;

    public Contexto(DbContextOptions<Contexto> options /*IOptions<AppSettings> appsettings*/) : base(options)
    {
       // _appSettings = appsettings;

        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<IdentityUser<Guid>>().ToTable("AspNetUsers").HasKey(t => t.Id); base.OnModelCreating(builder);
    }
}
