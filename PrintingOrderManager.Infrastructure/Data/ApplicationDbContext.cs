using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Core.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PrintingOrderManager.Infrastructure.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; } = null!;
    public virtual DbSet<Equipment> Equipment { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
    public virtual DbSet<Payment> Payments { get; set; } = null!;
    public virtual DbSet<Service> Services { get; set; } = null!;
    public virtual DbSet<Worker> Workers { get; set; } = null!;

    // OnConfiguring НЕ используется, если параметры передаются через DI (из Program.cs)
    // Строка подключения берётся из appsettings.json → ConnectionStrings:DefaultConnection

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK_Clients");

            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.ClientName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Contacts).HasMaxLength(255);
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("PK_Equipment");

            entity.Property(e => e.EquipmentId).HasColumnName("EquipmentID");
            entity.Property(e => e.EquipmentName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.Specifications).HasColumnType("nvarchar(max)");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK_Orders");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.PlacementDate).HasColumnType("date").IsRequired();
            entity.Property(e => e.CompletionDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Client)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Clients");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK_OrderItems");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.WorkerId).HasColumnName("WorkerID");
            entity.Property(e => e.EquipmentId).HasColumnName("EquipmentID");
            entity.Property(e => e.Paper).HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.Cost).HasColumnType("decimal(18,2)").IsRequired();

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Orders");

            entity.HasOne(d => d.Service)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Services");

            entity.HasOne(d => d.Worker)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_OrderItems_Workers");

            entity.HasOne(d => d.Equipment)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.EquipmentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_OrderItems_Equipment");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK_Payments");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.PaymentDate).HasColumnType("date").IsRequired();
            entity.Property(e => e.PaymentStatus).HasMaxLength(50);

            entity.HasOne(d => d.Order)
                .WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Orders");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK_Services");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.ServiceName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasColumnType("nvarchar(max)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.WorkerId).HasName("PK_Workers");

            entity.Property(e => e.WorkerId).HasColumnName("WorkerID");
            entity.Property(e => e.WorkerFullName).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Position).HasMaxLength(100);
            entity.Property(e => e.Contacts).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}