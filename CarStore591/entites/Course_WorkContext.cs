using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CarStore591
{
    public partial class Course_WorkContext : DbContext
    {
        public Course_WorkContext()
        {
        }

        public Course_WorkContext(DbContextOptions<Course_WorkContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarService> CarServices { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ClientFullName> ClientFullNames { get; set; }
        public virtual DbSet<CreditingDebiting> CreditingDebitings { get; set; }
        public virtual DbSet<CurrentBalance> CurrentBalances { get; set; }
        public virtual DbSet<ProvisionOfService> ProvisionOfServices { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-12RH4JAG;Database=Course_Work;Trusted_Connection=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Car>(entity =>
            {
                entity.Property(e => e.CarCost).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CarName)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CarService>(entity =>
            {
                entity.HasKey(e => e.Number)
                    .HasName("PK__Car_Serv__78A1A19CFF78F951");

                entity.ToTable("Car_Service");

                entity.Property(e => e.Service)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ServiceCost)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("Service_Cost");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.UserCode)
                    .HasName("PK__Client__3E6D1F353545DB31");

                entity.ToTable("Client");

                entity.HasIndex(e => e.PhoneNumber, "UQ__Client__17A35CA484C3A3D0")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Client__A9D10534954F7D3A")
                    .IsUnique();

                entity.Property(e => e.UserCode).HasColumnName("User_Code");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.FullNameIndex).HasColumnName("Full_Name_Index");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("Phone_Number");

                entity.HasOne(d => d.FullNameIndexNavigation)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.FullNameIndex)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Client__Full_Nam__286302EC");
            });

            modelBuilder.Entity<ClientFullName>(entity =>
            {
                entity.HasKey(e => e.NameIndex)
                    .HasName("PK__Client_F__4CA09F33285F8611");

                entity.ToTable("Client_Full_Name");

                entity.Property(e => e.NameIndex).HasColumnName("Name_Index");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Patronymic)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<CreditingDebiting>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Crediting_Debiting");

                entity.Property(e => e.CreditingDebiting1).HasColumnName("Crediting_Debiting");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.UserCode).HasColumnName("User_Code");

                entity.HasOne(d => d.UserCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.UserCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Crediting__User___2A4B4B5E");
            });

            modelBuilder.Entity<CurrentBalance>(entity =>
            {
                entity.HasKey(e => e.UserCode)
                    .HasName("PK__Current___3E6D1F3503FEAC5F");

                entity.ToTable("Current_Balance");

                entity.Property(e => e.UserCode)
                    .ValueGeneratedNever()
                    .HasColumnName("User_Code");

                entity.Property(e => e.CurrentBalance1).HasColumnName("Current_Balance");

                entity.HasOne(d => d.UserCodeNavigation)
                    .WithOne(p => p.CurrentBalance)
                    .HasForeignKey<CurrentBalance>(d => d.UserCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Current_B__User___2D27B809");
            });

            modelBuilder.Entity<ProvisionOfService>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Provision_of_services");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.UserCode).HasColumnName("User_Code");

                entity.HasOne(d => d.NumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Number)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Provision__Numbe__31EC6D26");

                entity.HasOne(d => d.UserCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.UserCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Provision__User___30F848ED");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.Property(e => e.SaleId).HasColumnName("Sale_Id");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.UserCode).HasColumnName("User_Code");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sales__CarId__37A5467C");

                entity.HasOne(d => d.UserCodeNavigation)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.UserCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sales__User_Code__36B12243");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
