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

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Follow> Follows { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderList> OrderLists { get; set; }

    public virtual DbSet<Travel> Travels { get; set; }

    public virtual DbSet<TravelAttraction> TravelAttractions { get; set; }

    public virtual DbSet<TravelSession> TravelSessions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Watch> Watches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chat__3213E83F9E42AB20");

            entity.ToTable("Chat");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.SendTime)
                .HasColumnType("datetime")
                .HasColumnName("send_time");
            entity.Property(e => e.Sender).HasColumnName("sender");
        });

        modelBuilder.Entity<Follow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Follow__3213E83F76C2D996");

            entity.ToTable("Follow");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.TravelId).HasColumnName("travel_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3213E83F2A7E8FBF");

            entity.ToTable("Order");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccountFiveDigits)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("account_five_digits");
            entity.Property(e => e.CheckDate)
                .HasColumnType("datetime")
                .HasColumnName("check_date");
            entity.Property(e => e.CheckStatus).HasColumnName("check_status");
            entity.Property(e => e.Nation)
                .HasMaxLength(255)
                .HasColumnName("nation");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.PayDate)
                .HasColumnType("datetime")
                .HasColumnName("pay_date");
            entity.Property(e => e.PayStatus).HasColumnName("pay_status");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.TravelSessionId).HasColumnName("travel_session_id");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_email");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("user_name");
            entity.Property(e => e.UserPhoneNumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_phone_number");
        });

        modelBuilder.Entity<OrderList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderLis__3213E83F1EC9511B");

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
            entity.HasKey(e => e.Id).HasName("PK__Travel__3213E83FEE20F736");

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
            entity.Property(e => e.Nation)
                .HasMaxLength(255)
                .HasColumnName("nation");
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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83F3C7F683B");

            entity.ToTable("User");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccessToken)
                .IsUnicode(false)
                .HasColumnName("access_token");
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
            entity.Property(e => e.Provider)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("provider");
            entity.Property(e => e.Role)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("role");
        });

        modelBuilder.Entity<Watch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Watch__3213E83F6091AE59");

            entity.ToTable("Watch");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.StayTime).HasColumnName("stay_time");
            entity.Property(e => e.TravelId).HasColumnName("travel_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
