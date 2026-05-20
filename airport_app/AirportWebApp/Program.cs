using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5171/")
});

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddSingleton<WebUserSession>();

builder.Services.AddSingleton<IAirportService, AirportServiceProxy>();
builder.Services.AddSingleton<ICompanyService, CompanyServiceProxy>();
builder.Services.AddSingleton<IRunwayService, RunwayServiceProxy>();
builder.Services.AddSingleton<IGateService, GateServiceProxy>();
builder.Services.AddSingleton<IEmployeeService, EmployeeServiceProxy>();
builder.Services.AddSingleton<IFlightService, FlightServiceProxy>();
builder.Services.AddSingleton<IRouteService, RouteServiceProxy>();
builder.Services.AddSingleton<IFlightRouteService, FlightRouteServiceProxy>();
builder.Services.AddSingleton<IEmployeeFlightService, EmployeeFlightServiceProxy>();
builder.Services.AddSingleton<IClientService, ClientServiceProxy>();
builder.Services.AddSingleton<IManagerService, ManagerServiceProxy>();
builder.Services.AddSingleton<IShopService, ShopServiceProxy>();
builder.Services.AddSingleton<IShopItemService, ShopItemServiceProxy>();
builder.Services.AddSingleton<ICartService, CartServiceProxy>();
builder.Services.AddSingleton<IReservationService, ReservationServiceProxy>();
builder.Services.AddSingleton<ITicketService, TicketServiceProxy>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllers();

app.Run();
