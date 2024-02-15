using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRTestApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCandidateTaskRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsTaskNeeded",
                table: "Candidates",
                newName: "IsTaskRequired");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsTaskRequired",
                table: "Candidates",
                newName: "IsTaskNeeded");
        }
    }
}
