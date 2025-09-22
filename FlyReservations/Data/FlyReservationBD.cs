namespace FlyReservations.Data
{
    using FlyReservations.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public class FlyReservationBD : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Airplane> Airplanes { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public FlyReservationBD(DbContextOptions<FlyReservationBD> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USER
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.HasOne(u => u.Rol)
                      .WithMany(r => r.Users)
                      .HasForeignKey(u => u.IdRol)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ROL
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.RoleName).IsRequired().HasMaxLength(50);
            });

            // AIRPLANE
            modelBuilder.Entity<Airplane>(entity =>
            {
                entity.HasKey(a => a.Id);
            });

            // SEAT
            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.HasOne(s => s.Airplane)
                      .WithMany(a => a.Seats)
                      .HasForeignKey(s => s.IdPlane)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // AIRPORT
            modelBuilder.Entity<Airport>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.AirportName).IsRequired().HasMaxLength(100);
            });

            // FLIGHT
            modelBuilder.Entity<Flight>(entity =>
            {
                entity.HasKey(f => f.FlightCode);

                entity.HasOne(f => f.OriginAirport)
                      .WithMany(a => a.FlightsOrigin)
                      .HasForeignKey(f => f.OriginAirportId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.DestinationAirport)
                      .WithMany(a => a.FlightsDestination)
                      .HasForeignKey(f => f.DestinationAirportId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.Airplane)
                      .WithMany(a => a.Flights)
                      .HasForeignKey(f => f.IdPlane)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // RESERVATION
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.User)
                      .WithMany(u => u.Reservations)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Flight)
                      .WithMany(f => f.Reservations)
                      .HasForeignKey(r => r.FlightId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Seat)
                      .WithMany(s => s.Reservations)
                      .HasForeignKey(r => r.SeatId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }

}
