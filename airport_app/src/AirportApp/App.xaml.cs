using System.Text.Json;

using AirportApp.Data;
using AirportApp.Data.User;
using AirportApp.ViewModel;
using AirportApp.ViewModel.DutyFreeShops;
using AirportApp.ViewModel.DutyFreeShops.Interface;
using AirportApp.WinUI.Utils;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

namespace AirportApp
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }
        public static Window MainWindow { get; private set; }
        public static int ConfiguredUserId { get; private set; } = DefaultUserId;

        private const int DefaultUserId = 1;
        private Window window;
        private static string connectionString;

        public App()
        {
            InitializeComponent();

            try
            {
                var services = new ServiceCollection();
                ConfigureServices(services);
                Services = services.BuildServiceProvider();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DI ERROR: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"INNER: {ex.InnerException?.Message}");
                throw;
            }
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            ConfiguredUserId = ReadConfiguredUserId();

            connectionString = @"Server=(localdb)\MSSQLLocalDB; Database = AirportDB; Trusted_Connection = True; TrustServerCertificate = True;";
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Airport Management: Infrastructure
            services.AddSingleton<DatabaseConnectionFactory>();
            services.AddSingleton<MockUserUtil>();

            // Airport Management: Repositories
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IAirportRepository, AirportRepository>();
            services.AddTransient<IRunwayRepository, EfRunwayRepository>();
            services.AddTransient<IGateRepository, EfGateRepository>();
            services.AddTransient<IEmployeeRepository, EfEmployeeRepository>();
            services.AddTransient<IFlightRepository, EfFlightRepository>();
            services.AddTransient<IRouteRepository, RouteRepository>();
            services.AddTransient<IEmployeeFlightRepository, EfEmployeeFlightRepository>();

            // Airport Management: Services
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IAirportService, AirportService>();
            services.AddTransient<IRunwayService, RunwayService>();
            services.AddTransient<IGateService, GateService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IFlightRouteService, FlightRouteService>();
            services.AddTransient<IEmployeeFlightService, EmployeeFlightService>();
            services.AddTransient<IRouteService, RouteService>();

            // Airport Management: ViewModels
            services.AddTransient<SelectCompanyViewModel>();
            services.AddTransient<AirportAdminViewModel>();
            services.AddTransient<EmployeesDashboardViewModel>();
            services.AddTransient<AirportDashboardViewModel>();
            services.AddTransient<FlightsDashboardViewModel>();
            services.AddTransient<CompanyViewModel>();
            services.AddTransient<StaffPageViewModel>();
            services.AddTransient<HeaderViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<StaffLoginViewModel>();

            // Duty-Free Shops: Repositories
            services.AddSingleton<IClientRepo, EfClientRepo>();
            services.AddSingleton<ITicketRepo, EfTicketRepo>();
            services.AddSingleton<IManagerRepo, EfManagerRepo>();
            services.AddSingleton<IShopRepo, EfShopRepository>();
            services.AddSingleton<IShopItemRepo, EfShopItemRepo>();
            services.AddSingleton<ICartRepo, EfCartDbRepo>();
            services.AddScoped<IReservationRepo, ReservationDbRepo>();

            // Duty-Free Shops: Services
            services.AddSingleton<IShopItemService, ShopItemService>();
            services.AddSingleton<IShopService, ShopService>();
            services.AddSingleton<ICartService, CartService>();
            services.AddSingleton<ITicketService, TicketService>();
            services.AddSingleton<IClientService, ClientService>();
            services.AddSingleton<IManagerService, ManagerService>();
            services.AddScoped<IReservationService, ReservationService>();

            // Duty-Free Shops: Session + ViewModels
            services.AddSingleton<UserSession>();
            services.AddTransient<ILandingViewModel, LandingViewModel>();
            services.AddTransient<IShopPageViewModel, ShopPageViewModel>();
            services.AddTransient<ICartViewModel, CartViewModel>();

            services.AddSingleton<Func<Shop, IShopItemsViewModel>>(sp => shop =>
                new ShopItemsViewModel(
                    sp.GetRequiredService<IShopItemService>(),
                    sp.GetRequiredService<ICartService>(),
                    sp.GetRequiredService<UserSession>(),
                    shop));

            services.AddSingleton<Func<ShopItem, Shop, IItemDetailsViewModel>>(sp => (shopItem, shop) =>
                new ItemDetailsViewModel(
                    sp.GetRequiredService<ICartService>(),
                    sp.GetRequiredService<IShopItemService>(),
                    sp.GetRequiredService<UserSession>(),
                    shopItem,
                    shop));

            // Shell
            services.AddSingleton<INavigationUtil, NavigationUtil>();
            services.AddSingleton<MainWindow>();
        }

        private static int ReadConfiguredUserId()
        {
            string settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(settingsPath))
            {
                return DefaultUserId;
            }

            try
            {
                using JsonDocument document = JsonDocument.Parse(File.ReadAllText(settingsPath));
                if (document.RootElement.TryGetProperty("UserID", out JsonElement userIdElement) &&
                    userIdElement.TryGetInt32(out int userId))
                {
                    return userId;
                }
            }
            catch (JsonException)
            {
            }

            return DefaultUserId;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            try
            {
                window = Services.GetRequiredService<MainWindow>();
                MainWindow = window;
                window.Activate();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LAUNCH ERROR: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"INNER: {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
