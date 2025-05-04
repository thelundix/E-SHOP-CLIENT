using dagnys.api.Converters;
using dagnys.api.Data;
using dagnys.api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connect to SQLite database
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DevConnection"));
});

// Add Controllers and custom JSON converters
builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonDateConverter());
    });

// Add DI services
builder.Services.AddScoped<IOrderService, OrderService>();

// Add API explorer + Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
        }
    );
});

var app = builder.Build();

// Run EF Core migrations and seed the database
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.LoadSuppliers(context);
    await Seed.LoadRawMaterials(context);
    await Seed.LoadSupplierRawMaterials(context);
    await Seed.LoadCustomers(context);
    await Seed.LoadProducts(context);
    await Seed.LoadOrders(context);
}
catch (Exception ex)
{
    Console.WriteLine("Error during migration/seed: {0}", ex.Message);
    throw;
}

// Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// ✅ Fix: Apply CORS before controllers
app.UseCors(MyAllowSpecificOrigins);

app.MapControllers(); // Must come after UseCors

app.Run();
