using dagnys.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace dagnys.api.Data;

public class DataContext : DbContext
{
    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<RawMaterial> RawMaterials { get; set; }

    public DbSet<SupplierRawMaterial> SupplierRawMaterials { get; set; }

    // Nya DbSet
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public DataContext(DbContextOptions options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<SupplierRawMaterial>()
            .HasKey(o => new { o.RawMaterialId, o.SupplierId });

        modelBuilder
            .Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder
            .Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);
    }
}
