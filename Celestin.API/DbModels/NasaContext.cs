using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Celestin.API.DbModels
{
    public partial class NasaContext : DbContext
    {
        public NasaContext()
        {
        }

        public NasaContext(DbContextOptions<NasaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Celestin> Celestin { get; set; }
        public virtual DbSet<DiscoverySource> DiscoverySource { get; set; }
        public virtual DbSet<Type> Type { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=EN1210883\\CAZAC;Database=Nasa;Trusted_Connection=True;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Celestin>(entity =>
            {
                entity.Property(e => e.DiscoveryDate).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.SurfaceTemperature).HasColumnName("Surface Temperature");

                entity.HasOne(d => d.DiscoverySource)
                    .WithMany(p => p.Celestin)
                    .HasForeignKey(d => d.DiscoverySourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiscoverySource_Celestin");
            });

            modelBuilder.Entity<DiscoverySource>(entity =>
            {
                entity.Property(e => e.EstablishmentDate).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.StateOwner)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.DiscoverySource)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Type_DiscoverySources");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(200);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
