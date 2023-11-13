using CQRS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations.Entities
{
    public class PermissionTypeConfiguration : IEntityTypeConfiguration<PermissionType>
    {
        public void Configure(EntityTypeBuilder<PermissionType> builder)
        {
            builder.HasData(
                new PermissionType
                {
                    Id = 1,
                    Description = "Type 1"
                },
                new PermissionType
                {
                    Id = 2,
                    Description = "Type 2"
                }
            );
        }
    }
}
