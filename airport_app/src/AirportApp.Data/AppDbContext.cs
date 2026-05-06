using Microsoft.EntityFrameworkCore;

namespace AirportApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Airport> Airports { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeFlight> EmployeeFlights { get; set; }
    public DbSet<EmployeeScheduleItem> EmployeeScheduleItems { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Gate> Gates { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Route> Routes { get; set; }
    public DbSet<Runway> Runways { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<ShopItem> ShopItems { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeFlight>().HasKey("EmployeeId", "FlightId");
        modelBuilder.Entity<EmployeeFlight>().HasOne(employeeFlight => employeeFlight.Employee).WithMany().HasForeignKey("EmployeeId");
        modelBuilder.Entity<EmployeeFlight>().HasOne(employeeFlight => employeeFlight.Flight).WithMany().HasForeignKey("FlightId");

        modelBuilder.Entity<Flight>().ToTable("Flights");
        modelBuilder.Entity<Flight>().Property(f => f.Id).HasColumnName("id");
        modelBuilder.Entity<Flight>().Property(f => f.Date).HasColumnName("date");
        modelBuilder.Entity<Flight>().Property(f => f.FlightNumber).HasColumnName("flight_number");
        modelBuilder.Entity<Flight>().HasOne(f => f.Route).WithMany().HasForeignKey("RouteId");
        modelBuilder.Entity<Flight>().Property<int>("RouteId").HasColumnName("route_id");
        modelBuilder.Entity<Flight>().HasOne(f => f.Runway).WithMany().HasForeignKey("RunwayId");
        modelBuilder.Entity<Flight>().Property<int>("RunwayId").HasColumnName("runway_id");
        modelBuilder.Entity<Flight>().HasOne(f => f.Gate).WithMany().HasForeignKey("GateId").OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Flight>().Property<int>("GateId").HasColumnName("gate_id");

        modelBuilder.Entity<Route>().HasOne(route => route.Company).WithMany().HasForeignKey("CompanyId");
        modelBuilder.Entity<Route>().HasOne(route => route.Airport).WithMany().HasForeignKey("AirportId");

        modelBuilder.Entity<Shop>().HasOne(shop => shop.Manager).WithMany().HasForeignKey("ManagerId");

        modelBuilder.Entity<ShopItem>().HasOne(shopItem => shopItem.Shop).WithMany().HasForeignKey("ShopId");

        modelBuilder.Entity<Cart>().HasOne(cart => cart.Client).WithMany().HasForeignKey("ClientId");

        modelBuilder.Entity<CartItem>().HasOne(cartItem => cartItem.ShopItem).WithMany().HasForeignKey("ShopItemId");
        modelBuilder.Entity<CartItem>().HasOne<Cart>().WithMany().HasForeignKey("CartId");

        modelBuilder.Entity<Reservation>().HasOne(reservation => reservation.ReservationCart).WithMany().HasForeignKey("CartId");
    }
}
