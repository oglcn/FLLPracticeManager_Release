using FLLPracticeManager.Entities;
using Microsoft.EntityFrameworkCore;

namespace FLLPracticeManager.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationSlot> ReservationSlots { get; set; }

    }
}
