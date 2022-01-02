using CloudConsult.Consultation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudConsult.Consultation.Services.SqlServer.Contexts
{
    public class ConsultationDbContext : DbContext
    {
        public ConsultationDbContext(DbContextOptions<ConsultationDbContext> options) : base(options)
        {
        }
        
        public DbSet<DoctorTimeSlot> DoctorTimeSlots { get; set; }
        public DbSet<ConsultationRequest> ConsultationRequests { get; set; }
        public DbSet<ConsultationEvent> ConsultationEvents { get; set; }
    }
}