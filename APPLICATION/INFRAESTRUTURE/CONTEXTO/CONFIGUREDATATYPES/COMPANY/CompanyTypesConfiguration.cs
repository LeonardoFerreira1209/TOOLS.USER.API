using APPLICATION.DOMAIN.ENTITY.COMPANY;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.COMPANY;

public class CompanyTypesConfiguration : IEntityTypeConfiguration<CompanyEntity>
{
    public void Configure(EntityTypeBuilder<CompanyEntity> builder)
    {
        // Renomeando nome.
        builder.ToTable("Companies").HasKey(company => company.Id);

        // Guid
        builder.Property(company => company.Id).IsRequired();
        builder.Property(company => company.PlanId).IsRequired();
        builder.Property(company => company.CreatedUserId).IsRequired();
        builder.Property(company => company.UpdatedUserId);

        // String
        builder.Property(company => company.Name).IsRequired();
        builder.Property(company => company.Description).HasMaxLength(80);

        // Enum
        builder.Property(company => company.Status).IsRequired();

        // DateTime
        builder.Property(company => company.StartDate);
        builder.Property(company => company.Created);
        builder.Property(company => company.Updated);

        // Vinculo com usuários.
        builder
            .HasMany(company => company.Users).WithOne(user => user.Company).HasForeignKey(user => user.CompanyId);

        // Vinculo com Roles.
        builder.HasOne(company => company.Plan);
    }
}
