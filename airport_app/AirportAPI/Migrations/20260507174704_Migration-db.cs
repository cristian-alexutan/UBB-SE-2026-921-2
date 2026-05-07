using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AirportAPI.Migrations
{
    /// <inheritdoc />
    public partial class Migrationdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: false),
                    HiringDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Salary = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Runways",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HandleTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runways", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subcategory = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecurrenceInterval = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DepartureTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    ArrivalTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    AirportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routes_Airports_AirportId",
                        column: x => x.AirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Routes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shops_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    RunwayId = table.Column<int>(type: "int", nullable: false),
                    GateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flights_Gates_GateId",
                        column: x => x.GateId,
                        principalTable: "Gates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flights_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flights_Runways_RunwayId",
                        column: x => x.RunwayId,
                        principalTable: "Runways",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    ShopId = table.Column<int>(type: "int", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopItems_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeFlights",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    FlightId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeFlights", x => new { x.EmployeeId, x.FlightId });
                    table.ForeignKey(
                        name: "FK_EmployeeFlights_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeFlights_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_ShopItems_ShopItemId",
                        column: x => x.ShopItemId,
                        principalTable: "ShopItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Airports",
                columns: new[] { "Id", "City", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "London", "LTN", "London Luton Airport" },
                    { 2, "Munich", "MUC", "Munich Airport" },
                    { 3, "Cluj-Napoca", "CLJ", "Cluj International Airport" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Crina" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "WizzAir" },
                    { 2, "Lufthansa" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Birthday", "HiringDate", "Name", "Role", "Salary" },
                values: new object[,]
                {
                    { 1, new DateOnly(1990, 5, 12), new DateOnly(2021, 3, 1), "Andrei Popescu", 1, 12000 },
                    { 2, new DateOnly(1995, 9, 20), new DateOnly(2022, 6, 15), "Maria Ionescu", 3, 7000 },
                    { 3, new DateOnly(1988, 11, 3), new DateOnly(2020, 1, 10), "Vlad Georgescu", 2, 10000 },
                    { 4, new DateOnly(1997, 2, 14), new DateOnly(2023, 4, 5), "Elena Dumitrescu", 3, 6800 }
                });

            migrationBuilder.InsertData(
                table: "Gates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Gate 1" },
                    { 2, "Gate 2" },
                    { 3, "Gate 3" },
                    { 4, "Gate 4" }
                });

            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "Id", "Email", "Name", "Phone" },
                values: new object[] { 1, "marcel@gmail.com", "Marcel", "4074593789" });

            migrationBuilder.InsertData(
                table: "Runways",
                columns: new[] { "Id", "HandleTime", "Name" },
                values: new object[,]
                {
                    { 1, 15, "Runway A1" },
                    { 2, 20, "Runway B2" },
                    { 3, 18, "Runway C3" }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "Category", "Subcategory" },
                values: new object[,]
                {
                    { 1, "Duty Free Shops", "Global Duty Free" },
                    { 2, "Duty Free Shops", "Sky Bites" },
                    { 3, "Duty Free Shops", "Runway Cafe" },
                    { 4, "Duty Free Shops", "Elite Boutique" },
                    { 5, "Duty Free Shops", "FlySmart Store" }
                });

            migrationBuilder.InsertData(
                table: "Carts",
                columns: new[] { "Id", "ClientId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "Id", "AirportId", "ArrivalTime", "Capacity", "CompanyId", "DepartureTime", "EndDate", "RecurrenceInterval", "RouteType", "StartDate" },
                values: new object[,]
                {
                    { 1, 1, new TimeOnly(10, 45, 0), 180, 1, new TimeOnly(8, 30, 0), new DateOnly(2026, 12, 31), 1, "DEP", new DateOnly(2026, 3, 1) },
                    { 2, 1, new TimeOnly(13, 40, 0), 180, 1, new TimeOnly(11, 30, 0), new DateOnly(2026, 12, 31), 1, "ARR", new DateOnly(2026, 3, 1) },
                    { 3, 2, new TimeOnly(15, 20, 0), 160, 2, new TimeOnly(14, 0, 0), new DateOnly(2026, 12, 31), 2, "DEP", new DateOnly(2026, 3, 1) },
                    { 4, 2, new TimeOnly(17, 25, 0), 160, 2, new TimeOnly(16, 0, 0), new DateOnly(2026, 12, 31), 2, "ARR", new DateOnly(2026, 3, 1) }
                });

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "ManagerId", "Name", "Type" },
                values: new object[,]
                {
                    { 1, 1, "Sky Bites", "Food & Beverage" },
                    { 2, 1, "Runway Cafe", "Coffee Shop" },
                    { 3, 1, "Elite Boutique", "Luxury Goods" },
                    { 4, 1, "FlySmart Store", "Travel Essentials" }
                });

            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "Id", "Date", "FlightNumber", "GateId", "RouteId", "RunwayId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 27, 8, 30, 0, 0, DateTimeKind.Unspecified), "W6 3401", 1, 1, 1 },
                    { 2, new DateTime(2026, 3, 27, 13, 40, 0, 0, DateTimeKind.Unspecified), "W6 3402", 2, 2, 2 },
                    { 3, new DateTime(2026, 3, 27, 14, 0, 0, 0, DateTimeKind.Unspecified), "LH 1671", 3, 3, 3 },
                    { 4, new DateTime(2026, 3, 27, 17, 25, 0, 0, DateTimeKind.Unspecified), "LH 1672", 4, 4, 1 },
                    { 5, new DateTime(2026, 3, 28, 8, 30, 0, 0, DateTimeKind.Unspecified), "W6 3403", 1, 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "ShopItems",
                columns: new[] { "Id", "Description", "Name", "Photo", "Price", "Quantity", "ShopId" },
                values: new object[,]
                {
                    { 1, "Fresh sandwich", "Chicken Sandwich", "https://images.unsplash.com/photo-1568901346375-23c9450c58cd", 12.5f, 98, 1 },
                    { 2, "Healthy salad", "Caesar Salad", "https://images.unsplash.com/photo-1551248429-40975aa4de74", 8f, 77, 1 },
                    { 3, "Fresh juice", "Orange Juice", "https://images.unsplash.com/photo-1621506289937-a8e4df240d0b", 5.5f, 60, 1 },
                    { 4, "Strong coffee", "Espresso", "https://images.unsplash.com/photo-1511920170033-f8396924c348", 3.5f, 200, 2 },
                    { 5, "Coffee with foam", "Cappuccino", "https://images.unsplash.com/photo-1509042239860-f550ce710b93", 4.5f, 150, 2 },
                    { 6, "Smooth milk coffee", "Latte", "https://images.unsplash.com/photo-1523942839745-7848d0f5c9d1", 6f, 100, 2 },
                    { 7, "High-end watch", "Luxury Watch", "https://images.unsplash.com/photo-1523275335684-37898b6baf30", 500f, 20, 3 },
                    { 8, "Premium leather bag", "Designer Handbag", "https://images.unsplash.com/photo-1584917865442-de89df76afd3", 1200f, 15, 3 },
                    { 9, "Stylish sunglasses", "RayBan Sunglasses", "https://images.unsplash.com/photo-1511499767150-a48a237f0083", 300f, 10, 3 },
                    { 10, "Travel pillow", "Neck Pillow", "https://images.unsplash.com/photo-1540497077202-7c8a3999166f", 25f, 120, 4 },
                    { 11, "Universal plug adapter", "Travel Adapter", "https://images.unsplash.com/photo-1572635196237-14b3f281503f", 10f, 205, 4 },
                    { 12, "Portable charger", "Power Bank", "https://images.unsplash.com/photo-1609592424060-bd0c5b305c91", 15f, 90, 4 }
                });

            migrationBuilder.InsertData(
                table: "CartItems",
                columns: new[] { "Id", "CartId", "Quantity", "ShopItemId" },
                values: new object[] { 1, 1, 2, 1 });

            migrationBuilder.InsertData(
                table: "EmployeeFlights",
                columns: new[] { "EmployeeId", "FlightId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 5 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 3, 1 },
                    { 3, 3 },
                    { 3, 4 },
                    { 4, 4 },
                    { 4, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ShopItemId",
                table: "CartItems",
                column: "ShopItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ClientId",
                table: "Carts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFlights_FlightId",
                table: "EmployeeFlights",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_GateId",
                table: "Flights",
                column: "GateId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_RouteId",
                table: "Flights",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_RunwayId",
                table: "Flights",
                column: "RunwayId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CartId",
                table: "Reservations",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_AirportId",
                table: "Routes",
                column: "AirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_CompanyId",
                table: "Routes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_ShopId",
                table: "ShopItems",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_ManagerId",
                table: "Shops",
                column: "ManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "EmployeeFlights");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "ShopItems");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "Gates");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Runways");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}

