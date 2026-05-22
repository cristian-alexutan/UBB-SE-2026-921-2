using Microsoft.EntityFrameworkCore;

using Route = AirportLib.Domain.Domain.Route;

namespace AirportAPI;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public AppDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=AirportDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    public DbSet<Airport> Airports { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeFlight> EmployeeFlights { get; set; }
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
        modelBuilder.Entity<EmployeeFlight>()
            .HasKey("EmployeeId", "FlightId");
        modelBuilder.Entity<EmployeeFlight>()
            .HasOne(employeeFlight => employeeFlight.Employee)
            .WithMany()
            .HasForeignKey("EmployeeId")
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<EmployeeFlight>()
            .HasOne(employeeFlight => employeeFlight.Flight)
            .WithMany()
            .HasForeignKey("FlightId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Flight>()
            .HasOne(flight => flight.Route)
            .WithMany()
            .HasForeignKey("RouteId")
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Flight>()
            .HasOne(flight => flight.Runway)
            .WithMany()
            .HasForeignKey("RunwayId")
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Flight>()
            .HasOne(flight => flight.Gate)
            .WithMany()
            .HasForeignKey("GateId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Route>()
            .HasOne(route => route.Airport)
            .WithMany()
            .HasForeignKey("AirportId")
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Route>()
            .HasOne(route => route.Company)
            .WithMany()
            .HasForeignKey("CompanyId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Shop>()
            .HasOne(shop => shop.Manager)
            .WithMany()
            .HasForeignKey("ManagerId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ShopItem>()
            .HasOne(shopItem => shopItem.Shop)
            .WithMany()
            .HasForeignKey("ShopId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Cart>()
            .HasOne(cart => cart.Client)
            .WithMany()
            .HasForeignKey("ClientId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartItem>()
            .HasOne(cartItem => cartItem.ShopItem)
            .WithMany()
            .HasForeignKey("ShopItemId")
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<CartItem>()
            .HasOne<Cart>()
            .WithMany(cart => cart.CartItems)
            .HasForeignKey("CartId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reservation>()
            .HasOne(reservation => reservation.ReservationCart)
            .WithMany()
            .HasForeignKey("CartId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Manager>().HasData(
            new Manager(1, "Marcel", "marcel@gmail.com", "4074593789"));

        modelBuilder.Entity<Shop>().HasData(
            new { Id = 1, Name = "Sky Bites", Type = "Food & Beverage", ManagerId = 1 },
            new { Id = 2, Name = "Runway Cafe", Type = "Coffee Shop", ManagerId = 1 },
            new { Id = 3, Name = "Elite Boutique", Type = "Luxury Goods", ManagerId = 1 },
            new { Id = 4, Name = "FlySmart Store", Type = "Travel Essentials", ManagerId = 1 });

        modelBuilder.Entity<ShopItem>().HasData(
            new { Id = 1, Name = "Chicken Sandwich", Description = "Fresh sandwich", Price = 12.5f, Quantity = 98, Photo = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd", ShopId = 1 },
            new { Id = 2, Name = "Caesar Salad", Description = "Healthy salad", Price = 8.0f, Quantity = 77, Photo = "https://images.unsplash.com/photo-1551248429-40975aa4de74", ShopId = 1 },
            new { Id = 3, Name = "Orange Juice", Description = "Fresh juice", Price = 5.5f, Quantity = 60, Photo = "https://images.unsplash.com/photo-1621506289937-a8e4df240d0b", ShopId = 1 },
            new { Id = 4, Name = "Espresso", Description = "Strong coffee", Price = 3.5f, Quantity = 200, Photo = "https://images.unsplash.com/photo-1511920170033-f8396924c348", ShopId = 2 },
            new { Id = 5, Name = "Cappuccino", Description = "Coffee with foam", Price = 4.5f, Quantity = 150, Photo = "https://images.unsplash.com/photo-1509042239860-f550ce710b93", ShopId = 2 },
            new { Id = 6, Name = "Latte", Description = "Smooth milk coffee", Price = 6.0f, Quantity = 100, Photo = "https://images.unsplash.com/photo-1523942839745-7848d0f5c9d1", ShopId = 2 },
            new { Id = 7, Name = "Luxury Watch", Description = "High-end watch", Price = 500.0f, Quantity = 20, Photo = "https://images.unsplash.com/photo-1523275335684-37898b6baf30", ShopId = 3 },
            new { Id = 8, Name = "Designer Handbag", Description = "Premium leather bag", Price = 1200.0f, Quantity = 15, Photo = "https://images.unsplash.com/photo-1584917865442-de89df76afd3", ShopId = 3 },
            new { Id = 9, Name = "RayBan Sunglasses", Description = "Stylish sunglasses", Price = 300.0f, Quantity = 10, Photo = "https://images.unsplash.com/photo-1511499767150-a48a237f0083", ShopId = 3 },
            new { Id = 10, Name = "Neck Pillow", Description = "Travel pillow", Price = 25.0f, Quantity = 120, Photo = "https://images.unsplash.com/photo-1540497077202-7c8a3999166f", ShopId = 4 },
            new { Id = 11, Name = "Travel Adapter", Description = "Universal plug adapter", Price = 10.0f, Quantity = 205, Photo = "https://images.unsplash.com/photo-1572635196237-14b3f281503f", ShopId = 4 },
            new { Id = 12, Name = "Power Bank", Description = "Portable charger", Price = 15.0f, Quantity = 90, Photo = "https://images.unsplash.com/photo-1609592424060-bd0c5b305c91", ShopId = 4 });

        modelBuilder.Entity<Client>().HasData(
            new Client(1, "Crina"));

        modelBuilder.Entity<Cart>().HasData(
            new { Id = 1, ClientId = 1 });

        modelBuilder.Entity<CartItem>().HasData(
            new { Id = 1, Quantity = 2, ShopItemId = 1, CartId = 1 });

        modelBuilder.Entity<Ticket>().HasData(
            new Ticket(1, "Duty Free Shops", "Global Duty Free"),
            new Ticket(2, "Duty Free Shops", "Sky Bites"),
            new Ticket(3, "Duty Free Shops", "Runway Cafe"),
            new Ticket(4, "Duty Free Shops", "Elite Boutique"),
            new Ticket(5, "Duty Free Shops", "FlySmart Store"));

        modelBuilder.Entity<Company>().HasData(
            new Company { Id = 1, Name = "WizzAir" },
            new Company { Id = 2, Name = "Lufthansa" });

        modelBuilder.Entity<Airport>().HasData(
            new Airport { Id = 1, City = "London", Name = "London Luton Airport", Code = "LTN" },
            new Airport { Id = 2, City = "Munich", Name = "Munich Airport", Code = "MUC" },
            new Airport { Id = 3, City = "Cluj-Napoca", Name = "Cluj International Airport", Code = "CLJ" });

        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, Name = "Andrei Popescu", Role = EmployeeRole.Pilot, Birthday = new DateOnly(1990, 5, 12), Salary = 12000, HiringDate = new DateOnly(2021, 3, 1) },
            new Employee { Id = 2, Name = "Maria Ionescu", Role = EmployeeRole.FlightAttendant, Birthday = new DateOnly(1995, 9, 20), Salary = 7000, HiringDate = new DateOnly(2022, 6, 15) },
            new Employee { Id = 3, Name = "Vlad Georgescu", Role = EmployeeRole.CoPilot, Birthday = new DateOnly(1988, 11, 3), Salary = 10000, HiringDate = new DateOnly(2020, 1, 10) },
            new Employee { Id = 4, Name = "Elena Dumitrescu", Role = EmployeeRole.FlightAttendant, Birthday = new DateOnly(1997, 2, 14), Salary = 6800, HiringDate = new DateOnly(2023, 4, 5) });

        modelBuilder.Entity<Runway>().HasData(
            new Runway { Id = 1, Name = "Runway A1", HandleTime = 15 },
            new Runway { Id = 2, Name = "Runway B2", HandleTime = 20 },
            new Runway { Id = 3, Name = "Runway C3", HandleTime = 18 });

        modelBuilder.Entity<Gate>().HasData(
            new Gate { Id = 1, Name = "Gate 1" },
            new Gate { Id = 2, Name = "Gate 2" },
            new Gate { Id = 3, Name = "Gate 3" },
            new Gate { Id = 4, Name = "Gate 4" });

        modelBuilder.Entity<Route>().HasData(
            new { Id = 1, RouteType = "DEP", CompanyId = 1, AirportId = 1, RecurrenceInterval = 1, StartDate = new DateOnly(2026, 3, 1), EndDate = new DateOnly(2026, 12, 31), DepartureTime = new TimeOnly(8, 30), ArrivalTime = new TimeOnly(10, 45), Capacity = 180 },
            new { Id = 2, RouteType = "ARR", CompanyId = 1, AirportId = 1, RecurrenceInterval = 1, StartDate = new DateOnly(2026, 3, 1), EndDate = new DateOnly(2026, 12, 31), DepartureTime = new TimeOnly(11, 30), ArrivalTime = new TimeOnly(13, 40), Capacity = 180 },
            new { Id = 3, RouteType = "DEP", CompanyId = 2, AirportId = 2, RecurrenceInterval = 2, StartDate = new DateOnly(2026, 3, 1), EndDate = new DateOnly(2026, 12, 31), DepartureTime = new TimeOnly(14, 0), ArrivalTime = new TimeOnly(15, 20), Capacity = 160 },
            new { Id = 4, RouteType = "ARR", CompanyId = 2, AirportId = 2, RecurrenceInterval = 2, StartDate = new DateOnly(2026, 3, 1), EndDate = new DateOnly(2026, 12, 31), DepartureTime = new TimeOnly(16, 0), ArrivalTime = new TimeOnly(17, 25), Capacity = 160 });

        modelBuilder.Entity<Flight>().HasData(
            new { Id = 1, Date = new DateTime(2026, 3, 27, 8, 30, 0), FlightNumber = "W6 3401", RouteId = 1, RunwayId = 1, GateId = 1 },
            new { Id = 2, Date = new DateTime(2026, 3, 27, 13, 40, 0), FlightNumber = "W6 3402", RouteId = 2, RunwayId = 2, GateId = 2 },
            new { Id = 3, Date = new DateTime(2026, 3, 27, 14, 0, 0), FlightNumber = "LH 1671", RouteId = 3, RunwayId = 3, GateId = 3 },
            new { Id = 4, Date = new DateTime(2026, 3, 27, 17, 25, 0), FlightNumber = "LH 1672", RouteId = 4, RunwayId = 1, GateId = 4 },
            new { Id = 5, Date = new DateTime(2026, 3, 28, 8, 30, 0), FlightNumber = "W6 3403", RouteId = 1, RunwayId = 2, GateId = 1 });

        modelBuilder.Entity<EmployeeFlight>().HasData(
            new { EmployeeId = 1, FlightId = 1 },
            new { EmployeeId = 1, FlightId = 2 },
            new { EmployeeId = 1, FlightId = 5 },
            new { EmployeeId = 2, FlightId = 1 },
            new { EmployeeId = 2, FlightId = 2 },
            new { EmployeeId = 2, FlightId = 3 },
            new { EmployeeId = 3, FlightId = 1 },
            new { EmployeeId = 3, FlightId = 3 },
            new { EmployeeId = 3, FlightId = 4 },
            new { EmployeeId = 4, FlightId = 4 },
            new { EmployeeId = 4, FlightId = 5 });
    }
}

