using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Configuration

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var configuration = builder.Configuration;


//DbContext
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


//Repositories

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();

/*
using (var scope = app.Services.CreateScope())
{


    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    var dbInitializer = new DbInitializer(context);
    await dbInitializer.InitializeAsync();

}
*/

    app.Run();
