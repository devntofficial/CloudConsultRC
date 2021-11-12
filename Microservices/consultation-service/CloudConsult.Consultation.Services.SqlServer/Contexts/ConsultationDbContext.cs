using CloudConsult.Consultation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudConsult.Consultation.Services.SqlServer.Contexts
{
    public class ConsultationDbContext : DbContext
    {
        public ConsultationDbContext(DbContextOptions<ConsultationDbContext> options) : base(options)
        {
        }
        
        public DbSet<DoctorAvailabilityEntity> DoctorAvailabilities { get; set; }
        public DbSet<ConsultationBookingEntity> ConsultationBookings { get; set; }
    }
}