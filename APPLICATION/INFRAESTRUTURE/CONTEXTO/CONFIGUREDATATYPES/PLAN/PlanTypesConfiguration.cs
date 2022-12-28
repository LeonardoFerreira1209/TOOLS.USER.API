using APPLICATION.DOMAIN.ENTITY.PLAN;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.PLAN;

public class PlanTypesConfiguration : IEntityTypeConfiguration<PlanEntity>
{
    public void Configure(EntityTypeBuilder<PlanEntity> builder)
    {
        // Renomeando nome.
        builder.ToTable("Plans").HasKey(plan => plan.Id);

        // Guid
        builder.Property(plan => plan.Id).IsRequired();
        builder.Property(plan => plan.RoleId).IsRequired();
        builder.Property(plan => plan.CreatedUserId).IsRequired();
        builder.Property(plan => plan.UpdatedUserId);

        // String
        builder.Property(plan => plan.PlanName).IsRequired();
        builder.Property(plan => plan.PlanDescription).HasMaxLength(100);

        // Enum
        builder.Property(plan => plan.Status).IsRequired();

        // DateTime
        builder.Property(plan => plan.Created);
        builder.Property(plan => plan.Updated);

        // Vinculo com roles.
        builder
            .HasOne(plan => plan.Role).WithMany(role => role.Plans).HasForeignKey(plan => plan.RoleId);
    }
}
