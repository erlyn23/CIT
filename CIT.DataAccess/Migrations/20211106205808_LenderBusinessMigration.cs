using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIT.DataAccess.Migrations
{
    public partial class LenderBusinessMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "entitiesinfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<short>(type: "smallint(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entitiesinfo", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OperationName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PageName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pages", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Country = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Province = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Street1 = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Street2 = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HouseNumber = table.Column<int>(type: "int(11)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    EntityInfoId = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_Addresses_EntitiesInfo",
                        column: x => x.EntityInfoId,
                        principalTable: "entitiesinfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "lenderbusiness",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BusinessName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rnc = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Photo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntityInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lenderbusiness", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_LenderBusiness_EntitiesInfo",
                        column: x => x.EntityInfoId,
                        principalTable: "entitiesinfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "lenderaddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LenderBusinessId = table.Column<int>(type: "int(11)", nullable: false),
                    AddressId = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.LenderBusinessId, x.AddressId });
                    table.ForeignKey(
                        name: "Fk_LenderAddresses_Address",
                        column: x => x.AddressId,
                        principalTable: "addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Fk_LenderAddresses_LenderBusinesses",
                        column: x => x.AddressId,
                        principalTable: "lenderbusiness",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "loans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DuesQuantity = table.Column<int>(type: "int(11)", nullable: false),
                    TotalLoan = table.Column<decimal>(type: "decimal(13,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PayDay = table.Column<int>(type: "int(11)", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    MensualPay = table.Column<decimal>(type: "decimal(13,2)", nullable: false),
                    LenderBusinessId = table.Column<int>(type: "int", nullable: false),
                    EntityInfoId = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loans", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_Loans_EntitiesInfo",
                        column: x => x.EntityInfoId,
                        principalTable: "entitiesinfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Fk_Loans_LenderBusiness",
                        column: x => x.LenderBusinessId,
                        principalTable: "lenderbusiness",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LenderBusinessId = table.Column<int>(type: "int", nullable: false),
                    EntityInfoId = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_Roles_EntitiesInfo",
                        column: x => x.EntityInfoId,
                        principalTable: "entitiesinfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Fk_Roles_LenderBusiness",
                        column: x => x.LenderBusinessId,
                        principalTable: "lenderbusiness",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificationDocument = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Photo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LenderBusinessId = table.Column<int>(type: "int", nullable: false),
                    EntityInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_Users_EntitiesInfo",
                        column: x => x.EntityInfoId,
                        principalTable: "entitiesinfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Fk_Users_LenderBusiness",
                        column: x => x.LenderBusinessId,
                        principalTable: "lenderbusiness",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Brand = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Model = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Enrollment = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LicensePlate = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Color = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Year = table.Column<int>(type: "int(11)", nullable: false),
                    LenderBusinessId = table.Column<int>(type: "int", nullable: false),
                    EntityInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_Vehicles_EntitiesInfo",
                        column: x => x.EntityInfoId,
                        principalTable: "entitiesinfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Fk_Vehicles_LenderBusiness",
                        column: x => x.LenderBusinessId,
                        principalTable: "lenderbusiness",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rolepermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OperationId = table.Column<int>(type: "int", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.OperationId, x.PageId });
                    table.ForeignKey(
                        name: "Fk_RolePermission_Pages",
                        column: x => x.PageId,
                        principalTable: "pages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Fk_RolePermissions_Operations",
                        column: x => x.OperationId,
                        principalTable: "operations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Fk_RolePermissions_Roles",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Operation = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int(11)", nullable: false),
                    ResultMessageOrObject = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, defaultValueSql: "'NULL'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LenderBusinessId = table.Column<int>(type: "int", nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_Logs_LenderBusiness",
                        column: x => x.LenderBusinessId,
                        principalTable: "lenderbusiness",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Fk_Logs_Users",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int(11)", nullable: false),
                    LoanId = table.Column<int>(type: "int(11)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Pay = table.Column<decimal>(type: "decimal(13,2)", nullable: false),
                    LenderBusinessId = table.Column<int>(type: "int", nullable: false),
                    EntityInfoId = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.UserId, x.LoanId });
                    table.ForeignKey(
                        name: "Fk_Payments_EntitiesInfo",
                        column: x => x.EntityInfoId,
                        principalTable: "entitiesinfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Fk_Payments_LenderBusiness",
                        column: x => x.LenderBusinessId,
                        principalTable: "lenderbusiness",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Fk_Payments_Loans",
                        column: x => x.LoanId,
                        principalTable: "loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Fk_Payments_Users",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "useraddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int(11)", nullable: false),
                    AddressId = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.UserId, x.AddressId });
                    table.ForeignKey(
                        name: "Fk_UserAddresses_Address",
                        column: x => x.AddressId,
                        principalTable: "addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Fk_UserAddresses_Users",
                        column: x => x.AddressId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "userroles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(type: "int(11)", nullable: false),
                    UserId = table.Column<int>(type: "int(11)", nullable: false),
                    EntityInfoId = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "Fk_UserRoles_EntitiesInfo",
                        column: x => x.EntityInfoId,
                        principalTable: "entitiesinfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Fk_UserRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Fk_UserRoles_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VehicleAssignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleAssignment_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleAssignment_vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "Fk_Addresses_EntitiesInfo",
                table: "addresses",
                column: "EntityInfoId");

            migrationBuilder.CreateIndex(
                name: "Fk_LenderAddresses_Address",
                table: "lenderaddress",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Fk_LenderAddresses_LenderBusinesses",
                table: "lenderaddress",
                column: "LenderBusinessId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Fk_LenderBusinesses_EntitiesInfo",
                table: "lenderbusiness",
                column: "EntityInfoId");

            migrationBuilder.CreateIndex(
                name: "Ix_LenderBusiness_Email",
                table: "lenderbusiness",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Ix_LenderBusiness_Phone",
                table: "lenderbusiness",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Ix_LenderBusiness_Rnc",
                table: "lenderbusiness",
                column: "Rnc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Fk_Loans_EntitiesInfo",
                table: "loans",
                column: "EntityInfoId");

            migrationBuilder.CreateIndex(
                name: "Fk_Loans_LenderBusiness",
                table: "loans",
                column: "LenderBusinessId");

            migrationBuilder.CreateIndex(
                name: "Fk_Logs_LenderBusiness",
                table: "logs",
                column: "LenderBusinessId");

            migrationBuilder.CreateIndex(
                name: "Fk_Logs_Users",
                table: "logs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "Fk_Payments_EntitiesInfo",
                table: "payments",
                column: "EntityInfoId");

            migrationBuilder.CreateIndex(
                name: "Fk_Payments_LenderBusiness",
                table: "payments",
                column: "LenderBusinessId");

            migrationBuilder.CreateIndex(
                name: "Fk_Payments_Loans",
                table: "payments",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "Fk_Payments_Users",
                table: "payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "Fk_RolePermission_Pages",
                table: "rolepermissions",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "Fk_RolePermissions_Operations",
                table: "rolepermissions",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "Fk_RolePermissions_Roles",
                table: "rolepermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "Fk_Roles_EntitiesInfo",
                table: "roles",
                column: "EntityInfoId");

            migrationBuilder.CreateIndex(
                name: "Fk_Roles_LenderBusiness",
                table: "roles",
                column: "LenderBusinessId");

            migrationBuilder.CreateIndex(
                name: "RoleName",
                table: "roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Fk_UserAddresses_Address",
                table: "useraddresses",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Fk_UserAddresses_Users",
                table: "useraddresses",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Fk_UserRoles_EntitiesInfo",
                table: "userroles",
                column: "EntityInfoId");

            migrationBuilder.CreateIndex(
                name: "RoleId",
                table: "userroles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UserId",
                table: "userroles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Fk_Users_EntitiesInfo",
                table: "users",
                column: "EntityInfoId");

            migrationBuilder.CreateIndex(
                name: "Fk_Users_LenderBusiness",
                table: "users",
                column: "LenderBusinessId");

            migrationBuilder.CreateIndex(
                name: "IdentificationDocument",
                table: "users",
                column: "IdentificationDocument",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Phone",
                table: "users",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleAssignment_UserId",
                table: "VehicleAssignment",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleAssignment_VehicleId",
                table: "VehicleAssignment",
                column: "VehicleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Enrollment",
                table: "vehicles",
                column: "Enrollment",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Fk_Vehicles_EntitiesInfo",
                table: "vehicles",
                column: "EntityInfoId");

            migrationBuilder.CreateIndex(
                name: "Fk_Vehicles_LenderBusiness",
                table: "vehicles",
                column: "LenderBusinessId");

            migrationBuilder.CreateIndex(
                name: "LicensePlate",
                table: "vehicles",
                column: "LicensePlate",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lenderaddress");

            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "rolepermissions");

            migrationBuilder.DropTable(
                name: "useraddresses");

            migrationBuilder.DropTable(
                name: "userroles");

            migrationBuilder.DropTable(
                name: "VehicleAssignment");

            migrationBuilder.DropTable(
                name: "loans");

            migrationBuilder.DropTable(
                name: "pages");

            migrationBuilder.DropTable(
                name: "operations");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "vehicles");

            migrationBuilder.DropTable(
                name: "lenderbusiness");

            migrationBuilder.DropTable(
                name: "entitiesinfo");
        }
    }
}
