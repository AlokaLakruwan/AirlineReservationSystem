using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Airline.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFlightOverlapTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER TRIGGER trg_Flight_NoOverlap
                ON [Flight]
                AFTER INSERT, UPDATE
                AS
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM [Flight] f
                        JOIN inserted i
                          ON f.AirplaneId = i.AirplaneId
                         AND f.FlightId   <> i.FlightId
                         AND i.DepartureTime < f.ArrivalTime
                         AND f.DepartureTime  < i.ArrivalTime
                    )
                    BEGIN
                        RAISERROR('Scheduling conflict: overlapping flight for this airplane.', 16, 1);
                        ROLLBACK TRANSACTION;
                    END
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Flight_NoOverlap;");
        }
    }
}
