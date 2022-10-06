using APPLICATION.DOMAIN.ENTITY.COMPANY;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.COMPANY;

public class CompanyTypesConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        // Renomeando nome.
        builder.ToTable("Companies").HasKey(company => company.Id);

        // Guid
        builder.Property(company => company.Id).IsRequired();
        builder.Property(company => company.CreatedUserId).IsRequired();

        // String
        builder.Property(company => company.Name).IsRequired();
        builder.Property(company => company.Description).HasMaxLength(80);

        // Enum
        builder.Property(company => company.Status).IsRequired();

        // DateTime
        builder.Property(company => company.StartDate).IsRequired();

        // Vinculo com profissões.
        builder
            .HasMany(company => company.Professions).WithOne(profession => profession.Company).HasForeignKey(profession => profession.CompanyId);

        // Vinculo com Roles.
        builder.HasMany(company => company.Roles).WithOne(role => role.Company).HasForeignKey(role => role.CompanyId);
    }
}
