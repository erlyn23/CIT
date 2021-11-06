using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CIT.DataAccess.Models;

#nullable disable

namespace CIT.DataAccess.DbContexts
{
    public partial class CentroInversionesTecnocorpDbContext : DbContext
    {
        public CentroInversionesTecnocorpDbContext()
        {
        }

        public CentroInversionesTecnocorpDbContext(DbContextOptions<CentroInversionesTecnocorpDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Entitiesinfo> Entitiesinfos { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Operation> Operations { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Rolepermission> Rolepermissions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Useraddress> Useraddresses { get; set; }
        public virtual DbSet<Userrole> Userroles { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }

        public virtual DbSet<LenderBusiness> LenderBusiness { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LenderBusiness>(entity =>
            {
                entity.ToTable("lenderbusiness");

                entity.HasIndex(e => e.Rnc, "Ix_LenderBusiness_Rnc").IsUnique();

                entity.HasIndex(e => e.Email, "Ix_LenderBusiness_Email").IsUnique();

                entity.HasIndex(e => e.Phone, "Ix_LenderBusiness_Phone").IsUnique();

                entity.HasIndex(e => e.EntityInfoId, "Fk_LenderBusinesses_EntitiesInfo");

                entity.Property(e => e.Id).HasColumnType("int").UseMySqlIdentityColumn();

                entity.Property(e => e.Rnc).IsRequired().HasMaxLength(20);

                entity.Property(e => e.Email).IsRequired().HasMaxLength(50);
                
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);

                entity.Property(e => e.Photo).HasMaxLength(255);

                entity.Property(e => e.EntityInfoId).HasColumnType("int");

                entity.Property(e => e.Password).IsRequired().HasMaxLength(255);

                entity.HasOne(d => d.EntityInfo)
                    .WithMany(p => p.LenderBusinesses)
                    .HasForeignKey(e => e.EntityInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_LenderBusiness_EntitiesInfo");

            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("addresses");

                entity.HasIndex(e => e.EntityInfoId, "Fk_Addresses_EntitiesInfo");

                entity.Property(e => e.Id).HasColumnType("int").UseMySqlIdentityColumn();

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EntityInfoId)
                    .IsRequired().HasColumnType("int(11)");

                entity.Property(e => e.HouseNumber).HasColumnType("int(11)");

                entity.Property(e => e.Latitude).HasColumnType("decimal(10,2)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(10,2)");

                entity.Property(e => e.Province)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Street1)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Street2)
                    .HasMaxLength(150)
                    .HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.EntityInfo)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.EntityInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Addresses_EntitiesInfo");
            });

            modelBuilder.Entity<Entitiesinfo>(entity =>
            {
                entity.ToTable("entitiesinfo");

                entity.Property(e => e.Id).HasColumnType("int").UseMySqlIdentityColumn();

                entity.Property(e => e.Status).HasColumnType("smallint(6)");
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.ToTable("loans");

                entity.HasIndex(e => e.EntityInfoId, "Fk_Loans_EntitiesInfo");
                
                entity.HasIndex(e => e.LenderBusinessId, "Fk_Loans_LenderBusiness");

                entity.Property(e => e.Id).HasColumnType("int").UseMySqlIdentityColumn();

                entity.Property(e => e.DuesQuantity).HasColumnType("int(11)");

                entity.Property(e => e.EntityInfoId)
                    .IsRequired().HasColumnType("int(11)");

                entity.Property(e => e.InterestRate).HasColumnType("decimal(3,2)");

                entity.Property(e => e.MensualPay).HasColumnType("decimal(13,2)");

                entity.Property(e => e.PayDay).HasColumnType("int(11)");

                entity.Property(e => e.TotalLoan).HasColumnType("decimal(13,2)");

                entity.HasOne(d => d.EntityInfo)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.EntityInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Loans_EntitiesInfo");
                
                entity.HasOne(d => d.LenderBusiness)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.LenderBusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Loans_LenderBusiness");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("logs");

                entity.HasIndex(e => e.UserId, "Fk_Logs_Users");
                
                entity.HasIndex(e => e.LenderBusinessId, "Fk_Logs_LenderBusiness");

                entity.Property(e => e.Id).HasColumnType("int").UseMySqlIdentityColumn();

                entity.Property(e => e.Operation)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ResultMessageOrObject)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Logs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Logs_Users");

                entity.HasOne(d => d.LenderBusiness)
                    .WithMany(p => p.Logs)
                    .HasForeignKey(d => d.LenderBusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Logs_LenderBusiness");
            });

            modelBuilder.Entity<Operation>(entity =>
            {
                entity.ToTable("operations");

                entity.Property(e => e.Id).HasColumnType("int").UseMySqlIdentityColumn();

                entity.Property(e => e.OperationName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.ToTable("pages");

                entity.Property(e => e.Id).HasColumnType("int").UseMySqlIdentityColumn();

                entity.Property(e => e.PageName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.UserId, e.LoanId })
                    .HasName("PRIMARY");

                entity.ToTable("payments");

                entity.HasIndex(e => e.EntityInfoId, "Fk_Payments_EntitiesInfo");
                
                entity.HasIndex(e => e.LenderBusinessId, "Fk_Payments_LenderBusiness");

                entity.HasIndex(e => e.LoanId, "Fk_Payments_Loans");

                entity.HasIndex(e => e.UserId, "Fk_Payments_Users");

                entity.Property(e => e.Id).HasColumnType("int").UseMySqlIdentityColumn();

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.LoanId).HasColumnType("int(11)");

                entity.Property(e => e.EntityInfoId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Pay).HasColumnType("decimal(13,2)");

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

                entity.HasOne(d => d.LenderBusiness)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.LenderBusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Payments_LenderBusiness");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.HasIndex(e => e.EntityInfoId, "Fk_Roles_EntitiesInfo");
                
                entity.HasIndex(e => e.LenderBusinessId, "Fk_Roles_LenderBusiness");

                entity.HasIndex(e => e.RoleName, "RoleName")
                    .IsUnique();


                entity.Property(e => e.Id).HasColumnType("int").UseMySqlIdentityColumn();

                entity.Property(e => e.EntityInfoId).HasColumnType("int(11)");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(d => d.EntityInfo)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.EntityInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Roles_EntitiesInfo");

                entity.HasOne(d => d.LenderBusiness)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.LenderBusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Roles_LenderBusiness");

            });

            modelBuilder.Entity<Rolepermission>(entity =>
            {
                entity.ToTable("rolepermissions");

                entity.HasIndex(e => e.PageId, "Fk_RolePermission_Pages");

                entity.HasIndex(e => e.RoleId, "Fk_RolePermissions_Roles");

                entity.HasIndex(e => e.OperationId, "Fk_RolePermissions_Operations");

                entity.HasKey(e => new { e.Id, e.OperationId, e.PageId }).HasName("PRIMARY");

                entity.Property(e => e.Id).HasColumnType("int").UseMySqlIdentityColumn();

                entity.Property(e => e.OperationId)
                    .IsRequired().HasColumnType("int");

                entity.Property(e => e.PageId)
                    .IsRequired().HasColumnType("int");

                entity.Property(e => e.RoleId)
                    .IsRequired().HasColumnType("int");

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.Rolepermissions)
                    .HasForeignKey(d => d.OperationId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("Fk_RolePermissions_Operations");

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.Rolepermissions)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("Fk_RolePermission_Pages");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Rolepermissions)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("Fk_RolePermissions_Roles");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "Email")
                    .IsUnique();

                entity.HasIndex(e => e.EntityInfoId, "Fk_Users_EntitiesInfo");

                entity.HasIndex(e => e.LenderBusinessId, "Fk_Users_LenderBusiness");

                entity.HasIndex(e => e.IdentificationDocument, "IdentificationDocument")
                    .IsUnique();

                entity.HasIndex(e => e.Phone, "Phone")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int").UseMySqlIdentityColumn(); 

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EntityInfoId)
                    .IsRequired().HasColumnType("int");

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

                entity.Property(e => e.Photo)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.EntityInfo)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.EntityInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Users_EntitiesInfo");

                entity.HasOne(d => d.LenderBusiness)
                      .WithMany(p => p.Users)
                      .HasForeignKey(d => d.LenderBusinessId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("Fk_Users_LenderBusiness");
            });

            modelBuilder.Entity<Useraddress>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.UserId, e.AddressId })
                    .HasName("PRIMARY");

                entity.ToTable("useraddresses");

                entity.HasIndex(e => e.AddressId, "Fk_UserAddresses_Address");

                entity.HasIndex(e => e.UserId, "Fk_UserAddresses_Users").IsUnique();

                entity.Property(e => e.Id).HasColumnType("int").UseMySqlIdentityColumn();

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.AddressId).HasColumnType("int(11)");

                entity.HasOne(d => d.Address)
                    .WithOne(p => p.UserAddress)
                    .HasForeignKey<Useraddress>(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_UserAddresses_Address");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserAddress)
                    .HasForeignKey<Useraddress>(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_UserAddresses_Users");
            });

            modelBuilder.Entity<Userrole>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.RoleId, e.UserId })
                    .HasName("PRIMARY");

                entity.ToTable("userroles");

                entity.HasIndex(e => e.EntityInfoId, "Fk_UserRoles_EntitiesInfo");

                entity.HasIndex(e => e.RoleId, "RoleId");

                entity.HasIndex(e => e.UserId, "UserId")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int").UseMySqlIdentityColumn();

                entity.Property(e => e.RoleId).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.EntityInfoId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.EntityInfo)
                    .WithMany(p => p.Userroles)
                    .HasForeignKey(d => d.EntityInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_UserRoles_EntitiesInfo");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Userroles)
                    .HasForeignKey(d => d.RoleId)
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

                entity.HasIndex(e => e.LenderBusinessId, "Fk_Vehicles_LenderBusiness");

                entity.HasIndex(e => e.LicensePlate, "LicensePlate")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int").UseMySqlIdentityColumn();

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Enrollment)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.EntityInfoId)
                    .IsRequired().HasColumnType("int");

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

                entity.HasOne(d => d.LenderBusiness)
                      .WithMany(p => p.Vehicles)
                      .HasForeignKey(d => d.LenderBusinessId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("Fk_Vehicles_LenderBusiness");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
