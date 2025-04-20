using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using IST.Zeiterfassung.Domain.Entities;


namespace IST.Zeiterfassung.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Feiertag> Feiertage => Set<Feiertag>();
        public DbSet<User> Users => Set<User>();
        public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();
        public DbSet<Absence> Absences => Set<Absence>();
        public DbSet<Zeitmodell> Zeitmodelle => Set<Zeitmodell>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TimeEntry>()
                .HasOne(te => te.Erfasser)
                .WithMany()
                .HasForeignKey(te => te.ErfasstVonUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TimeEntry>()
                .HasOne(te => te.Betroffener)
                .WithMany()
                .HasForeignKey(te => te.ErfasstFürUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TimeEntry>()
                .HasOne(te => te.ZeitmodellUser)
                .WithMany()
                .HasForeignKey(te => te.ZeitmodellUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Absence>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }

    }
}