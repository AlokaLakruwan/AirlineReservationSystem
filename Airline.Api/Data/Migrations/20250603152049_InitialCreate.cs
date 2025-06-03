using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Airline.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Intentionally left blank – this is our “baseline” so EF will not recreate tables.
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intentionally left blank.
        }

    }
}
