using APPLICATION.DOMAIN.ENTITY.CONTACT;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.CONTACT;

public class ContactTypesConfiguration : IEntityTypeConfiguration<ContactEntity>
{
    public void Configure(EntityTypeBuilder<ContactEntity> builder)
    {
        // Nomeando tabela.
        builder.ToTable("Contacts").HasKey(contact => contact.Id);

        // Guid
        builder.Property(contact => contact.Id).IsRequired();
        builder.Property(contact => contact.PersonId).IsRequired();
        builder.Property(contact => contact.CreatedUserId).IsRequired();

        // String
        builder.Property(contact => contact.CEP);
        builder.Property(contact => contact.Complement);
        builder.Property(contact => contact.Email).HasMaxLength(50);
        builder.Property(contact => contact.Name).HasMaxLength(20).IsRequired();

        // DateTime
        builder.Property(contact => contact.Created).IsRequired();
        builder.Property(contact => contact.Updated);

        // Enum
        builder.Property(contact => contact.Status).IsRequired();

        // Vinculos com pessoa
        builder
            .HasOne(contact => contact.Person).WithMany(person => person.Contacts).HasForeignKey(contact => contact.PersonId);
    }
}
