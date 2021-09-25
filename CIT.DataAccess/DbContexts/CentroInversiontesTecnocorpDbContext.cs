using System;
using CIT.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CIT.DataAccess.DbContexts
{
    public partial class CentroInversiontesTecnocorpDbContext : DbContext
    {
        public CentroInversiontesTecnocorpDbContext()
        {
        }

        public CentroInversiontesTecnocorpDbContext(DbContextOptions<CentroInversiontesTecnocorpDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Entitiesinfo> Entitiesinfos { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Rolepermission> Rolepermissions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Useraddress> Useraddresses { get; set; }
        public virtual DbSet<Userrole> Userroles { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("addresses");

                entity.HasIndex(e => e.EntityInfoId, "Fk_Addresses_EntitiesInfo");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EntityInfoId).HasColumnType("int(11)");

                entity.Property(e => e.HouseNumber).HasColumnType("int(11)");

                entity.Property(e => e.Latitude).HasPrecision(10, 2);

                entity.Property(e => e.Longitude).HasPrecision(10, 2);

                entity.Property(e => e.Province)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Street1)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Street2).HasMaxLength(150);

                entity.HasOne(d => d.EntityInfo)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.EntityInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Addresses_EntitiesInfo");
            });

            modelBuilder.Entity<Entitiesinfo>(entity =>
            {
                entity.ToTable("entitiesinfo");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Status).HasColumnType("smallint(6)");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.ToTable("loans");

                entity.HasIndex(e => e.EntityInfoId, "Fk_Loans_EntitiesInfo");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.DuesQuantity).HasColumnType("int(11)");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.EntityInfoId).HasColumnType("int(11)");

                entity.Property(e => e.InterestRate).HasPrecision(3, 2);

                entity.Property(e => e.MensualPay).HasPrecision(13, 2);

                entity.Property(e => e.PayDay).HasColumnType("int(11)");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.TotalLoan).HasPrecision(13, 2);

                entity.HasOne(d => d.EntityInfo)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.EntityInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Loans_EntitiesInfo");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("logs");

                entity.HasIndex(e => e.UserId, "Fk_Logs_Users");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.LogDate).HasColumnType("datetime");

                entity.Property(e => e.Operation)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ResultMessageOrObject).HasMaxLength(255);

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Logs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Logs_Users");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.UserId, e.LoanId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("payments");

                entity.HasIndex(e => e.EntityInfoId, "Fk_Payments_EntitiesInfo");

                entity.HasIndex(e => e.LoanId, "Fk_Payments_Loans");

                entity.HasIndex(e => e.UserId, "Fk_Payments_Users");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.LoanId).HasColumnType("int(11)");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.EntityInfoId).HasColumnType("int(11)");

                entity.Property(e => e.Pay).HasPrecision(13, 2);

                entity.HasOne(d => d.EntityInfo)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.EntityInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Payments_EntitiesInfo");

                entity.HasOne(d => d.Loan)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.LoanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Payments_Loans");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Payments_Users");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.HasIndex(e => e.EntityInfoId, "Fk_Roles_EntitiesInfo");

                entity.HasIndex(e => e.RoleName, "RoleName")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.EntityInfoId).HasColumnType("int(11)");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(d => d.EntityInfo)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.EntityInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Roles_EntitiesInfo");
            });

            modelBuilder.Entity<Rolepermission>(entity =>
            {
                entity.ToTable("rolepermissions");

                entity.HasIndex(e => e.RoleId, "Fk_RolePermissions_Roles");

                entity.HasIndex(e => e.PermissionName, "PermissionName")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.PermissionName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.RoleId).HasColumnType("int(11)");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Rolepermissions)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_RolePermissions_Roles");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "Email")
                    .IsUnique();

                entity.HasIndex(e => e.EntityInfoId, "Fk_Users_EntitiesInfo");

                entity.HasIndex(e => e.IdentificationDocument, "IdentificationDocument")
                    .IsUnique();

                entity.HasIndex(e => e.Phone, "Phone")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EntityInfoId).HasColumnType("int(11)");

                entity.Property(e => e.IdentificationDocument)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Photo).HasMaxLength(255);

                entity.HasOne(d => d.EntityInfo)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.EntityInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Users_EntitiesInfo");
            });

            modelBuilder.Entity<Useraddress>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.UserId, e.AddressId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("useraddresses");

                entity.HasIndex(e => e.AddressId, "Fk_UserAddresses_Address");

                entity.HasIndex(e => e.UserId, "Fk_UserAddresses_Users");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.AddressId).HasColumnType("int(11)");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Useraddresses)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_UserAddresses_Address");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Useraddresses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_UserAddresses_Users");
            });

            modelBuilder.Entity<Userrole>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.RoleId, e.UserId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("userroles");

                entity.HasIndex(e => e.EntityInfoId, "Fk_UserRoles_EntitiesInfo");

                entity.HasIndex(e => e.RoleId, "RoleId")
                    .IsUnique();

                entity.HasIndex(e => e.UserId, "UserId")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.RoleId).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.EntityInfoId).HasColumnType("int(11)");

                entity.HasOne(d => d.EntityInfo)
                    .WithMany(p => p.Userroles)
                    .HasForeignKey(d => d.EntityInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_UserRoles_EntitiesInfo");

                entity.HasOne(d => d.Role)
                    .WithOne(p => p.Userrole)
                    .HasForeignKey<Userrole>(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_UserRoles_RoleId");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Userrole)
                    .HasForeignKey<Userrole>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_UserRoles_UserId");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("vehicles");

                entity.HasIndex(e => e.Enrollment, "Enrollment")
                    .IsUnique();

                entity.HasIndex(e => e.EntityInfoId, "Fk_Vehicles_EntitiesInfo");

                entity.HasIndex(e => e.LicensePlate, "LicensePlate")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Enrollment)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.EntityInfoId).HasColumnType("int(11)");

                entity.Property(e => e.LicensePlate)
                    .IsRequired()
                    .HasMaxLength(7);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Year).HasColumnType("int(11)");

                entity.HasOne(d => d.EntityInfo)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.EntityInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Vehicles_EntitiesInfo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
