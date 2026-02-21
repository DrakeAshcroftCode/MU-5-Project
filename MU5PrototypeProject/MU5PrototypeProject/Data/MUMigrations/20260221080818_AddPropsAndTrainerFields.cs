using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MU5PrototypeProject.Data.MUMigrations
{
    /// <inheritdoc />
    public partial class AddPropsAndTrainerFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Trainers",
                type: "TEXT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Trainers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Trainers",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Props",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionID = table.Column<int>(type: "INTEGER", nullable: false),
                    PropName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Props", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Props_Sessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "Sessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseProps",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExerciseID = table.Column<int>(type: "INTEGER", nullable: false),
                    PropID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseProps", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExerciseProps_Exercises_ExerciseID",
                        column: x => x.ExerciseID,
                        principalTable: "Exercises",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseProps_Props_PropID",
                        column: x => x.PropID,
                        principalTable: "Props",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseProps_ExerciseID",
                table: "ExerciseProps",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseProps_PropID",
                table: "ExerciseProps",
                column: "PropID");

            migrationBuilder.CreateIndex(
                name: "IX_Props_SessionID",
                table: "Props",
                column: "SessionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseProps");

            migrationBuilder.DropTable(
                name: "Props");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Trainers");
        }
    }
}
