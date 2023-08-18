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

    public virtual DbSet<Follow> Follows { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderList> OrderLists { get; set; }

    public virtual DbSet<Travel> Travels { get; set; }

    public virtual DbSet<TravelAttraction> TravelAttractions { get; set; }

    public virtual DbSet<TravelSession> TravelSessions { get; set; }

    public virtual DbSet<Watch> Watches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Follow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Follow__3213E83FA92D3919");

            entity.ToTable("Follow");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.TravelId).HasColumnName("travel_id");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Member__3213E83F2351CA01");

            entity.ToTable("Member");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Birthday)
                .HasColumnType("date")
                .HasColumnName("birthday");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.Region)
                .HasMaxLength(255)
                .HasColumnName("region");
            entity.Property(e => e.Sex)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("sex");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3213E83F416008B8");

            entity.ToTable("Order");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.TravelSessionId).HasColumnName("travel_session_id");
        });

        modelBuilder.Entity<OrderList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderLis__3213E83FFA58A766");

            entity.ToTable("OrderList");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Birthday)
                .HasColumnType("date")
                .HasColumnName("birthday");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.IdentityCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("identity_code");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("passport_number");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Sex)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("sex");
            entity.Property(e => e.SpecialNeed)
                .HasMaxLength(255)
                .HasColumnName("special_need");
        });

        modelBuilder.Entity<Travel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Travel__3213E83F9C3567FE");

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
            entity.HasKey(e => e.Id).HasName("PK__TravelAt__3213E83F9685ED7E");

            entity.ToTable("TravelAttraction");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Attraction)
                .HasMaxLength(255)
                .HasColumnName("attraction");
            entity.Property(e => e.TravelId).HasColumnName("travel_id");
        });

        modelBuilder.Entity<TravelSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TravelSe__3213E83F8AE85B38");

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
        });

        modelBuilder.Entity<Watch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Watch__3213E83FDE9805AB");

            entity.ToTable("Watch");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.StayTime).HasColumnName("stay_time");
            entity.Property(e => e.TravelId).HasColumnName("travel_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
