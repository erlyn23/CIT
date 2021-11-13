﻿// <auto-generated />
using System;
using CIT.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CIT.DataAccess.Migrations
{
    [DbContext(typeof(CentroInversionesTecnocorpDbContext))]
    partial class CentroInversionesTecnocorpDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("CIT.DataAccess.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("EntityInfoId")
                        .HasColumnType("int(11)");

                    b.Property<int>("HouseNumber")
                        .HasColumnType("int(11)");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Street1")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Street2")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasDefaultValueSql("'NULL'");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "EntityInfoId" }, "Fk_Addresses_EntitiesInfo");

                    b.ToTable("addresses");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Entitiesinfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<short>("Status")
                        .HasColumnType("smallint(6)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("entitiesinfo");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.LenderAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LenderBusinessId")
                        .HasColumnType("int(11)");

                    b.Property<int>("AddressId")
                        .HasColumnType("int(11)");

                    b.HasKey("Id", "LenderBusinessId", "AddressId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "AddressId" }, "Fk_LenderAddresses_Address")
                        .IsUnique();

                    b.HasIndex(new[] { "LenderBusinessId" }, "Fk_LenderAddresses_LenderBusinesses")
                        .IsUnique();

                    b.ToTable("lenderaddress");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.LenderBusiness", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BusinessName")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("EntityInfoId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Photo")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Rnc")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "EntityInfoId" }, "Fk_LenderBusinesses_EntitiesInfo");

                    b.HasIndex(new[] { "Email" }, "Ix_LenderBusiness_Email")
                        .IsUnique();

                    b.HasIndex(new[] { "Phone" }, "Ix_LenderBusiness_Phone")
                        .IsUnique();

                    b.HasIndex(new[] { "Rnc" }, "Ix_LenderBusiness_Rnc")
                        .IsUnique();

                    b.ToTable("lenderbusinesses");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.LenderRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RoleId")
                        .HasColumnType("int(11)");

                    b.Property<int>("LenderBusinessId")
                        .HasColumnType("int(11)");

                    b.HasKey("Id", "RoleId", "LenderBusinessId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "LenderBusinessId" }, "LenderBusinessId")
                        .IsUnique();

                    b.HasIndex(new[] { "RoleId" }, "RoleId");

                    b.ToTable("lenderroles");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DuesQuantity")
                        .HasColumnType("int(11)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EntityInfoId")
                        .HasColumnType("int(11)");

                    b.Property<decimal>("InterestRate")
                        .HasColumnType("decimal(3,2)");

                    b.Property<int>("LenderBusinessId")
                        .HasColumnType("int");

                    b.Property<decimal>("MensualPay")
                        .HasColumnType("decimal(13,2)");

                    b.Property<int>("PayDay")
                        .HasColumnType("int(11)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("TotalLoan")
                        .HasColumnType("decimal(13,2)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "EntityInfoId" }, "Fk_Loans_EntitiesInfo");

                    b.HasIndex(new[] { "LenderBusinessId" }, "Fk_Loans_LenderBusiness");

                    b.ToTable("loans");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LenderBusinessId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Operation")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("ResultMessageOrObject")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasDefaultValueSql("'NULL'");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "LenderBusinessId" }, "Fk_Logs_LenderBusiness");

                    b.ToTable("logs");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Login", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Email" }, "Ix_LenderBusiness_Email")
                        .IsUnique()
                        .HasDatabaseName("Ix_LenderBusiness_Email1");

                    b.ToTable("logins");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Operation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("OperationName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("operations");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Page", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PageName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("pages");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("UserId")
                        .HasColumnType("int(11)");

                    b.Property<int>("LoanId")
                        .HasColumnType("int(11)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EntityInfoId")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.Property<int>("LenderBusinessId")
                        .HasColumnType("int");

                    b.Property<decimal>("Pay")
                        .HasColumnType("decimal(13,2)");

                    b.HasKey("Id", "UserId", "LoanId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "EntityInfoId" }, "Fk_Payments_EntitiesInfo");

                    b.HasIndex(new[] { "LenderBusinessId" }, "Fk_Payments_LenderBusiness");

                    b.HasIndex(new[] { "LoanId" }, "Fk_Payments_Loans");

                    b.HasIndex(new[] { "UserId" }, "Fk_Payments_Users");

                    b.ToTable("payments");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EntityInfoId")
                        .HasColumnType("int(11)");

                    b.Property<int>("LenderBusinessId")
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "EntityInfoId" }, "Fk_Roles_EntitiesInfo");

                    b.HasIndex(new[] { "LenderBusinessId" }, "Fk_Roles_LenderBusiness");

                    b.HasIndex(new[] { "RoleName" }, "RoleName")
                        .IsUnique();

                    b.ToTable("roles");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Rolepermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OperationId")
                        .HasColumnType("int");

                    b.Property<int>("PageId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id", "OperationId", "PageId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "PageId" }, "Fk_RolePermission_Pages");

                    b.HasIndex(new[] { "OperationId" }, "Fk_RolePermissions_Operations");

                    b.HasIndex(new[] { "RoleId" }, "Fk_RolePermissions_Roles");

                    b.ToTable("rolepermissions");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("EntityInfoId")
                        .HasColumnType("int");

                    b.Property<string>("IdentificationDocument")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<int>("LenderBusinessId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Photo")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasDefaultValueSql("'NULL'");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Email" }, "Email")
                        .IsUnique();

                    b.HasIndex(new[] { "EntityInfoId" }, "Fk_Users_EntitiesInfo");

                    b.HasIndex(new[] { "LenderBusinessId" }, "Fk_Users_LenderBusiness");

                    b.HasIndex(new[] { "IdentificationDocument" }, "IdentificationDocument")
                        .IsUnique();

                    b.HasIndex(new[] { "Phone" }, "Phone")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Useraddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("UserId")
                        .HasColumnType("int(11)");

                    b.Property<int>("AddressId")
                        .HasColumnType("int(11)");

                    b.HasKey("Id", "UserId", "AddressId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "AddressId" }, "Fk_UserAddresses_Address")
                        .IsUnique();

                    b.HasIndex(new[] { "UserId" }, "Fk_UserAddresses_Users")
                        .IsUnique();

                    b.ToTable("useraddresses");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Userrole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RoleId")
                        .HasColumnType("int(11)");

                    b.Property<int>("UserId")
                        .HasColumnType("int(11)");

                    b.Property<int>("EntityInfoId")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.HasKey("Id", "RoleId", "UserId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "EntityInfoId" }, "Fk_UserRoles_EntitiesInfo");

                    b.HasIndex(new[] { "RoleId" }, "RoleId")
                        .HasDatabaseName("RoleId1");

                    b.HasIndex(new[] { "UserId" }, "UserId")
                        .IsUnique();

                    b.ToTable("userroles");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Enrollment")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<int>("EntityInfoId")
                        .HasColumnType("int");

                    b.Property<int>("LenderBusinessId")
                        .HasColumnType("int");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("varchar(7)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<int>("Year")
                        .HasColumnType("int(11)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Enrollment" }, "Enrollment")
                        .IsUnique();

                    b.HasIndex(new[] { "EntityInfoId" }, "Fk_Vehicles_EntitiesInfo");

                    b.HasIndex(new[] { "LenderBusinessId" }, "Fk_Vehicles_LenderBusiness");

                    b.HasIndex(new[] { "LicensePlate" }, "LicensePlate")
                        .IsUnique();

                    b.ToTable("vehicles");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.VehicleAssignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.HasIndex("VehicleId")
                        .IsUnique();

                    b.ToTable("VehicleAssignment");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Address", b =>
                {
                    b.HasOne("CIT.DataAccess.Models.Entitiesinfo", "EntityInfo")
                        .WithMany("Addresses")
                        .HasForeignKey("EntityInfoId")
                        .HasConstraintName("Fk_Addresses_EntitiesInfo")
                        .IsRequired();

                    b.Navigation("EntityInfo");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.LenderAddress", b =>
                {
                    b.HasOne("CIT.DataAccess.Models.Address", "Address")
                        .WithOne("LenderAddress")
                        .HasForeignKey("CIT.DataAccess.Models.LenderAddress", "AddressId")
                        .HasConstraintName("Fk_LenderAddresses_Address")
                        .IsRequired();

                    b.HasOne("CIT.DataAccess.Models.LenderBusiness", "LenderBusiness")
                        .WithOne("LenderAddress")
                        .HasForeignKey("CIT.DataAccess.Models.LenderAddress", "LenderBusinessId")
                        .HasConstraintName("Fk_LenderAddresses_LenderBusinesses")
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("LenderBusiness");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.LenderBusiness", b =>
                {
                    b.HasOne("CIT.DataAccess.Models.Entitiesinfo", "EntityInfo")
                        .WithMany("LenderBusinesses")
                        .HasForeignKey("EntityInfoId")
                        .HasConstraintName("Fk_LenderBusiness_EntitiesInfo")
                        .IsRequired();

                    b.Navigation("EntityInfo");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.LenderRole", b =>
                {
                    b.HasOne("CIT.DataAccess.Models.LenderBusiness", "LenderBusiness")
                        .WithOne("LenderRole")
                        .HasForeignKey("CIT.DataAccess.Models.LenderRole", "LenderBusinessId")
                        .HasConstraintName("Fk_LenderRole_LenderBusiness")
                        .IsRequired();

                    b.HasOne("CIT.DataAccess.Models.Role", "Role")
                        .WithMany("LenderRoles")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("Fk_LenderRole_Roles")
                        .IsRequired();

                    b.Navigation("LenderBusiness");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Loan", b =>
                {
                    b.HasOne("CIT.DataAccess.Models.Entitiesinfo", "EntityInfo")
                        .WithMany("Loans")
                        .HasForeignKey("EntityInfoId")
                        .HasConstraintName("Fk_Loans_EntitiesInfo")
                        .IsRequired();

                    b.HasOne("CIT.DataAccess.Models.LenderBusiness", "LenderBusiness")
                        .WithMany("Loans")
                        .HasForeignKey("LenderBusinessId")
                        .HasConstraintName("Fk_Loans_LenderBusiness")
                        .IsRequired();

                    b.Navigation("EntityInfo");

                    b.Navigation("LenderBusiness");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Log", b =>
                {
                    b.HasOne("CIT.DataAccess.Models.LenderBusiness", "LenderBusiness")
                        .WithMany("Logs")
                        .HasForeignKey("LenderBusinessId")
                        .HasConstraintName("Fk_Logs_LenderBusiness")
                        .IsRequired();

                    b.Navigation("LenderBusiness");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Payment", b =>
                {
                    b.HasOne("CIT.DataAccess.Models.Entitiesinfo", "EntityInfo")
                        .WithMany("Payments")
                        .HasForeignKey("EntityInfoId")
                        .HasConstraintName("Fk_Payments_EntitiesInfo")
                        .IsRequired();

                    b.HasOne("CIT.DataAccess.Models.LenderBusiness", "LenderBusiness")
                        .WithMany("Payments")
                        .HasForeignKey("LenderBusinessId")
                        .HasConstraintName("Fk_Payments_LenderBusiness")
                        .IsRequired();

                    b.HasOne("CIT.DataAccess.Models.Loan", "Loan")
                        .WithMany("Payments")
                        .HasForeignKey("LoanId")
                        .HasConstraintName("Fk_Payments_Loans")
                        .IsRequired();

                    b.HasOne("CIT.DataAccess.Models.User", "User")
                        .WithMany("Payments")
                        .HasForeignKey("UserId")
                        .HasConstraintName("Fk_Payments_Users")
                        .IsRequired();

                    b.Navigation("EntityInfo");

                    b.Navigation("LenderBusiness");

                    b.Navigation("Loan");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Role", b =>
                {
                    b.HasOne("CIT.DataAccess.Models.Entitiesinfo", "EntityInfo")
                        .WithMany("Roles")
                        .HasForeignKey("EntityInfoId")
                        .HasConstraintName("Fk_Roles_EntitiesInfo")
                        .IsRequired();

                    b.HasOne("CIT.DataAccess.Models.LenderBusiness", "LenderBusiness")
                        .WithMany("Roles")
                        .HasForeignKey("LenderBusinessId")
                        .HasConstraintName("Fk_Roles_LenderBusiness")
                        .IsRequired();

                    b.Navigation("EntityInfo");

                    b.Navigation("LenderBusiness");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Rolepermission", b =>
                {
                    b.HasOne("CIT.DataAccess.Models.Operation", "Operation")
                        .WithMany("Rolepermissions")
                        .HasForeignKey("OperationId")
                        .HasConstraintName("Fk_RolePermissions_Operations")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CIT.DataAccess.Models.Page", "Page")
                        .WithMany("Rolepermissions")
                        .HasForeignKey("PageId")
                        .HasConstraintName("Fk_RolePermission_Pages")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CIT.DataAccess.Models.Role", "Role")
                        .WithMany("Rolepermissions")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("Fk_RolePermissions_Roles")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Operation");

                    b.Navigation("Page");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.User", b =>
                {
                    b.HasOne("CIT.DataAccess.Models.Entitiesinfo", "EntityInfo")
                        .WithMany("Users")
                        .HasForeignKey("EntityInfoId")
                        .HasConstraintName("Fk_Users_EntitiesInfo")
                        .IsRequired();

                    b.HasOne("CIT.DataAccess.Models.LenderBusiness", "LenderBusiness")
                        .WithMany("Users")
                        .HasForeignKey("LenderBusinessId")
                        .HasConstraintName("Fk_Users_LenderBusiness")
                        .IsRequired();

                    b.Navigation("EntityInfo");

                    b.Navigation("LenderBusiness");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Useraddress", b =>
                {
                    b.HasOne("CIT.DataAccess.Models.Address", "Address")
                        .WithOne("UserAddress")
                        .HasForeignKey("CIT.DataAccess.Models.Useraddress", "AddressId")
                        .HasConstraintName("Fk_UserAddresses_Address")
                        .IsRequired();

                    b.HasOne("CIT.DataAccess.Models.User", "User")
                        .WithOne("Useraddress")
                        .HasForeignKey("CIT.DataAccess.Models.Useraddress", "UserId")
                        .HasConstraintName("Fk_UserAddresses_Users")
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Userrole", b =>
                {
                    b.HasOne("CIT.DataAccess.Models.Entitiesinfo", "EntityInfo")
                        .WithMany("Userroles")
                        .HasForeignKey("EntityInfoId")
                        .HasConstraintName("Fk_UserRoles_EntitiesInfo")
                        .IsRequired();

                    b.HasOne("CIT.DataAccess.Models.Role", "Role")
                        .WithMany("Userroles")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("Fk_UserRoles_RoleId")
                        .IsRequired();

                    b.HasOne("CIT.DataAccess.Models.User", "User")
                        .WithOne("Userrole")
                        .HasForeignKey("CIT.DataAccess.Models.Userrole", "UserId")
                        .HasConstraintName("Fk_UserRoles_UserId")
                        .IsRequired();

                    b.Navigation("EntityInfo");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Vehicle", b =>
                {
                    b.HasOne("CIT.DataAccess.Models.Entitiesinfo", "EntityInfo")
                        .WithMany("Vehicles")
                        .HasForeignKey("EntityInfoId")
                        .HasConstraintName("Fk_Vehicles_EntitiesInfo")
                        .IsRequired();

                    b.HasOne("CIT.DataAccess.Models.LenderBusiness", "LenderBusiness")
                        .WithMany("Vehicles")
                        .HasForeignKey("LenderBusinessId")
                        .HasConstraintName("Fk_Vehicles_LenderBusiness")
                        .IsRequired();

                    b.Navigation("EntityInfo");

                    b.Navigation("LenderBusiness");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.VehicleAssignment", b =>
                {
                    b.HasOne("CIT.DataAccess.Models.User", "User")
                        .WithOne("VehicleAssignment")
                        .HasForeignKey("CIT.DataAccess.Models.VehicleAssignment", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CIT.DataAccess.Models.Vehicle", "Vehicle")
                        .WithOne("VehicleAssignment")
                        .HasForeignKey("CIT.DataAccess.Models.VehicleAssignment", "VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Address", b =>
                {
                    b.Navigation("LenderAddress");

                    b.Navigation("UserAddress");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Entitiesinfo", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("LenderBusinesses");

                    b.Navigation("Loans");

                    b.Navigation("Payments");

                    b.Navigation("Roles");

                    b.Navigation("Userroles");

                    b.Navigation("Users");

                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.LenderBusiness", b =>
                {
                    b.Navigation("LenderAddress");

                    b.Navigation("LenderRole");

                    b.Navigation("Loans");

                    b.Navigation("Logs");

                    b.Navigation("Payments");

                    b.Navigation("Roles");

                    b.Navigation("Users");

                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Loan", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Operation", b =>
                {
                    b.Navigation("Rolepermissions");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Page", b =>
                {
                    b.Navigation("Rolepermissions");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Role", b =>
                {
                    b.Navigation("LenderRoles");

                    b.Navigation("Rolepermissions");

                    b.Navigation("Userroles");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.User", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("Useraddress");

                    b.Navigation("Userrole");

                    b.Navigation("VehicleAssignment");
                });

            modelBuilder.Entity("CIT.DataAccess.Models.Vehicle", b =>
                {
                    b.Navigation("VehicleAssignment");
                });
#pragma warning restore 612, 618
        }
    }
}
