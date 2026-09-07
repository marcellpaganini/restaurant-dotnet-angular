/*
    Restaurant database schema
    Tables: Clients, Meals, Orders, OrderItems
*/

IF DB_ID(N'RestaurantDb') IS NULL
BEGIN
    CREATE DATABASE RestaurantDb;
END
GO

USE RestaurantDb;
GO

IF OBJECT_ID(N'dbo.OrderItems', N'U') IS NOT NULL DROP TABLE dbo.OrderItems;
IF OBJECT_ID(N'dbo.Orders', N'U') IS NOT NULL DROP TABLE dbo.Orders;
IF OBJECT_ID(N'dbo.Meals', N'U') IS NOT NULL DROP TABLE dbo.Meals;
IF OBJECT_ID(N'dbo.Clients', N'U') IS NOT NULL DROP TABLE dbo.Clients;
GO

CREATE TABLE dbo.Clients
(
    Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Clients PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(50) NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Clients_CreatedAt DEFAULT SYSUTCDATETIME()
);
GO

CREATE TABLE dbo.Meals
(
    Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Meals PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500) NULL,
    Price DECIMAL(10,2) NOT NULL,
    IsAvailable BIT NOT NULL CONSTRAINT DF_Meals_IsAvailable DEFAULT (1),
    CONSTRAINT CK_Meals_Price CHECK (Price >= 0)
);
GO

CREATE TABLE dbo.Orders
(
    Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Orders PRIMARY KEY,
    ClientId INT NOT NULL,
    OrderDate DATETIME2 NOT NULL CONSTRAINT DF_Orders_OrderDate DEFAULT SYSUTCDATETIME(),
    Status NVARCHAR(50) NOT NULL CONSTRAINT DF_Orders_Status DEFAULT (N'Pending'),
    TotalAmount DECIMAL(10,2) NOT NULL CONSTRAINT DF_Orders_TotalAmount DEFAULT (0),
    CONSTRAINT FK_Orders_Clients FOREIGN KEY (ClientId) REFERENCES dbo.Clients(Id),
    CONSTRAINT CK_Orders_TotalAmount CHECK (TotalAmount >= 0)
);
GO

CREATE TABLE dbo.OrderItems
(
    Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_OrderItems PRIMARY KEY,
    OrderId INT NOT NULL,
    MealId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) REFERENCES dbo.Orders(Id),
    CONSTRAINT FK_OrderItems_Meals FOREIGN KEY (MealId) REFERENCES dbo.Meals(Id),
    CONSTRAINT CK_OrderItems_Quantity CHECK (Quantity > 0),
    CONSTRAINT CK_OrderItems_UnitPrice CHECK (UnitPrice >= 0)
);
GO

CREATE INDEX IX_Orders_ClientId ON dbo.Orders(ClientId);
CREATE INDEX IX_OrderItems_OrderId ON dbo.OrderItems(OrderId);
CREATE INDEX IX_OrderItems_MealId ON dbo.OrderItems(MealId);
GO

-- Sample data
INSERT INTO dbo.Clients (Name, Email, Phone)
VALUES
    (N'Alice Johnson', N'alice@example.com', N'555-0101'),
    (N'Bob Smith', N'bob@example.com', N'555-0102');

INSERT INTO dbo.Meals (Name, Description, Price, IsAvailable)
VALUES
    (N'Margherita Pizza', N'Tomato, mozzarella, basil', 12.50, 1),
    (N'Caesar Salad', N'Romaine, parmesan, croutons', 8.00, 1),
    (N'Spaghetti Bolognese', N'Classic meat sauce', 14.00, 1);

INSERT INTO dbo.Orders (ClientId, Status, TotalAmount)
VALUES (1, N'Pending', 20.50);

INSERT INTO dbo.OrderItems (OrderId, MealId, Quantity, UnitPrice)
VALUES
    (1, 1, 1, 12.50),
    (1, 2, 1, 8.00);
GO
