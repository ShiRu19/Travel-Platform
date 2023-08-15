using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TravelPlatform.Models.Domain;

public partial class TravelContext : DbContext
{
    public TravelContext(DbContextOptions<TravelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Travel> Travels { get; set; }

    public virtual DbSet<TravelAttraction> TravelAttractions { get; set; }

    public virtual DbSet<TravelSession> TravelSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Travel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Travel__3213E83FD3F89C89");

            entity.ToTable("Travel");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DateRangeEnd)
                .HasColumnType("date")
                .HasColumnName("date_range_end");
            entity.Property(e => e.DateRangeStart)
                .HasColumnType("date")
                .HasColumnName("date_range_start");
            entity.Property(e => e.Days).HasColumnName("days");
            entity.Property(e => e.DepartureLocation)
                .HasMaxLength(255)
                .HasColumnName("departure_location");
            entity.Property(e => e.MainImageUrl)
                .HasMaxLength(255)
                .HasColumnName("main_image_url");
            entity.Property(e => e.PdfUrl)
                .HasMaxLength(255)
                .HasColumnName("pdf_url");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<TravelAttraction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TravelAt__3213E83F39E2AE08");

            entity.ToTable("TravelAttraction");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Attraction)
                .HasMaxLength(255)
                .HasColumnName("attraction");
            entity.Property(e => e.TravelId).HasColumnName("travel_id");

            entity.HasOne(d => d.Travel).WithMany(p => p.TravelAttractions)
                .HasForeignKey(d => d.TravelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TravelAtt__trave__412EB0B6");
        });

        modelBuilder.Entity<TravelSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TravelSe__3213E83F1E718C26");

            entity.ToTable("TravelSession");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DepartureDate)
                .HasColumnType("date")
                .HasColumnName("departure_date");
            entity.Property(e => e.GroupStatus).HasColumnName("group_status");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductNumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("product_number");
            entity.Property(e => e.RemainingSeats).HasColumnName("remaining_seats");
            entity.Property(e => e.Seats).HasColumnName("seats");
            entity.Property(e => e.TravelId).HasColumnName("travel_id");

            entity.HasOne(d => d.Travel).WithMany(p => p.TravelSessions)
                .HasForeignKey(d => d.TravelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TravelSes__trave__440B1D61");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
