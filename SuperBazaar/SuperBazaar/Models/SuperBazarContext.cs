using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SuperBazaar.Models;

public partial class SuperBazarContext : DbContext
{
    public SuperBazarContext()
    {
    }

    public SuperBazarContext(DbContextOptions<SuperBazarContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Superbazaruser> Superbazarusers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=NIRVANA;database=SuperBazar;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Itemid).HasName("PK__item__56A22C925BD3EBCA");

            entity.ToTable("item");

            entity.Property(e => e.Itemid).HasColumnName("itemid");
            entity.Property(e => e.Itemname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("itemname");
            entity.Property(e => e.Itemprice).HasColumnName("itemprice");
            entity.Property(e => e.Itemquantity).HasColumnName("itemquantity");
        });

        modelBuilder.Entity<Superbazaruser>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("PK__superbaz__CBA1B257145BBFB8");

            entity.ToTable("superbazaruser");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("username");
            entity.Property(e => e.Userphone).HasColumnName("userphone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
