using APPLICATION.DOMAIN.ENTITY.PROFESSION;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.PROFESSION;

/// <summary>
/// Classe responsável por ajudar as entidades.
/// </summary>
public class ProfessionTypesConfiguration : IEntityTypeConfiguration<Profession>
{
    public void Configure(EntityTypeBuilder<Profession> builder)
    {
        // Ajustando nome da tabela e definindo chave primaria
        builder.ToTable("Professions").HasKey(profession => profession.Id);

        // Guid
        builder.Property(profession => profession.Id).IsRequired();
        builder.Property(profession => profession.PersonId).IsRequired();
        builder.Property(profession => profession.CompanyId);

        // String
        builder.Property(profession => profession.Description).HasMaxLength(200);
        builder.Property(profession => profession.Office).HasMaxLength(50).IsRequired();

        // Enum
        builder.Property(profession => profession.Status).IsRequired();

        // Booleam
        builder.Property(profession => profession.Current).HasDefaultValue(false);

        // Decimal
        builder.Property(profession => profession.Wage);
        builder.Property(profession => profession.Workload).HasMaxLength(4);

        // DateTime
        builder.Property(profession => profession.Created).IsRequired();
        builder.Property(profession => profession.StartDate).IsRequired();
        builder.Property(profession => profession.EndDate).IsRequired();

        // Vinculando Profession com pessoas e pessoas com profession.
        builder
           .HasOne(profession => profession.Person).WithMany(person => person.Professions).HasForeignKey(profession => profession.PersonId);

        // Vinculando Profession com Company e Company com profession.
        builder
            .HasOne(profession => profession.Company).WithMany(company => company.Professions).HasForeignKey(profession => profession.CompanyId);
    }
}
