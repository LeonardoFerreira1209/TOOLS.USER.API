using APPLICATION.DOMAIN.ENTITY.PERSON;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.PERSON;

public class PersonTypesConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {

        builder.ToTable("Persons").HasKey(person => person.Id);

        builder.
            HasOne(person => person.User);

        builder
            .HasMany(person => person.Professions).WithOne(profession => profession.Person).HasForeignKey(profession => profession.PersonId);

        builder
            .HasMany(person => person.Contacts).WithOne(contact => contact.Person).HasForeignKey(contact => contact.PersonId);
    }
}
