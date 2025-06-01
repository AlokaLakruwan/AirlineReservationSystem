CREATE TABLE [Role] (
  [RoleID] int PRIMARY KEY IDENTITY(1, 1),
  [RoleName] nvarchar(255),
  [CreatedAt] datetime,
  [UpdatedAt] datetime
)
GO

CREATE TABLE [User] (
  [UserID] int PRIMARY KEY IDENTITY(1, 1),
  [Username] nvarchar(255),
  [PasswordHash] nvarchar(255),
  [FirstName] nvarchar(255),
  [LastName] nvarchar(255),
  [Email] nvarchar(255),
  [RoleID] int,
  [CreatedAt] datetime,
  [UpdatedAt] datetime
)
GO

CREATE TABLE [Airport] (
  [AirportID] int PRIMARY KEY IDENTITY(1, 1),
  [Code] nvarchar(255),
  [Name] nvarchar(255),
  [City] nvarchar(255),
  [Country] nvarchar(255),
  [CreatedAt] datetime,
  [UpdatedAt] datetime
)
GO

CREATE TABLE [Airplane] (
  [AirplaneID] int PRIMARY KEY IDENTITY(1, 1),
  [TailNumber] nvarchar(255),
  [Model] nvarchar(255),
  [CapacityClass] nvarchar(255),
  [CreatedAt] datetime,
  [UpdatedAt] datetime
)
GO

CREATE TABLE [Flight] (
  [FlightID] int PRIMARY KEY IDENTITY(1, 1),
  [FlightNumber] nvarchar(255),
  [DepTime] datetime,
  [ArrTime] datetime,
  [OriginAirportID] int,
  [DestinationAirportID] int,
  [AirplaneID] int,
  [CreatedAt] datetime,
  [UpdatedAt] datetime
)
GO

CREATE TABLE [Booking] (
  [BookingID] int PRIMARY KEY IDENTITY(1, 1),
  [BookingDate] datetime,
  [UserID] int,
  [FlightID] int,
  [SeatNumber] nvarchar(255),
  [ServiceClass] nvarchar(255),
  [CreatedAt] datetime,
  [UpdatedAt] datetime
)
GO

ALTER TABLE [User] ADD FOREIGN KEY ([RoleID]) REFERENCES [Role] ([RoleID])
GO

ALTER TABLE [Flight] ADD FOREIGN KEY ([OriginAirportID]) REFERENCES [Airport] ([AirportID])
GO

ALTER TABLE [Flight] ADD FOREIGN KEY ([DestinationAirportID]) REFERENCES [Airport] ([AirportID])
GO

ALTER TABLE [Flight] ADD FOREIGN KEY ([AirplaneID]) REFERENCES [Airplane] ([AirplaneID])
GO

ALTER TABLE [Booking] ADD FOREIGN KEY ([UserID]) REFERENCES [User] ([UserID])
GO

ALTER TABLE [Booking] ADD FOREIGN KEY ([FlightID]) REFERENCES [Flight] ([FlightID])
GO
