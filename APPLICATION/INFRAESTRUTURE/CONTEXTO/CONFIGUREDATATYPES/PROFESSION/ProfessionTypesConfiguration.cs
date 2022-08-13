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

        // Vinculando Profession com pessoas e pessoas com profession.
        builder
           .HasOne(profession => profession.Person).WithMany(person => person.Professions).HasForeignKey(profession => profession.PersonId);

        // Vinculando Profession com Company e Company com profession.
        builder
            .HasOne(profession => profession.Company).WithMany(company => company.Professions).HasForeignKey(profession => profession.CompanyId);
    }
}
