using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MP.Models;

public partial class PhoneContext : DbContext
{
    public PhoneContext(DbContextOptions<PhoneContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Account { get; set; }

    public virtual DbSet<Cart> Cart { get; set; }

    public virtual DbSet<Format> Format { get; set; }

    public virtual DbSet<Img> Img { get; set; }

    public virtual DbSet<Item> Item { get; set; }

    public virtual DbSet<Order> Order { get; set; }

    public virtual DbSet<OrderItem> OrderItem { get; set; }

    public virtual DbSet<QA> QA { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Account1).HasName("PK__Account__B0C3AC47E3AEAEB6");

            entity.Property(e => e.Account1)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Account");
            entity.Property(e => e.AuthCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Cellphone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Password).IsUnicode(false);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart__3214EC07031FF0EE");

            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.AddTime).HasColumnType("datetime");

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.Cart)
                .HasForeignKey(d => d.Account)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__Account__403A8C7D");

            entity.HasOne(d => d.Item).WithMany(p => p.Cart)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__ItemId__412EB0B6");
        });

        modelBuilder.Entity<Format>(entity =>
        {
            entity.HasKey(e => e.FormatId).HasName("PK__Format__5D3DCB5948F76996");

            entity.Property(e => e.Brand)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Instruction)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Space)
                .HasMaxLength(5)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Img>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Img__3214EC072730533C");

            entity.Property(e => e.ItemImg)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Format).WithMany(p => p.Img)
                .HasForeignKey(d => d.FormatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Img__FormatId__6FE99F9F");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Item__727E838BED3998F9");

            entity.Property(e => e.ItemName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Format).WithMany(p => p.Item)
                .HasForeignKey(d => d.FormatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Item__FormatId__3D5E1FD2");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BCF73228E1C");

            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OrderTime).HasColumnType("datetime");

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.Order)
                .HasForeignKey(d => d.Account)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__Account__440B1D61");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC07D793BA71");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItem)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Order__46E78A0C");
        });

        modelBuilder.Entity<QA>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QA__3214EC07EFE38943");

            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Content)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.Reply)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ReplyTime).HasColumnType("datetime");

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.QA)
                .HasForeignKey(d => d.Account)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QA__Account__49C3F6B7");

            entity.HasOne(d => d.Item).WithMany(p => p.QA)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QA__ItemId__4AB81AF0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
