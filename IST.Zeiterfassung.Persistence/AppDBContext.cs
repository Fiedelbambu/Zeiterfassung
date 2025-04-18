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

        public DbSet<User> Users => Set<User>();
        public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();
        public DbSet<Absence> Absences => Set<Absence>();
    }
}
