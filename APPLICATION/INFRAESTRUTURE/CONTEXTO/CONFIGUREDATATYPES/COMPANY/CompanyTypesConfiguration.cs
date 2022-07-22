using APPLICATION.DOMAIN.ENTITY.COMPANY;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.INFRAESTRUTURE.CONTEXTO.CONFIGUREDATATYPES.COMPANY;

public class CompanyTypesConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies").HasKey(company => company.Id);

        builder.HasMany(company => company.Professions).WithOne(profession => profession.Company).HasForeignKey(profession => profession.CompanyId);
    }
}
