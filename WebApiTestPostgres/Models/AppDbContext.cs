using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApiTestPostgres.Models;

public partial class AppDbContext : DbContext
{

    private readonly IConfiguration _configuration;

    public AppDbContext(IConfiguration configuration, DbContextOptions<AppDbContext> options)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Person> People { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseNpgsql("Host=localhost;Database=testdb;Username=postgres;Password=postgres");

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Addresses_pkey");

            entity.Property(e => e.Id).UseIdentityColumn(); // Configura Id come serial

            entity.Property(e => e.City).HasMaxLength(200);
            entity.Property(e => e.Street).HasMaxLength(200);

            entity.HasOne(d => d.Person).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_person");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Person_pkey");
            entity.Property(e => e.Id).UseIdentityColumn(); // Configura Id come serial

            entity.ToTable("Person");

            entity.Property(e => e.Name).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
