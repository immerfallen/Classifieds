using Classifieds.Data.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Classifieds.Data.Configuration
{
    class RoleSeedConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData( 
                new IdentityRole
                {
                    Id = "cac43a6e-f7bb-4448-baff-1add431ccbbf",
                    Name = Role.Customer,
                    NormalizedName = Role.Customer.ToUpper()

                },
                 new IdentityRole
                 {
                     Id = "cbc43a8e-f7bb-4445-baaf-1add431ffbbf",
                     Name = Role.Administrator,
                     NormalizedName = Role.Administrator.ToUpper()

                 }

        );
        }
    }
}
