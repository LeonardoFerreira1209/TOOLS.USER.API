using APPLICATION.DOMAIN.ENTITY.PROFESSION;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.PROFESSION;

public class ProfessionTypesConfiguration : IEntityTypeConfiguration<Profession>
{
    public void Configure(EntityTypeBuilder<Profession> builder)
    {
        builder.ToTable("Professions").HasKey(profession => profession.Id);

        builder
           .HasOne(profession => profession.Person).WithMany(person => person.Professions).HasForeignKey(profession => profession.PersonId);

        builder
            .HasOne(profession => profession.Company).WithMany(company => company.Professions).HasForeignKey(profession => profession.CompanyId);
    }
}
