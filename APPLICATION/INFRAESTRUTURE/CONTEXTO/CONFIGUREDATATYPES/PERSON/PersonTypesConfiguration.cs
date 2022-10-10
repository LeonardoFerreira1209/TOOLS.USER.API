using APPLICATION.DOMAIN.ENTITY.PERSON;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.PERSON;

public class PersonTypesConfiguration : IEntityTypeConfiguration<PersonEntity>
{
    public void Configure(EntityTypeBuilder<PersonEntity> builder)
    {
        // Nomeando a tabela
        builder.ToTable("Persons").HasKey(person => person.Id);

        // Guid
        builder.Property(person => person.Id).IsRequired();
        builder.Property(person => person.UserId).IsRequired();
        builder.Property(person => person.CreatedUserId).IsRequired();
        builder.Property(person => person.UpdatedUserId);

        // String
        builder.Property(person => person.FirstName).HasMaxLength(20).IsRequired();
        builder.Property(person => person.LastName).HasMaxLength(20).IsRequired();
        builder.Property(person => person.RG).IsRequired();
        builder.Property(person => person.CPF).IsRequired();

        // Bytes
        builder.Property(person => person.Image);

        // Integer
        builder.Property(person => person.Age).IsRequired();

        // Datetime
        builder.Property(person => person.Created).IsRequired();
        builder.Property(person => person.Updated).IsRequired();
        builder.Property(person => person.BirthDay).IsRequired();

        // Enum
        builder.Property(person => person.Status).IsRequired();
        builder.Property(person => person.Gender).IsRequired();

        // Vinculo de pessoa com usuário.
        builder.
            HasOne(person => person.User);

        // Vinculos com profissões.
        builder
            .HasMany(person => person.Professions).WithOne(profession => profession.Person).HasForeignKey(profession => profession.PersonId);

        // Vinculos com contatos.
        builder
            .HasMany(person => person.Contacts).WithOne(contact => contact.Person).HasForeignKey(contact => contact.PersonId);
    }
}
