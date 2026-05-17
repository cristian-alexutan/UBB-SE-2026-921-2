IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [Airports] (
        [Id] int NOT NULL IDENTITY,
        [Code] nvarchar(max) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [City] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Airports] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [Clients] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Clients] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [Companies] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Companies] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [Employees] (
        [Id] int NOT NULL IDENTITY,
        [Role] int NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Birthday] date NOT NULL,
        [HiringDate] date NOT NULL,
        [Salary] int NOT NULL,
        CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [Gates] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Gates] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [Managers] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Managers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [Runways] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [HandleTime] int NOT NULL,
        CONSTRAINT [PK_Runways] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [Tickets] (
        [Id] int NOT NULL IDENTITY,
        [Category] nvarchar(max) NOT NULL,
        [Subcategory] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Tickets] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [Carts] (
        [Id] int NOT NULL IDENTITY,
        [ClientId] int NOT NULL,
        CONSTRAINT [PK_Carts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Carts_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [Routes] (
        [Id] int NOT NULL IDENTITY,
        [RouteType] nvarchar(max) NOT NULL,
        [RecurrenceInterval] int NOT NULL,
        [StartDate] date NOT NULL,
        [EndDate] date NOT NULL,
        [DepartureTime] time NOT NULL,
        [ArrivalTime] time NOT NULL,
        [Capacity] int NOT NULL,
        [CompanyId] int NOT NULL,
        [AirportId] int NOT NULL,
        CONSTRAINT [PK_Routes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Routes_Airports_AirportId] FOREIGN KEY ([AirportId]) REFERENCES [Airports] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Routes_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Companies] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [Shops] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Type] nvarchar(max) NOT NULL,
        [ManagerId] int NOT NULL,
        CONSTRAINT [PK_Shops] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Shops_Managers_ManagerId] FOREIGN KEY ([ManagerId]) REFERENCES [Managers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [Reservations] (
        [Id] int NOT NULL IDENTITY,
        [CartId] int NOT NULL,
        [Active] bit NOT NULL,
        [ReservationDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Reservations] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Reservations_Carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [Carts] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [Flights] (
        [Id] int NOT NULL IDENTITY,
        [Date] datetime2 NOT NULL,
        [FlightNumber] nvarchar(max) NOT NULL,
        [RouteId] int NOT NULL,
        [RunwayId] int NOT NULL,
        [GateId] int NOT NULL,
        CONSTRAINT [PK_Flights] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Flights_Gates_GateId] FOREIGN KEY ([GateId]) REFERENCES [Gates] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Flights_Routes_RouteId] FOREIGN KEY ([RouteId]) REFERENCES [Routes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Flights_Runways_RunwayId] FOREIGN KEY ([RunwayId]) REFERENCES [Runways] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [ShopItems] (
        [Id] int NOT NULL IDENTITY,
        [Quantity] int NOT NULL,
        [Price] real NOT NULL,
        [ShopId] int NOT NULL,
        [Photo] nvarchar(max) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ShopItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ShopItems_Shops_ShopId] FOREIGN KEY ([ShopId]) REFERENCES [Shops] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [EmployeeFlights] (
        [EmployeeId] int NOT NULL,
        [FlightId] int NOT NULL,
        CONSTRAINT [PK_EmployeeFlights] PRIMARY KEY ([EmployeeId], [FlightId]),
        CONSTRAINT [FK_EmployeeFlights_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_EmployeeFlights_Flights_FlightId] FOREIGN KEY ([FlightId]) REFERENCES [Flights] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE TABLE [CartItems] (
        [Id] int NOT NULL IDENTITY,
        [ShopItemId] int NOT NULL,
        [Quantity] int NOT NULL,
        [CartId] int NULL,
        CONSTRAINT [PK_CartItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CartItems_Carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [Carts] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CartItems_ShopItems_ShopItemId] FOREIGN KEY ([ShopItemId]) REFERENCES [ShopItems] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'City', N'Code', N'Name') AND [object_id] = OBJECT_ID(N'[Airports]'))
        SET IDENTITY_INSERT [Airports] ON;
    EXEC(N'INSERT INTO [Airports] ([Id], [City], [Code], [Name])
    VALUES (1, N''London'', N''LTN'', N''London Luton Airport''),
    (2, N''Munich'', N''MUC'', N''Munich Airport''),
    (3, N''Cluj-Napoca'', N''CLJ'', N''Cluj International Airport'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'City', N'Code', N'Name') AND [object_id] = OBJECT_ID(N'[Airports]'))
        SET IDENTITY_INSERT [Airports] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Clients]'))
        SET IDENTITY_INSERT [Clients] ON;
    EXEC(N'INSERT INTO [Clients] ([Id], [Name])
    VALUES (1, N''Crina'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Clients]'))
        SET IDENTITY_INSERT [Clients] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Companies]'))
        SET IDENTITY_INSERT [Companies] ON;
    EXEC(N'INSERT INTO [Companies] ([Id], [Name])
    VALUES (1, N''WizzAir''),
    (2, N''Lufthansa'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Companies]'))
        SET IDENTITY_INSERT [Companies] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Birthday', N'HiringDate', N'Name', N'Role', N'Salary') AND [object_id] = OBJECT_ID(N'[Employees]'))
        SET IDENTITY_INSERT [Employees] ON;
    EXEC(N'INSERT INTO [Employees] ([Id], [Birthday], [HiringDate], [Name], [Role], [Salary])
    VALUES (1, ''1990-05-12'', ''2021-03-01'', N''Andrei Popescu'', 1, 12000),
    (2, ''1995-09-20'', ''2022-06-15'', N''Maria Ionescu'', 3, 7000),
    (3, ''1988-11-03'', ''2020-01-10'', N''Vlad Georgescu'', 2, 10000),
    (4, ''1997-02-14'', ''2023-04-05'', N''Elena Dumitrescu'', 3, 6800)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Birthday', N'HiringDate', N'Name', N'Role', N'Salary') AND [object_id] = OBJECT_ID(N'[Employees]'))
        SET IDENTITY_INSERT [Employees] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Gates]'))
        SET IDENTITY_INSERT [Gates] ON;
    EXEC(N'INSERT INTO [Gates] ([Id], [Name])
    VALUES (1, N''Gate 1''),
    (2, N''Gate 2''),
    (3, N''Gate 3''),
    (4, N''Gate 4'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Gates]'))
        SET IDENTITY_INSERT [Gates] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'Name', N'Phone') AND [object_id] = OBJECT_ID(N'[Managers]'))
        SET IDENTITY_INSERT [Managers] ON;
    EXEC(N'INSERT INTO [Managers] ([Id], [Email], [Name], [Phone])
    VALUES (1, N''marcel@gmail.com'', N''Marcel'', N''4074593789'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'Name', N'Phone') AND [object_id] = OBJECT_ID(N'[Managers]'))
        SET IDENTITY_INSERT [Managers] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'HandleTime', N'Name') AND [object_id] = OBJECT_ID(N'[Runways]'))
        SET IDENTITY_INSERT [Runways] ON;
    EXEC(N'INSERT INTO [Runways] ([Id], [HandleTime], [Name])
    VALUES (1, 15, N''Runway A1''),
    (2, 20, N''Runway B2''),
    (3, 18, N''Runway C3'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'HandleTime', N'Name') AND [object_id] = OBJECT_ID(N'[Runways]'))
        SET IDENTITY_INSERT [Runways] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Category', N'Subcategory') AND [object_id] = OBJECT_ID(N'[Tickets]'))
        SET IDENTITY_INSERT [Tickets] ON;
    EXEC(N'INSERT INTO [Tickets] ([Id], [Category], [Subcategory])
    VALUES (1, N''Duty Free Shops'', N''Global Duty Free''),
    (2, N''Duty Free Shops'', N''Sky Bites''),
    (3, N''Duty Free Shops'', N''Runway Cafe''),
    (4, N''Duty Free Shops'', N''Elite Boutique''),
    (5, N''Duty Free Shops'', N''FlySmart Store'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Category', N'Subcategory') AND [object_id] = OBJECT_ID(N'[Tickets]'))
        SET IDENTITY_INSERT [Tickets] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClientId') AND [object_id] = OBJECT_ID(N'[Carts]'))
        SET IDENTITY_INSERT [Carts] ON;
    EXEC(N'INSERT INTO [Carts] ([Id], [ClientId])
    VALUES (1, 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClientId') AND [object_id] = OBJECT_ID(N'[Carts]'))
        SET IDENTITY_INSERT [Carts] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AirportId', N'ArrivalTime', N'Capacity', N'CompanyId', N'DepartureTime', N'EndDate', N'RecurrenceInterval', N'RouteType', N'StartDate') AND [object_id] = OBJECT_ID(N'[Routes]'))
        SET IDENTITY_INSERT [Routes] ON;
    EXEC(N'INSERT INTO [Routes] ([Id], [AirportId], [ArrivalTime], [Capacity], [CompanyId], [DepartureTime], [EndDate], [RecurrenceInterval], [RouteType], [StartDate])
    VALUES (1, 1, ''10:45:00'', 180, 1, ''08:30:00'', ''2026-12-31'', 1, N''DEP'', ''2026-03-01''),
    (2, 1, ''13:40:00'', 180, 1, ''11:30:00'', ''2026-12-31'', 1, N''ARR'', ''2026-03-01''),
    (3, 2, ''15:20:00'', 160, 2, ''14:00:00'', ''2026-12-31'', 2, N''DEP'', ''2026-03-01''),
    (4, 2, ''17:25:00'', 160, 2, ''16:00:00'', ''2026-12-31'', 2, N''ARR'', ''2026-03-01'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AirportId', N'ArrivalTime', N'Capacity', N'CompanyId', N'DepartureTime', N'EndDate', N'RecurrenceInterval', N'RouteType', N'StartDate') AND [object_id] = OBJECT_ID(N'[Routes]'))
        SET IDENTITY_INSERT [Routes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ManagerId', N'Name', N'Type') AND [object_id] = OBJECT_ID(N'[Shops]'))
        SET IDENTITY_INSERT [Shops] ON;
    EXEC(N'INSERT INTO [Shops] ([Id], [ManagerId], [Name], [Type])
    VALUES (1, 1, N''Sky Bites'', N''Food & Beverage''),
    (2, 1, N''Runway Cafe'', N''Coffee Shop''),
    (3, 1, N''Elite Boutique'', N''Luxury Goods''),
    (4, 1, N''FlySmart Store'', N''Travel Essentials'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ManagerId', N'Name', N'Type') AND [object_id] = OBJECT_ID(N'[Shops]'))
        SET IDENTITY_INSERT [Shops] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Date', N'FlightNumber', N'GateId', N'RouteId', N'RunwayId') AND [object_id] = OBJECT_ID(N'[Flights]'))
        SET IDENTITY_INSERT [Flights] ON;
    EXEC(N'INSERT INTO [Flights] ([Id], [Date], [FlightNumber], [GateId], [RouteId], [RunwayId])
    VALUES (1, ''2026-03-27T08:30:00.0000000'', N''W6 3401'', 1, 1, 1),
    (2, ''2026-03-27T13:40:00.0000000'', N''W6 3402'', 2, 2, 2),
    (3, ''2026-03-27T14:00:00.0000000'', N''LH 1671'', 3, 3, 3),
    (4, ''2026-03-27T17:25:00.0000000'', N''LH 1672'', 4, 4, 1),
    (5, ''2026-03-28T08:30:00.0000000'', N''W6 3403'', 1, 1, 2)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Date', N'FlightNumber', N'GateId', N'RouteId', N'RunwayId') AND [object_id] = OBJECT_ID(N'[Flights]'))
        SET IDENTITY_INSERT [Flights] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name', N'Photo', N'Price', N'Quantity', N'ShopId') AND [object_id] = OBJECT_ID(N'[ShopItems]'))
        SET IDENTITY_INSERT [ShopItems] ON;
    EXEC(N'INSERT INTO [ShopItems] ([Id], [Description], [Name], [Photo], [Price], [Quantity], [ShopId])
    VALUES (1, N''Fresh sandwich'', N''Chicken Sandwich'', N''https://images.unsplash.com/photo-1568901346375-23c9450c58cd'', CAST(12.5 AS real), 98, 1),
    (2, N''Healthy salad'', N''Caesar Salad'', N''https://images.unsplash.com/photo-1551248429-40975aa4de74'', CAST(8 AS real), 77, 1),
    (3, N''Fresh juice'', N''Orange Juice'', N''https://images.unsplash.com/photo-1621506289937-a8e4df240d0b'', CAST(5.5 AS real), 60, 1),
    (4, N''Strong coffee'', N''Espresso'', N''https://images.unsplash.com/photo-1511920170033-f8396924c348'', CAST(3.5 AS real), 200, 2),
    (5, N''Coffee with foam'', N''Cappuccino'', N''https://images.unsplash.com/photo-1509042239860-f550ce710b93'', CAST(4.5 AS real), 150, 2),
    (6, N''Smooth milk coffee'', N''Latte'', N''https://images.unsplash.com/photo-1523942839745-7848d0f5c9d1'', CAST(6 AS real), 100, 2),
    (7, N''High-end watch'', N''Luxury Watch'', N''https://images.unsplash.com/photo-1523275335684-37898b6baf30'', CAST(500 AS real), 20, 3),
    (8, N''Premium leather bag'', N''Designer Handbag'', N''https://images.unsplash.com/photo-1584917865442-de89df76afd3'', CAST(1200 AS real), 15, 3),
    (9, N''Stylish sunglasses'', N''RayBan Sunglasses'', N''https://images.unsplash.com/photo-1511499767150-a48a237f0083'', CAST(300 AS real), 10, 3),
    (10, N''Travel pillow'', N''Neck Pillow'', N''https://images.unsplash.com/photo-1540497077202-7c8a3999166f'', CAST(25 AS real), 120, 4),
    (11, N''Universal plug adapter'', N''Travel Adapter'', N''https://images.unsplash.com/photo-1572635196237-14b3f281503f'', CAST(10 AS real), 205, 4),
    (12, N''Portable charger'', N''Power Bank'', N''https://images.unsplash.com/photo-1609592424060-bd0c5b305c91'', CAST(15 AS real), 90, 4)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name', N'Photo', N'Price', N'Quantity', N'ShopId') AND [object_id] = OBJECT_ID(N'[ShopItems]'))
        SET IDENTITY_INSERT [ShopItems] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CartId', N'Quantity', N'ShopItemId') AND [object_id] = OBJECT_ID(N'[CartItems]'))
        SET IDENTITY_INSERT [CartItems] ON;
    EXEC(N'INSERT INTO [CartItems] ([Id], [CartId], [Quantity], [ShopItemId])
    VALUES (1, 1, 2, 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CartId', N'Quantity', N'ShopItemId') AND [object_id] = OBJECT_ID(N'[CartItems]'))
        SET IDENTITY_INSERT [CartItems] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'EmployeeId', N'FlightId') AND [object_id] = OBJECT_ID(N'[EmployeeFlights]'))
        SET IDENTITY_INSERT [EmployeeFlights] ON;
    EXEC(N'INSERT INTO [EmployeeFlights] ([EmployeeId], [FlightId])
    VALUES (1, 1),
    (1, 2),
    (1, 5),
    (2, 1),
    (2, 2),
    (2, 3),
    (3, 1),
    (3, 3),
    (3, 4),
    (4, 4),
    (4, 5)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'EmployeeId', N'FlightId') AND [object_id] = OBJECT_ID(N'[EmployeeFlights]'))
        SET IDENTITY_INSERT [EmployeeFlights] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE INDEX [IX_CartItems_CartId] ON [CartItems] ([CartId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE INDEX [IX_CartItems_ShopItemId] ON [CartItems] ([ShopItemId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE INDEX [IX_Carts_ClientId] ON [Carts] ([ClientId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE INDEX [IX_EmployeeFlights_FlightId] ON [EmployeeFlights] ([FlightId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE INDEX [IX_Flights_GateId] ON [Flights] ([GateId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE INDEX [IX_Flights_RouteId] ON [Flights] ([RouteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE INDEX [IX_Flights_RunwayId] ON [Flights] ([RunwayId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE INDEX [IX_Reservations_CartId] ON [Reservations] ([CartId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE INDEX [IX_Routes_AirportId] ON [Routes] ([AirportId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE INDEX [IX_Routes_CompanyId] ON [Routes] ([CompanyId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE INDEX [IX_ShopItems_ShopId] ON [ShopItems] ([ShopId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    CREATE INDEX [IX_Shops_ManagerId] ON [Shops] ([ManagerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507174704_Migration-db'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260507174704_Migration-db', N'10.0.7');
END;

COMMIT;
GO

