using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities.Order_Aggregate;

namespace Talabat.Repository.Layer.Data.Configrations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            //// OwnsOne, WithOwner : used to map the Address object to be columns in the Order table
            builder.OwnsOne(order => order.ShipToAddress, np => np.WithOwner());

            builder.Property(order => order.Status)
                   .HasConversion(
                       order => order.ToString(), 
                       o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
                   );

            builder.HasMany(o=>o.OrderItems).WithOne()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(order => order.Subtotal).HasColumnType("decimal(18,2)");

            //builder.HasOne(order => order.DeliveryMethod).WithOne();

            //builder.Property(order => order.BuyerEmail).IsRequired();

            //builder.Property(order => order.OrderDate).IsRequired().HasDefaultValue(DateTimeOffset.Now);

            //// Computed column for the Subtotal property in the Order table
            //builder.Property(order => order.Subtotal).HasColumnType("decimal(18,2)")
            //    .HasComputedColumnSql("SUM([OrderItems].[Price] * [OrderItems].[Quantity])");

            //builder.Property(order => order.PaymentIntentId).IsRequired(false);
        }
    }
}