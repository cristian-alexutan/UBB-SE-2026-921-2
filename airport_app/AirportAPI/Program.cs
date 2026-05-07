using System.Text.Json.Serialization;

using AirportAPI;
using AirportAPI.Repositories;
using AirportAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAirportRepository, AirportRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICartRepo, EfCartDbRepo>();
builder.Services.AddScoped<IClientRepo, EfClientRepo>();
builder.Services.AddScoped<IEmployeeFlightRepository, EfEmployeeFlightRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IShopItemRepo, EfShopItemRepo>();
builder.Services.AddScoped<IShopRepo, EfShopRepository>();
builder.Services.AddScoped<IReservationRepo, ReservationRepository>();
builder.Services.AddScoped<ITicketRepo, EfTicketRepo>();
builder.Services.AddScoped<IEmployeeRepository, EfEmployeeRepository>();
builder.Services.AddScoped<IFlightRepository, EfFlightRepository>();
builder.Services.AddScoped<IGateRepository, EfGateRepository>();
builder.Services.AddScoped<IManagerRepo, EfManagerRepo>();
builder.Services.AddScoped<IRunwayRepository, EfRunwayRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();
