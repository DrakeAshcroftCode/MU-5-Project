using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MU5PrototypeProject.Data.MUMigrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apparatuses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApparatusName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apparatuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DOB = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ClientFolderUrl = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExerciseName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ApparatusID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Exercises_Apparatuses_ApparatusID",
                        column: x => x.ApparatusID,
                        principalTable: "Apparatuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SessionType = table.Column<int>(type: "INTEGER", nullable: false),
                    SessionsPerWeekRecommended = table.Column<int>(type: "INTEGER", nullable: true),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false),
                    TrainerID = table.Column<int>(type: "INTEGER", nullable: false),
                    ClientID = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Sessions_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sessions_Trainers_TrainerID",
                        column: x => x.TrainerID,
                        principalTable: "Trainers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseSettings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExerciseID = table.Column<int>(type: "INTEGER", nullable: false),
                    GearBarLevel = table.Column<int>(type: "INTEGER", nullable: true),
                    StopperSetting = table.Column<int>(type: "INTEGER", nullable: true),
                    HeadrestPosition = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    StrapsType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseSettings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExerciseSettings_Exercises_ExerciseID",
                        column: x => x.ExerciseID,
                        principalTable: "Exercises",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionID = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPaid = table.Column<bool>(type: "INTEGER", nullable: false),
                    AdminNotes = table.Column<string>(type: "TEXT", nullable: true),
                    AdminInitials = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    NextAppointmentBooked = table.Column<bool>(type: "INTEGER", nullable: false),
                    CommunicatedProgress = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReadyToProgress = table.Column<bool>(type: "INTEGER", nullable: false),
                    CourseCorrectionNeeded = table.Column<bool>(type: "INTEGER", nullable: false),
                    TeamConsult = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReferredExternally = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReferredTo = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminStatuses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AdminStatuses_Sessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "Sessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhysioInfos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionID = table.Column<int>(type: "INTEGER", nullable: false),
                    InsuranceCompany = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CoverageAmountPerYear = table.Column<decimal>(type: "TEXT", nullable: true),
                    AmountUsed = table.Column<decimal>(type: "TEXT", nullable: true),
                    CoverageResetsDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PhysiotherapistName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CoverageShared = table.Column<bool>(type: "INTEGER", nullable: false),
                    CommunicatedWithPhysio = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysioInfos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PhysioInfos_Sessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "Sessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "SessionExercises",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionID = table.Column<int>(type: "INTEGER", nullable: false),
                    ExerciseID = table.Column<int>(type: "INTEGER", nullable: false),
                    Springs = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Props = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionExercises", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SessionExercises_Exercises_ExerciseID",
                        column: x => x.ExerciseID,
                        principalTable: "Exercises",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SessionExercises_Sessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "Sessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionNotes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionID = table.Column<int>(type: "INTEGER", nullable: false),
                    CompletedByTrainerID = table.Column<int>(type: "INTEGER", nullable: true),
                    Goals = table.Column<string>(type: "TEXT", nullable: true),
                    HealthMedicalHistory = table.Column<string>(type: "TEXT", nullable: true),
                    GeneralComments = table.Column<string>(type: "TEXT", nullable: true),
                    SubjectiveReports = table.Column<string>(type: "TEXT", nullable: true),
                    ObjectiveFindings = table.Column<string>(type: "TEXT", nullable: true),
                    Plan = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionNotes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SessionNotes_Sessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "Sessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionNotes_Trainers_CompletedByTrainerID",
                        column: x => x.CompletedByTrainerID,
                        principalTable: "Trainers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_AdminStatuses_SessionID",
                table: "AdminStatuses",
                column: "SessionID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_DOB_LastName_FirstName",
                table: "Clients",
                columns: new[] { "DOB", "LastName", "FirstName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseProps_ExerciseID",
                table: "ExerciseProps",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseProps_PropID",
                table: "ExerciseProps",
                column: "PropID");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ApparatusID",
                table: "Exercises",
                column: "ApparatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSettings_ExerciseID",
                table: "ExerciseSettings",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_PhysioInfos_SessionID",
                table: "PhysioInfos",
                column: "SessionID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Props_SessionID",
                table: "Props",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_SessionExercises_ExerciseID",
                table: "SessionExercises",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_SessionExercises_SessionID",
                table: "SessionExercises",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_SessionNotes_CompletedByTrainerID",
                table: "SessionNotes",
                column: "CompletedByTrainerID");

            migrationBuilder.CreateIndex(
                name: "IX_SessionNotes_SessionID",
                table: "SessionNotes",
                column: "SessionID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_ClientID",
                table: "Sessions",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_TrainerID",
                table: "Sessions",
                column: "TrainerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminStatuses");

            migrationBuilder.DropTable(
                name: "ExerciseProps");

            migrationBuilder.DropTable(
                name: "ExerciseSettings");

            migrationBuilder.DropTable(
                name: "PhysioInfos");

            migrationBuilder.DropTable(
                name: "SessionExercises");

            migrationBuilder.DropTable(
                name: "SessionNotes");

            migrationBuilder.DropTable(
                name: "Props");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Apparatuses");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Trainers");
        }
    }
}
