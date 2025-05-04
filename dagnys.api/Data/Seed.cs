using System.Text.Json;
using dagnys.api.Entities;

namespace dagnys.api.Data;

public static class Seed
{
    public static async Task LoadSuppliers(DataContext context)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        if (context.Suppliers.Any())
            return;

        var json = File.ReadAllText("Data/json/suppliers.json");
        var suppliers = JsonSerializer.Deserialize<List<Supplier>>(json, options);

        if (suppliers is not null && suppliers.Count > 0)
        {
            await context.Suppliers.AddRangeAsync(suppliers);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadRawMaterials(DataContext context)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        if (context.RawMaterials.Any())
            return;

        var json = File.ReadAllText("Data/json/rawmaterials.json");
        var rawmaterials = JsonSerializer.Deserialize<List<RawMaterial>>(json, options);

        if (rawmaterials is not null && rawmaterials.Count > 0)
        {
            await context.RawMaterials.AddRangeAsync(rawmaterials);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadSupplierRawMaterials(DataContext context)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        if (context.SupplierRawMaterials.Any())
            return;

        var json = File.ReadAllText("Data/json/supplierrawmaterials.json");
        var supplierrawmaterials = JsonSerializer.Deserialize<List<SupplierRawMaterial>>(
            json,
            options
        );

        if (supplierrawmaterials is not null && supplierrawmaterials.Count > 0)
        {
            await context.SupplierRawMaterials.AddRangeAsync(supplierrawmaterials);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadCustomers(DataContext context)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        if (context.Customers.Any())
            return;

        var json = File.ReadAllText("Data/json/customers.json");
        var customers = JsonSerializer.Deserialize<List<Customer>>(json, options);

        if (customers is not null && customers.Count > 0)
        {
            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadProducts(DataContext context)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        if (context.Products.Any())
            return;

        var json = File.ReadAllText("Data/json/products.json");
        var products = JsonSerializer.Deserialize<List<Product>>(json, options);

        if (products is not null && products.Count > 0)
        {
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadOrders(DataContext context)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        if (context.Orders.Any())
            return;

        var json = File.ReadAllText("Data/json/orders.json");
        var orders = JsonSerializer.Deserialize<List<Order>>(json, options);

        if (orders is not null && orders.Count > 0)
        {
            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync();
        }
    }
}
