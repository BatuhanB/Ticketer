using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketer.Domain.Booking.Aggregates;

namespace Ticketer.Infrastructure.Configurations;

// This is where we tell EF Core how to map our rich domain model to flat SQL tables
public class ShowInventoryConfiguration : IEntityTypeConfiguration<ShowInventory>
{
    public void Configure(EntityTypeBuilder<ShowInventory> builder)
    {
        builder.ToTable("ShowInventories");

        builder.HasKey(s => s.Id);

        // 1. CONCURRENCY: Map the Version property to a SQL Server RowVersion.
        // If two users try to update the same ShowInventory at the same time,
        // EF Core will throw a DbUpdateConcurrencyException.
        builder.Property(s => s.Version)
            .IsRowVersion();

        // 2. ENCAPSULATION: Tell EF Core to map the private _seats field, 
        // since the public Seats property is a read-only collection.
        var navigation = builder.Metadata.FindNavigation(nameof(ShowInventory.Seats));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        // 3. OWNED ENTITIES: Map the SeatAvailability collection.
        // This will create a separate "Seats" table with a foreign key back to ShowInventories.
        builder.OwnsMany(s => s.Seats, sb =>
        {
            sb.ToTable("Seats");
            sb.HasKey(seat => seat.Id);

            sb.Property(seat => seat.SeatNumber).IsRequired().HasMaxLength(10);
            sb.Property(seat => seat.Price).HasColumnType("decimal(18,2)");

            // SeatAvailability belongs strictly to the ShowInventory aggregate
            sb.WithOwner().HasForeignKey("ShowInventoryId");
        });
    }
}
