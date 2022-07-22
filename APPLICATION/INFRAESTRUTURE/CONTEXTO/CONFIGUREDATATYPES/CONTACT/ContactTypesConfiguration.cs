using APPLICATION.DOMAIN.ENTITY.CONTACT;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.CONTACT;

public class ContactTypesConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts").HasKey(contact => contact.Id);

        builder
            .HasOne(contact => contact.Person).WithMany(person => person.Contacts).HasForeignKey(contact => contact.PersonId);
    }
}
