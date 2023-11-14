using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RoadEventsProject.Models.Data;

public partial class RoadEventsContext : DbContext
{
    public RoadEventsContext()
    {
    }

    public RoadEventsContext(DbContextOptions<RoadEventsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CityVillage> CityVillages { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Name> Names { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<RoadEvent> RoadEvents { get; set; }

    public virtual DbSet<RoleUser> RoleUsers { get; set; }

    public virtual DbSet<StatusEvent> StatusEvents { get; set; }

    public virtual DbSet<TypeViolation> TypeViolations { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<Violation> Violations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=Olha_Makarchuk\\SQLEXPRESS; Database =RoadEvents;Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CityVillage>(entity =>
        {
            entity.HasKey(e => e.IdCityVillage).HasName("PK_IdCityVillage");

            entity.ToTable("CityVillage");

            entity.Property(e => e.NameCityVillage)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRegionNavigation).WithMany(p => p.CityVillages)
                .HasForeignKey(d => d.IdRegion)
                .HasConstraintName("FK_CityVillage_Region");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.IdDriver).HasName("PK_IdDriver");

            entity.ToTable("Driver");

            entity.Property(e => e.IpnNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IPN_number");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.IdImage).HasName("PK_IdImage");

            entity.Property(e => e.ImageUrl)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ImageURL");
        });

        modelBuilder.Entity<Name>(entity =>
        {
            entity.HasKey(e => e.IdName).HasName("PK_IdName");

            entity.ToTable("Name");

            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.IdRegion).HasName("PK_IdRegion");

            entity.ToTable("Region");

            entity.Property(e => e.NameRegion)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RoadEvent>(entity =>
        {
            entity.HasKey(e => e.IdRoadEvent).HasName("PK_IdRoadEvent");

            entity.ToTable("RoadEvent");

            entity.Property(e => e.DateEvent).HasColumnType("datetime");
            entity.Property(e => e.DescriptionEvent)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCityVillageNavigation).WithMany(p => p.RoadEvents)
                .HasForeignKey(d => d.IdCityVillage)
                .HasConstraintName("FK_RoadEvent_CityVillage");

            entity.HasOne(d => d.IdImageNavigation).WithMany(p => p.RoadEvents)
                .HasForeignKey(d => d.IdImage)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RoadEvent_Images");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.RoadEvents)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("FK_RoadEvent_StatusEvent");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.RoadEvents)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_RoadEvent_UserInfo");

            entity.HasOne(d => d.IdVideoNavigation).WithMany(p => p.RoadEvents)
                .HasForeignKey(d => d.IdVideo)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RoadEvent_Videos");
        });

        modelBuilder.Entity<RoleUser>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK_IdRole");

            entity.ToTable("RoleUser");

            entity.Property(e => e.NameRole)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StatusEvent>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PK_IdStatus");

            entity.ToTable("StatusEvent");

            entity.Property(e => e.NameStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TypeViolation>(entity =>
        {
            entity.HasKey(e => e.IdType).HasName("PK_IdType");

            entity.ToTable("TypeViolation");

            entity.Property(e => e.NameType)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.IdViolationNavigation).WithMany(p => p.TypeViolations)
                .HasForeignKey(d => d.IdViolation)
                .HasConstraintName("FK_TypeViolation_Violation");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK_IdUser");

            entity.ToTable("UserInfo");

            entity.Property(e => e.LoginUser)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdNameNavigation).WithMany(p => p.UserInfos)
                .HasForeignKey(d => d.IdName)
                .HasConstraintName("FK_UserInfo_Name");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.UserInfos)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("FK_UserInfo_RoleUser");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.IdVehicle).HasName("PK_IdVehicle");

            entity.ToTable("Vehicle");

            entity.Property(e => e.NumberCar)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDriverNavigation).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.IdDriver)
                .HasConstraintName("FK_Vehicle_Driver");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.IdVideo).HasName("PK_IdVideo");

            entity.Property(e => e.VideoUrl)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VideoURL");
        });

        modelBuilder.Entity<Violation>(entity =>
        {
            entity.HasKey(e => e.IdViolation).HasName("PK_IdViolation");

            entity.ToTable("Violation");

            entity.Property(e => e.DateEvent).HasColumnType("datetime");
            entity.Property(e => e.Fine).HasColumnType("decimal(7, 2)");

            entity.HasOne(d => d.IdCityVillageNavigation).WithMany(p => p.Violations)
                .HasForeignKey(d => d.IdCityVillage)
                .HasConstraintName("FK_Violation_CityVillage");

            entity.HasOne(d => d.IdDriverNavigation).WithMany(p => p.Violations)
                .HasForeignKey(d => d.IdDriver)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Violation_Driver");

            entity.HasOne(d => d.IdRoadEventNavigation).WithMany(p => p.Violations)
                .HasForeignKey(d => d.IdRoadEvent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Violation_RoadEvent");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Violations)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_Violation_UserInfo");

            entity.HasOne(d => d.IdVehicleNavigation).WithMany(p => p.Violations)
                .HasForeignKey(d => d.IdVehicle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Violation_Vehicle");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

