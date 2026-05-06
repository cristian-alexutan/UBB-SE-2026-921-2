IF NOT EXISTS (
    SELECT name
    FROM sys.databases
    WHERE name = 'AirportDB'
)
BEGIN
    CREATE DATABASE AirportDB;
END;
GO

USE AirportDB;
GO

DROP TABLE IF EXISTS Flight_employees;
DROP TABLE IF EXISTS Flights;
DROP TABLE IF EXISTS Routes;
DROP TABLE IF EXISTS CartItem;
DROP TABLE IF EXISTS Reservation;
DROP TABLE IF EXISTS Cart;
DROP TABLE IF EXISTS Item;
DROP TABLE IF EXISTS Shop;
DROP TABLE IF EXISTS Manager;
DROP TABLE IF EXISTS Client;
DROP TABLE IF EXISTS Ticket;
DROP TABLE IF EXISTS Gates;
DROP TABLE IF EXISTS Runways;
DROP TABLE IF EXISTS Airports;
DROP TABLE IF EXISTS Employees;
DROP TABLE IF EXISTS EmployeeRoles;
DROP TABLE IF EXISTS Companies;
GO


CREATE TABLE Manager (
    manager_id  INT           IDENTITY(1,1) PRIMARY KEY,
    name        VARCHAR(50),
    email       VARCHAR(50),
    phone       VARCHAR(15)
);
GO

CREATE TABLE Shop (
    shop_id     INT           IDENTITY(1,1) PRIMARY KEY,
    type        VARCHAR(50)   NOT NULL,
    name        VARCHAR(50)   NOT NULL,
    manager_id  INT           FOREIGN KEY REFERENCES Manager(manager_id)
);
GO

CREATE TABLE Item (
    item_id     INT           IDENTITY(1,1) PRIMARY KEY,
    shop_id     INT           FOREIGN KEY REFERENCES Shop(shop_id) ON DELETE CASCADE,
    name        VARCHAR(100),
    description VARCHAR(500),
    price       FLOAT,
    stock       INT,
    img         VARCHAR(300)
);
GO

CREATE TABLE Client (
    client_id     INT         IDENTITY(1,1) PRIMARY KEY,
    name          VARCHAR(50),
    date_of_birth DATE
);
GO

CREATE TABLE Cart (
    cart_id    INT            IDENTITY(1,1) PRIMARY KEY,
    client_id  INT            FOREIGN KEY REFERENCES Client(client_id),
    status     VARCHAR(50)
);
GO

CREATE TABLE CartItem (
    cart_item_id  INT         IDENTITY(1,1) PRIMARY KEY,
    cart_id       INT         FOREIGN KEY REFERENCES Cart(cart_id),
    item_id       INT         FOREIGN KEY REFERENCES Item(item_id),
    quantity      INT
);
GO

CREATE TABLE Reservation (
    reservation_id    INT     IDENTITY(1,1) PRIMARY KEY,
    cart_id           INT     FOREIGN KEY REFERENCES Cart(cart_id),
    reservation_date  DATE,
    time_slot         TIME,
    active            BIT     NOT NULL DEFAULT 1
);
GO

CREATE TABLE Ticket (
    ticket_id    INT          IDENTITY(1,1) PRIMARY KEY,
    category     VARCHAR(50),
    subcategory  VARCHAR(50)
);
GO

CREATE TABLE Companies (
    id   INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(20)
);
GO

CREATE TABLE Airports (
    id   INT           IDENTITY(1,1) PRIMARY KEY,
    city VARCHAR(30),
    name VARCHAR(100),
    code VARCHAR(10)
);
GO

CREATE TABLE EmployeeRoles (
    role_id INT         NOT NULL PRIMARY KEY,
    title   VARCHAR(50) NOT NULL
);
GO

CREATE TABLE Employees (
    id           INT  IDENTITY(1,1) PRIMARY KEY,
    name         VARCHAR(50),
    birthday     DATE,
    salary       INT,
    hiring_date  DATE,
    role_id      INT  FOREIGN KEY REFERENCES EmployeeRoles(role_id)
);
GO

CREATE TABLE Runways (
    id           INT           IDENTITY(1,1) PRIMARY KEY,
    name         VARCHAR(20),
    handle_time  INT
);
GO

CREATE TABLE Gates (
    id   INT         IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(20)
);
GO

CREATE TABLE Routes (
    id                   INT           IDENTITY(1,1) PRIMARY KEY,
    company_id           INT           FOREIGN KEY REFERENCES Companies(id),
    route_type           VARCHAR(10),
    airport_id           INT           FOREIGN KEY REFERENCES Airports(id),
    reccurence_interval  INT,
    start_date           DATE,
    end_date             DATE,
    departure_time       DATETIME,
    arrival_time         DATETIME,
    capacity             INT
);
GO

CREATE TABLE Flights (
    id            INT           IDENTITY(1,1) PRIMARY KEY,
    route_id      INT           FOREIGN KEY REFERENCES Routes(id),
    date          DATETIME,
    runway_id     INT           FOREIGN KEY REFERENCES Runways(id),
    gate_id       INT           FOREIGN KEY REFERENCES Gates(id),
    flight_number VARCHAR(50)
);
GO

CREATE TABLE Flight_employees (
    id_employee  INT  FOREIGN KEY REFERENCES Employees(id),
    id_flight    INT  FOREIGN KEY REFERENCES Flights(id),
    PRIMARY KEY (id_employee, id_flight)
);
GO


INSERT INTO Manager (name, email, phone) VALUES
('Marcel', 'marcel@gmail.com', '4074593789');

INSERT INTO Shop (type, name, manager_id) VALUES
('Food & Beverage',   'Sky Bites',      1),
('Coffee Shop',       'Runway Cafe',    1),
('Luxury Goods',      'Elite Boutique', 1),
('Travel Essentials', 'FlySmart Store', 1);

INSERT INTO Item (shop_id, name, description, price, stock, img) VALUES
(1, 'Chicken Sandwich', 'Fresh sandwich',          12.5,  98, 'https://images.unsplash.com/photo-1568901346375-23c9450c58cd'),
(1, 'Caesar Salad',     'Healthy salad',             8.0,  77, 'https://images.unsplash.com/photo-1551248429-40975aa4de74'),
(1, 'Orange Juice',     'Fresh juice',               5.5,  60, 'https://images.unsplash.com/photo-1621506289937-a8e4df240d0b'),
(2, 'Espresso',         'Strong coffee',             3.5, 200, 'https://images.unsplash.com/photo-1511920170033-f8396924c348'),
(2, 'Cappuccino',       'Coffee with foam',          4.5, 150, 'https://images.unsplash.com/photo-1509042239860-f550ce710b93'),
(2, 'Latte',            'Smooth milk coffee',        6.0, 100, 'https://images.unsplash.com/photo-1523942839745-7848d0f5c9d1'),
(3, 'Luxury Watch',     'High-end watch',          500.0,  20, 'https://images.unsplash.com/photo-1523275335684-37898b6baf30'),
(3, 'Designer Handbag', 'Premium leather bag',    1200.0,  15, 'https://images.unsplash.com/photo-1584917865442-de89df76afd3'),
(3, 'RayBan Sunglasses','Stylish sunglasses',      300.0,  10, 'https://images.unsplash.com/photo-1511499767150-a48a237f0083'),
(4, 'Neck Pillow',      'Travel pillow',            25.0, 120, 'https://images.unsplash.com/photo-1540497077202-7c8a3999166f'),
(4, 'Travel Adapter',   'Universal plug adapter',   10.0, 205, 'https://images.unsplash.com/photo-1572635196237-14b3f281503f'),
(4, 'Power Bank',       'Portable charger',         15.0,  90, 'https://images.unsplash.com/photo-1609592424060-bd0c5b305c91');

INSERT INTO Client (name, date_of_birth) VALUES
('Crina', '2003-03-04');

INSERT INTO Cart (client_id, status) VALUES
(1, 'active');

INSERT INTO CartItem (cart_id, item_id, quantity) VALUES
(1, 1, 2);

INSERT INTO Ticket (category, subcategory) VALUES
('Duty Free Shops', 'Global Duty Free'),
('Duty Free Shops', 'Sky Bites'),
('Duty Free Shops', 'Runway Cafe'),
('Duty Free Shops', 'Elite Boutique'),
('Duty Free Shops', 'FlySmart Store');
GO

INSERT INTO Companies(name) VALUES
('WizzAir'),
('Lufthansa');
GO

--------------------------------------------------
-- AIRPORTS
--------------------------------------------------
INSERT INTO Airports(city, name, code) VALUES
('London', 'London Luton Airport', 'LTN'),
('Munich', 'Munich Airport', 'MUC'),
('Cluj-Napoca', 'Cluj International Airport', 'CLJ');
GO

--------------------------------------------------
-- EMPLOYEES
--------------------------------------------------
INSERT INTO EmployeeRoles(role_id, title) VALUES
(0, 'Other'),
(1, 'Pilot'),
(2, 'Co-Pilot'),
(3, 'Flight Attendant'),
(4, 'Flight Dispatcher');
GO

INSERT INTO Employees(name, role_id, birthday, salary, hiring_date) VALUES
('Andrei Popescu', 1, '1990-05-12', 12000, '2021-03-01'),
('Maria Ionescu', 3, '1995-09-20', 7000, '2022-06-15'),
('Vlad Georgescu', 2, '1988-11-03', 10000, '2020-01-10'),
('Elena Dumitrescu', 3, '1997-02-14', 6800, '2023-04-05');
GO

--------------------------------------------------
-- RUNWAYS
--------------------------------------------------
INSERT INTO Runways(name, handle_time) VALUES
('Runway A1', 15),
('Runway B2', 20),
('Runway C3', 18);
GO

--------------------------------------------------
-- GATES
--------------------------------------------------
INSERT INTO Gates(name) VALUES
('Gate 1'),
('Gate 2'),
('Gate 3'),
('Gate 4');
GO

--------------------------------------------------
-- ROUTES
-- company_id references Companies
-- airport_id references Airports
-- route_type should match your UI logic: ARR / DEP
--------------------------------------------------
INSERT INTO Routes(
    company_id,
    route_type,
    airport_id,
    reccurence_interval,
    start_date,
    end_date,
    departure_time,
    arrival_time,
    capacity
)
VALUES
-- WizzAir departure from base to London
(1, 'DEP', 1, 1, '2026-03-01', '2026-12-31', '1900-01-01 08:30:00', '1900-01-01 10:45:00', 180),

-- WizzAir arrival from London
(1, 'ARR', 1, 1, '2026-03-01', '2026-12-31', '1900-01-01 11:30:00', '1900-01-01 13:40:00', 180),

-- Lufthansa departure to Munich
(2, 'DEP', 2, 2, '2026-03-01', '2026-12-31', '1900-01-01 14:00:00', '1900-01-01 15:20:00', 160),

-- Lufthansa arrival from Munich
(2, 'ARR', 2, 2, '2026-03-01', '2026-12-31', '1900-01-01 16:00:00', '1900-01-01 17:25:00', 160);
GO

--------------------------------------------------
-- FLIGHTS
-- route_id references Routes
-- runway_id references Runways
-- gate_id references Gates
--------------------------------------------------
INSERT INTO Flights(route_id, date, runway_id, gate_id, flight_number) VALUES
(1, '2026-03-27 08:30:00', 1, 1, 'W6 3401'),
(2, '2026-03-27 13:40:00', 2, 2, 'W6 3402'),
(3, '2026-03-27 14:00:00', 3, 3, 'LH 1671'),
(4, '2026-03-27 17:25:00', 1, 4, 'LH 1672'),
(1, '2026-03-28 08:30:00', 2, 1, 'W6 3403');
GO

--------------------------------------------------
-- ASSIGN EMPLOYEES TO FLIGHTS
--------------------------------------------------
INSERT INTO Flight_employees(id_employee, id_flight) VALUES
-- Andrei (Pilot)
(1, 1),
(1, 2),
(1, 5),

-- Maria (Flight Attendant)
(2, 1),
(2, 2),
(2, 3),

-- Vlad (Co-Pilot)
(3, 1),
(3, 3),
(3, 4),

-- Elena (Flight Attendant)
(4, 4),
(4, 5);
GO
