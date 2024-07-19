using Microsoft.EntityFrameworkCore;
using WebDockerAPI;
using WebDockerAPI.Data;
using WebDockerAPI.Interface;
using WebDockerAPI.Repository;

bool islocal = false;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if(islocal)
{
    builder.Services.AddDbContext<DataContext>(options =>
    {
        //options.UseSqlServer(defaultsqlconnection);
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    builder.Services.AddTransient<Seed>();
}
else
{
    string host = Environment.GetEnvironmentVariable("DB_HOST");
    string dbname = Environment.GetEnvironmentVariable("DB_NAME");
    string baseport = Environment.GetEnvironmentVariable("DB_BASEPORT");
    string dbuser = Environment.GetEnvironmentVariable("DB_USER");
    string password = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
    var defaultsqlconnection = $"Server={host},{baseport};Initial Catalog={dbname};User ID={dbuser};Password={password};Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

    builder.Services.AddDbContext<DataContext>(options =>
    {
        options.UseSqlServer(defaultsqlconnection);
        //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

}

builder.Services.AddScoped<ICountryRepo, CountryRepo>();

var app = builder.Build();

if(islocal)
{
    if (args.Length == 1 && args[0].ToLower() == "seeddata")
        SeedData(app);

    void SeedData(IHost app)
    {
        var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

        using (var scope = scopedFactory.CreateScope())
        {
            var service = scope.ServiceProvider.GetService<Seed>();
            service.SeedDataContext();
        }
    }
}
else
{
    DatabaseManagementService.MigrationInitialize(app);
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
