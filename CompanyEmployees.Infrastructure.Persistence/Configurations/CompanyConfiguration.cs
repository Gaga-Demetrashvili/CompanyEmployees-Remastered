using CompanyEmployees.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyEmployees.Infrastructure.Persistence.Configurations;

// If you want to have cleaner models, without those Required, MaxLength,  and ForeignKey attributes
// you can extract the configuration in separate files using the Fluent API approach.

// Here we have initial configuration for creating our tables and columns with relationships.
// IsRequired() method makes the property required (and the column as not null in the database).

// So, this is how you can extract the configuration logic from the model classes. First,
// you would create the initial configuration for both entities, create migrations, and update the database.
// Then, you could add the seeding logic in both classes, create additional migrations, and update the database again.
// You would get the same result as you have now.
// Of course, if we decided to use this approach from the start, we wouldn’t need all those attributes inside the model classes, just clean properties.
public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies")
            .HasKey(c => c.Id);

        // builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("CompanyId")
            .IsRequired();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(c => c.Country);

        builder.HasMany(c => c.Employees)
            .WithOne(e => e.Company)
            .HasForeignKey(e => e.CompanyId)
            // Configures a cascade delete behavior in Entity Framework Core.
            // This means that if a Company entity is deleted, all related Employee entities
            // (those with a CompanyId foreign key pointing to the deleted Company) will also be automatically deleted from the database.
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData
        (
            new Company
            {
                Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                Name = "IT_Solutions Ltd",
                Address = "583 Wall Dr. Gwynn Oak, MD 21207",
                Country = "USA"
            },
            new Company
            {
                Id = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                Name = "Admin_Solutions Ltd",
                Address = "312 Forest Avenue, BF 923",
                Country = "USA"
            }
        );
    }
}