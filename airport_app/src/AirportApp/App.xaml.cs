using System.Text.Json;

using AirportApp.Data.Repositories.Proxies;
using AirportApp.Data.Services.Proxies;
using AirportApp.Data.User;
using AirportApp.ViewModel;
using AirportApp.ViewModel.DutyFreeShops;
using AirportApp.ViewModel.DutyFreeShops.Interface;
using AirportApp.WinUI.Utils;

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
        private const string DefaultApiBaseUrl = "http://localhost:5171/";
        private Window window;

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
            services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri(ReadConfiguredApiBaseUrl())
            });

            // Airport Management: Infrastructure
            services.AddSingleton<MockUserUtil>();

            // Airport Management: Services
            services.AddTransient<ICompanyService, CompanyServiceProxy>();
            services.AddTransient<IAirportService, AirportServiceProxy>();
            services.AddTransient<IRunwayService, RunwayServiceProxy>();
            services.AddTransient<IGateService, GateServiceProxy>();
            services.AddTransient<IEmployeeService, EmployeeServiceProxy>();
            services.AddTransient<IFlightRouteService, FlightRouteServiceProxy>();
            services.AddTransient<IEmployeeFlightService, EmployeeFlightServiceProxy>();
            services.AddTransient<IRouteService, RouteServiceProxy>();
            services.AddTransient<IFlightService, FlightServiceProxy>();

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

            // Duty-Free Shops: Services
            services.AddSingleton<IShopItemService, ShopItemServiceProxy>();
            services.AddSingleton<IShopService, ShopServiceProxy>();
            services.AddSingleton<ICartService, CartServiceProxy>();
            services.AddSingleton<ITicketService, TicketServiceProxy>();
            services.AddSingleton<IClientService, ClientServiceProxy>();
            services.AddSingleton<IManagerService, ManagerServiceProxy>();
            services.AddScoped<IReservationService, ReservationServiceProxy>();

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

        private static string ReadConfiguredApiBaseUrl()
        {
            string settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(settingsPath))
            {
                return DefaultApiBaseUrl;
            }

            try
            {
                using JsonDocument document = JsonDocument.Parse(File.ReadAllText(settingsPath));
                if (document.RootElement.TryGetProperty("ApiBaseUrl", out JsonElement apiBaseUrlElement))
                {
                    string? apiBaseUrl = apiBaseUrlElement.GetString();
                    if (!string.IsNullOrWhiteSpace(apiBaseUrl))
                    {
                        return apiBaseUrl;
                    }
                }
            }
            catch (JsonException)
            {
            }

            return DefaultApiBaseUrl;
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
