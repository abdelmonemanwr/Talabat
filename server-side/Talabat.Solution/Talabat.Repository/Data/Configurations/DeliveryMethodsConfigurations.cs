using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities.Order_Aggregate;

namespace Talabat.Repository.Layer.Data.Configurations
{
    public class DeliveryMethodsConfigurations : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(deliveryMethod => deliveryMethod.Cost).HasColumnType("decimal(18,2)");
        }
    }
}
