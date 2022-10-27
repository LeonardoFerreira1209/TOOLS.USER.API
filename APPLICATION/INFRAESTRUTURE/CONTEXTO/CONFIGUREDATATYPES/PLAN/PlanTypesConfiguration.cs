using APPLICATION.DOMAIN.ENTITY.PLAN;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.PLAN;

public class PlanTypesConfiguration : IEntityTypeConfiguration<PlanEntity>
{
    public void Configure(EntityTypeBuilder<PlanEntity> builder)
    {
        // Renomeando nome.
        builder.ToTable("Plans").HasKey(company => company.Id);

        // Guid
        builder.Property(company => company.Id).IsRequired();
        builder.Property(company => company.RoleId).IsRequired();
        builder.Property(company => company.CreatedUserId).IsRequired();
        builder.Property(company => company.UpdatedUserId);

        // String
        builder.Property(company => company.PlanName).IsRequired();
        builder.Property(company => company.PlanDescription).HasMaxLength(100);

        // Enum
        builder.Property(company => company.Status).IsRequired();

        // DateTime
        builder.Property(company => company.Created);
        builder.Property(company => company.Updated);

        // Vinculo com roles.
        builder
            .HasOne(company => company.Role).WithMany(role => role.Plans).HasForeignKey(plan => plan.RoleId);
    }
}
