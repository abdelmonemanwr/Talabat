using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;
using Talabat.Domain.Layer.Entities.Order_Aggregate;
using Talabat.Repository.Layer.Data.Configrations;

namespace Talabat.Repository.Layer.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options): base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add Configrations of Product Fluent API
            //modelBuilder.ApplyConfiguration(new ProductConfigrations());

            // Add all classes that implement interface: IEntityTypeConfiguration<TEntity>, these classes contain Fluent API configrations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        #region DbSets
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<ProductBrand> ProductBrands { get; set; }

        public DbSet<SeedInfo> SeedInfo { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        #endregion
    }
}
