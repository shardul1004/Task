using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MatchBall.ApplicationContext;

public partial class TestDbContext : DbContext
{
    public TestDbContext()
    {
    }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accountballgame> Accountballgames { get; set; }

    public virtual DbSet<Transactionballgame> Transactionballgames { get; set; }

    public virtual DbSet<Usertableballgame> Usertableballgames { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=NIRVANA;database=TestDb;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accountballgame>(entity =>
        {
            entity.HasKey(e => e.Accountid).HasName("PK__ACCOUNTB__F20699F6683C9C13");

            entity.ToTable("ACCOUNTBALLGAME");

            entity.Property(e => e.Accountid).HasColumnName("accountid");
            entity.Property(e => e.Credit).HasColumnName("CREDIT");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<Transactionballgame>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__TRANSACT__9B57CF729D7F3D13");

            entity.ToTable("TRANSACTIONBALLGAME");

            entity.Property(e => e.TransactionId).HasColumnName("transactionId");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Transactiontype)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("transactiontype");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Transactionballgames)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user");
        });

        modelBuilder.Entity<Usertableballgame>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("PK__USERTABL__CBA1B257A791F6F4");

            entity.ToTable("USERTABLEBALLGAME");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Hpassword)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Salt)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("salt");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
