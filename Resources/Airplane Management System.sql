/****** Object:  Database [airline-db]    Script Date: 6/3/2025 1:47:09 PM ******/
CREATE DATABASE [airline-db]  (EDITION = 'GeneralPurpose', SERVICE_OBJECTIVE = 'GP_S_Gen5_2', MAXSIZE = 32 GB) WITH CATALOG_COLLATION = SQL_Latin1_General_CP1_CI_AS, LEDGER = OFF;
GO
ALTER DATABASE [airline-db] SET COMPATIBILITY_LEVEL = 170
GO
ALTER DATABASE [airline-db] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [airline-db] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [airline-db] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [airline-db] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [airline-db] SET ARITHABORT OFF 
GO
ALTER DATABASE [airline-db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [airline-db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [airline-db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [airline-db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [airline-db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [airline-db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [airline-db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [airline-db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [airline-db] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [airline-db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [airline-db] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [airline-db] SET  MULTI_USER 
GO
ALTER DATABASE [airline-db] SET ENCRYPTION ON
GO
ALTER DATABASE [airline-db] SET QUERY_STORE = ON
GO
ALTER DATABASE [airline-db] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
/*** The scripts of database scoped configurations in Azure should be executed inside the target database connection. ***/
GO
-- ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 8;
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 6/3/2025 1:47:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Airplane]    Script Date: 6/3/2025 1:47:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Airplane](
	[AirplaneID] [int] IDENTITY(1,1) NOT NULL,
	[TailNumber] [nvarchar](50) NOT NULL,
	[Model] [nvarchar](100) NOT NULL,
	[CapacityClass] [nvarchar](20) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[AirplaneID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AirplaneConfiguration]    Script Date: 6/3/2025 1:47:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AirplaneConfiguration](
	[AirplaneConfigID] [int] IDENTITY(1,1) NOT NULL,
	[AirplaneID] [int] NOT NULL,
	[ClassName] [nvarchar](50) NOT NULL,
	[SeatCount] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[AirplaneConfigID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Airport]    Script Date: 6/3/2025 1:47:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Airport](
	[AirportID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[City] [nvarchar](255) NOT NULL,
	[Country] [nvarchar](255) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[AirportID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Booking]    Script Date: 6/3/2025 1:47:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[BookingID] [int] IDENTITY(1,1) NOT NULL,
	[BookingDate] [datetime] NOT NULL,
	[UserID] [int] NOT NULL,
	[FlightID] [int] NOT NULL,
	[SeatNumber] [nvarchar](10) NOT NULL,
	[ServiceClass] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[BookingID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Flight]    Script Date: 6/3/2025 1:47:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Flight](
	[FlightID] [int] IDENTITY(1,1) NOT NULL,
	[FlightNumber] [nvarchar](50) NOT NULL,
	[DepartureTime] [datetime] NOT NULL,
	[ArrivalTime] [datetime] NOT NULL,
	[OriginAirportID] [int] NOT NULL,
	[DestinationAirportID] [int] NOT NULL,
	[AirplaneID] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[FlightID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 6/3/2025 1:47:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 6/3/2025 1:47:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](255) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[FirstName] [nvarchar](255) NOT NULL,
	[LastName] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[RoleID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_Booking_Flight_Seat]    Script Date: 6/3/2025 1:47:09 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Booking_Flight_Seat] ON [dbo].[Booking]
(
	[FlightID] ASC,
	[SeatNumber] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Airplane] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Airplane] ADD  DEFAULT (NULL) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[AirplaneConfiguration] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[AirplaneConfiguration] ADD  DEFAULT (NULL) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Airport] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Airport] ADD  DEFAULT (NULL) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Booking] ADD  DEFAULT (getdate()) FOR [BookingDate]
GO
ALTER TABLE [dbo].[Booking] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Booking] ADD  DEFAULT (NULL) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Flight] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Flight] ADD  DEFAULT (NULL) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Role] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Role] ADD  DEFAULT (NULL) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT (NULL) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[AirplaneConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_AirplaneConfig_Airplane] FOREIGN KEY([AirplaneID])
REFERENCES [dbo].[Airplane] ([AirplaneID])
GO
ALTER TABLE [dbo].[AirplaneConfiguration] CHECK CONSTRAINT [FK_AirplaneConfig_Airplane]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_Flight] FOREIGN KEY([FlightID])
REFERENCES [dbo].[Flight] ([FlightID])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_Flight]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_User]
GO
ALTER TABLE [dbo].[Flight]  WITH CHECK ADD  CONSTRAINT [FK_Flight_Airplane] FOREIGN KEY([AirplaneID])
REFERENCES [dbo].[Airplane] ([AirplaneID])
GO
ALTER TABLE [dbo].[Flight] CHECK CONSTRAINT [FK_Flight_Airplane]
GO
ALTER TABLE [dbo].[Flight]  WITH CHECK ADD  CONSTRAINT [FK_Flight_DestinationAirport] FOREIGN KEY([DestinationAirportID])
REFERENCES [dbo].[Airport] ([AirportID])
GO
ALTER TABLE [dbo].[Flight] CHECK CONSTRAINT [FK_Flight_DestinationAirport]
GO
ALTER TABLE [dbo].[Flight]  WITH CHECK ADD  CONSTRAINT [FK_Flight_OriginAirport] FOREIGN KEY([OriginAirportID])
REFERENCES [dbo].[Airport] ([AirportID])
GO
ALTER TABLE [dbo].[Flight] CHECK CONSTRAINT [FK_Flight_OriginAirport]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([RoleID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[Airplane]  WITH CHECK ADD  CONSTRAINT [CHK_Airplane_CapacityClass] CHECK  (([CapacityClass]='Large' OR [CapacityClass]='Medium' OR [CapacityClass]='Small'))
GO
ALTER TABLE [dbo].[Airplane] CHECK CONSTRAINT [CHK_Airplane_CapacityClass]
GO
ALTER TABLE [dbo].[AirplaneConfiguration]  WITH CHECK ADD  CONSTRAINT [CHK_ApConfig_ClassName] CHECK  (([ClassName]='Economy' OR [ClassName]='Business' OR [ClassName]='First'))
GO
ALTER TABLE [dbo].[AirplaneConfiguration] CHECK CONSTRAINT [CHK_ApConfig_ClassName]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [CHK_Booking_ServiceClass] CHECK  (([ServiceClass]='Economy' OR [ServiceClass]='Business' OR [ServiceClass]='First'))
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [CHK_Booking_ServiceClass]
GO
ALTER DATABASE [airline-db] SET  READ_WRITE 
GO
