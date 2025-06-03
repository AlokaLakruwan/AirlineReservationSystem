USE [airline-db];
GO

/*=============================================
  2. TABLE: Role
  - Stores user roles: Customer, Operator, Administrator
=============================================*/
CREATE TABLE [Role] (
  RoleID     INT IDENTITY(1,1) PRIMARY KEY,
  RoleName   NVARCHAR(50) NOT NULL,           -- e.g. 'Customer', 'Operator', 'Administrator'
  CreatedAt  DATETIME  NOT NULL DEFAULT GETDATE(),
  UpdatedAt  DATETIME  DEFAULT NULL
);



/*=============================================
  3. TABLE: [User]
  - Each user has exactly one RoleID.
  - Administrators can deactivate users via IsActive flag.
=============================================*/
CREATE TABLE [User] (
  UserID        INT IDENTITY(1,1) PRIMARY KEY,
  Username      NVARCHAR(255) NOT NULL,        -- Login username
  PasswordHash  NVARCHAR(255) NOT NULL,        -- Hashed password
  FirstName     NVARCHAR(255) NOT NULL,
  LastName      NVARCHAR(255) NOT NULL,
  Email         NVARCHAR(255) NOT NULL,
  RoleID        INT          NOT NULL,         -- FK ⇒ Role.RoleID
  IsActive      BIT          NOT NULL DEFAULT 1, -- 1 = active, 0 = deactivated
  CreatedAt     DATETIME     NOT NULL DEFAULT GETDATE(),
  UpdatedAt     DATETIME     DEFAULT NULL,
  CONSTRAINT FK_User_Role
    FOREIGN KEY (RoleID) REFERENCES [Role](RoleID)
);



/*=============================================
  4. TABLE: Airport
  - Stores airport codes and locations.
=============================================*/
CREATE TABLE [Airport] (
  AirportID   INT IDENTITY(1,1) PRIMARY KEY,
  Code        NVARCHAR(10)  NOT NULL,          -- e.g. 'JFK', 'LHR'
  Name        NVARCHAR(255) NOT NULL,          -- Full airport name
  City        NVARCHAR(255) NOT NULL,
  Country     NVARCHAR(255) NOT NULL,
  CreatedAt   DATETIME      NOT NULL DEFAULT GETDATE(),
  UpdatedAt   DATETIME      DEFAULT NULL
);



/*=============================================
  5. TABLE: Airplane
  - Stores each airplane’s tail number, model, capacity class.
  - CapacityClass must be 'Small', 'Medium', or 'Large'.
=============================================*/
CREATE TABLE [Airplane] (
  AirplaneID    INT IDENTITY(1,1) PRIMARY KEY,
  TailNumber    NVARCHAR(50)  NOT NULL,         -- e.g. 'N12345'
  Model         NVARCHAR(100) NOT NULL,         -- e.g. 'Boeing 737'
  CapacityClass NVARCHAR(20)  NOT NULL,         -- 'Small', 'Medium', 'Large'
  CreatedAt     DATETIME      NOT NULL DEFAULT GETDATE(),
  UpdatedAt     DATETIME      DEFAULT NULL,
  CONSTRAINT CHK_Airplane_CapacityClass
    CHECK (CapacityClass IN ('Small','Medium','Large'))
);



/*=============================================
  6. TABLE: AirplaneConfiguration
  - Defines how many seats each airplane has in each class.
  - ClassName must be 'First','Business','Economy'.
=============================================*/
CREATE TABLE [AirplaneConfiguration] (
  AirplaneConfigID INT IDENTITY(1,1) PRIMARY KEY,
  AirplaneID       INT          NOT NULL,      -- FK ⇒ Airplane.AirplaneID
  ClassName        NVARCHAR(50) NOT NULL,      -- 'First', 'Business', 'Economy'
  SeatCount        INT          NOT NULL,      -- Total seats in that class on that airplane
  CreatedAt        DATETIME     NOT NULL DEFAULT GETDATE(),
  UpdatedAt        DATETIME     DEFAULT NULL,
  CONSTRAINT FK_AirplaneConfig_Airplane
    FOREIGN KEY (AirplaneID) REFERENCES [Airplane](AirplaneID),
  CONSTRAINT CHK_ApConfig_ClassName
    CHECK (ClassName IN ('First','Business','Economy'))
);



/*=============================================
  7. TABLE: AirplaneLocation
  - Tracks where each airplane “is” at any given time.
  - Before scheduling a new flight, code can verify
    that CurrentAirportID matches the proposed origin.
=============================================*/
CREATE TABLE [AirplaneLocation] (
  AirplaneID       INT        PRIMARY KEY,    -- FK ⇒ Airplane.AirplaneID
  CurrentAirportID INT        NOT NULL,       -- FK ⇒ Airport.AirportID
  LastUpdated      DATETIME   NOT NULL DEFAULT GETDATE(),
  CONSTRAINT FK_AirplaneLoc_Airplane
    FOREIGN KEY (AirplaneID)       REFERENCES [Airplane](AirplaneID),
  CONSTRAINT FK_AirplaneLoc_Airport
    FOREIGN KEY (CurrentAirportID) REFERENCES [Airport](AirportID)
);



/*=============================================
  8. TABLE: Flight
  - Each flight ties a specific Airplane to two Airports and times.
  - We add a trigger to prevent overlapping flights for the same Airplane.
=============================================*/
CREATE TABLE [Flight] (
  FlightID           INT IDENTITY(1,1) PRIMARY KEY,
  FlightNumber       NVARCHAR(50)      NOT NULL,    -- e.g. 'AA101'
  DepartureTime      DATETIME          NOT NULL,
  ArrivalTime        DATETIME          NOT NULL,
  OriginAirportID    INT               NOT NULL,    -- FK ⇒ Airport.AirportID
  DestinationAirportID INT             NOT NULL,    -- FK ⇒ Airport.AirportID
  AirplaneID         INT               NOT NULL,    -- FK ⇒ Airplane.AirplaneID
  CreatedAt          DATETIME          NOT NULL DEFAULT GETDATE(),
  UpdatedAt          DATETIME          DEFAULT NULL,
  CONSTRAINT FK_Flight_OriginAirport
    FOREIGN KEY (OriginAirportID)      REFERENCES [Airport](AirportID),
  CONSTRAINT FK_Flight_DestinationAirport
    FOREIGN KEY (DestinationAirportID) REFERENCES [Airport](AirportID),
  CONSTRAINT FK_Flight_Airplane
    FOREIGN KEY (AirplaneID)           REFERENCES [Airplane](AirplaneID)
);



/*---------------------------------------------
  8.1 TRIGGER: Prevent overlapping flights
  If you try to insert/update a Flight for an AirplaneID
  whose new DepartureTime/ArrivalTime overlaps an existing row,
  the trigger throws an error and rolls back the insert/update.
---------------------------------------------*/
CREATE TRIGGER trg_Flight_NoOverlap
ON [Flight]
AFTER INSERT, UPDATE
AS
BEGIN
  IF EXISTS (
    SELECT 1
    FROM [Flight] f
    JOIN inserted i 
      ON f.AirplaneID = i.AirplaneID
     AND f.FlightID   <> i.FlightID
     AND i.DepartureTime < f.ArrivalTime
     AND f.DepartureTime  < i.ArrivalTime
  )
  BEGIN
    RAISERROR('Scheduling conflict: overlapping flight for this airplane.', 16, 1);
    ROLLBACK TRANSACTION;
  END
END;
GO



/*=============================================
  9. TABLE: Booking
  - Each booking links a User to a Flight and a seat.
  - We enforce that ServiceClass is 'First','Business','Economy'.
  - We add a UNIQUE INDEX on (FlightID, SeatNumber) to prevent double-booking.
=============================================*/
CREATE TABLE [Booking] (
  BookingID     INT IDENTITY(1,1) PRIMARY KEY,
  BookingDate   DATETIME     NOT NULL DEFAULT GETDATE(),
  UserID        INT          NOT NULL,          -- FK ⇒ User.UserID
  FlightID      INT          NOT NULL,          -- FK ⇒ Flight.FlightID
  SeatNumber    NVARCHAR(10) NOT NULL,          -- e.g. '12A'
  ServiceClass  NVARCHAR(50) NOT NULL,          -- 'First','Business','Economy'
  CreatedAt     DATETIME     NOT NULL DEFAULT GETDATE(),
  UpdatedAt     DATETIME     DEFAULT NULL,
  CONSTRAINT FK_Booking_User
    FOREIGN KEY (UserID)   REFERENCES [User](UserID),
  CONSTRAINT FK_Booking_Flight
    FOREIGN KEY (FlightID) REFERENCES [Flight](FlightID),
  CONSTRAINT CHK_Booking_ServiceClass
    CHECK (ServiceClass IN ('First','Business','Economy'))
);

/* Prevent two passengers from booking the same seat on the same flight */
CREATE UNIQUE INDEX UX_Booking_Flight_Seat
  ON [Booking](FlightID, SeatNumber);
GO
