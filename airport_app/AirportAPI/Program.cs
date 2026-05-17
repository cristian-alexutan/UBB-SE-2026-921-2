using System.Text.Json.Serialization;

using AirportAPI;
using AirportAPI.Repositories;
using AirportAPI.Repositories.Interfaces;
using AirportAPI.Services;
using AirportAPI.Services.Interfaces;

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

builder.Services.AddScoped<IAirportRepository, EfAirportRepository>();
builder.Services.AddScoped<ICompanyRepository, EfCompanyRepository>();
builder.Services.AddScoped<ICartRepository, EfCartRepository>();
builder.Services.AddScoped<IClientRepository, EfClientRepository>();
builder.Services.AddScoped<IEmployeeFlightRepository, EfEmployeeFlightRepository>();
builder.Services.AddScoped<IRouteRepository, EfRouteRepository>();
builder.Services.AddScoped<IShopItemRepository, EfShopItemRepository>();
builder.Services.AddScoped<IShopRepository, EfShopRepository>();
builder.Services.AddScoped<IReservationRepository, EfReservationRepository>();
builder.Services.AddScoped<ITicketRepository, EfTicketRepository>();
builder.Services.AddScoped<IEmployeeRepository, EfEmployeeRepository>();
builder.Services.AddScoped<IFlightRepository, EfFlightRepository>();
builder.Services.AddScoped<IGateRepository, EfGateRepository>();
builder.Services.AddScoped<IManagerRepository, EfManagerRepository>();
builder.Services.AddScoped<IRunwayRepository, EfRunwayRepository>();

builder.Services.AddScoped<IGateService, GateService>();
builder.Services.AddScoped<IAirportService, AirportService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();
