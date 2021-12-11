using CloudConsult.Common.Enums;
using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Responses;
using CloudConsult.Consultation.Domain.Services;
using CloudConsult.Consultation.Services.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CloudConsult.Consultation.Services.SqlServer.Services
{
    public class ConsultationService : IConsultationService
    {
        private readonly ConsultationDbContext db;

        public ConsultationService(ConsultationDbContext db)
        {
            this.db = db;
        }

        public async Task<ConsultationStatusResponse> Request(ConsultationRequest consultation, CancellationToken cancellationToken = default)
        {
            var timeSlot = await db.DoctorTimeSlots.FirstOrDefaultAsync(x => x.Id == consultation.TimeSlotId, cancellationToken);
            
            var validation = ValidateForRequest(timeSlot);
            if (validation.IsSuccess is false) return validation;

            using var txn = await db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                consultation.Status = ConsultationEvents.ConsultationRequested.ToString();
                await db.ConsultationRequests.AddAsync(consultation, cancellationToken);

                timeSlot.IsBooked = true;
                timeSlot.ConsultationId = consultation.Id;

                await db.ConsultationEvents.AddAsync(new ConsultationEvent
                {
                    ConsultationId = consultation.Id,
                    EventName = ConsultationEvents.ConsultationRequested.ToString(),
                    Timestamp = DateTime.Now
                }, cancellationToken);

                await db.SaveChangesAsync(cancellationToken);
                await txn.CommitAsync(cancellationToken);
                return new ConsultationStatusResponse
                {
                    IsSuccess = true,
                    ConsultationId = consultation.Id,
                    Status = "Consultation request saved successfully"
                };
            }
            catch
            {
                await txn.RollbackAsync(cancellationToken);
                return new ConsultationStatusResponse { Status = "An error occured while saving consultation" };
            }
        }

        public async Task<ConsultationStatusResponse> Accept(string consultationId, CancellationToken cancellationToken = default)
        {
            var consultation = await db.ConsultationRequests.Include(x => x.TimeSlot).Include(x => x.Events)
                .FirstOrDefaultAsync(x => x.Id == consultationId, cancellationToken);

            var validation = ValidateForAccept(consultation);
            if (validation.IsSuccess is false) return validation;

            using var txn = await db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                consultation.Status = ConsultationEvents.ConsultationAccepted.ToString();

                consultation.TimeSlot.IsBooked = true;
                consultation.TimeSlot.ConsultationId = consultation.Id;

                await db.ConsultationEvents.AddAsync(new ConsultationEvent
                {
                    ConsultationId = consultation.Id,
                    EventName = ConsultationEvents.ConsultationAccepted.ToString(),
                    Timestamp = DateTime.Now
                }, cancellationToken);

                await db.SaveChangesAsync(cancellationToken);
                await txn.CommitAsync(cancellationToken);
                return new ConsultationStatusResponse
                {
                    IsSuccess = true,
                    ConsultationId = consultation.Id,
                    Status = "Consultation request accepted by doctor successfully"
                };
            }
            catch
            {
                await txn.RollbackAsync(cancellationToken);
                return new ConsultationStatusResponse { Status = "An error occured while accepting consultation" };
            }
        }

        public async Task<ConsultationStatusResponse> Reject(string consultationId, CancellationToken cancellationToken = default)
        {
            var consultation = await db.ConsultationRequests.Include(x => x.TimeSlot).Include(x => x.Events)
                .FirstOrDefaultAsync(x => x.Id == consultationId, cancellationToken);

            var validation = ValidateForReject(consultation);
            if (validation.IsSuccess is false) return validation;

            using var txn = await db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                consultation.Status = ConsultationEvents.ConsultationRejected.ToString();
                consultation.IsComplete = true;

                consultation.TimeSlot.IsBooked = false;
                consultation.TimeSlot.ConsultationId = null;

                await db.ConsultationEvents.AddAsync(new ConsultationEvent
                {
                    ConsultationId = consultation.Id,
                    EventName = ConsultationEvents.ConsultationRejected.ToString(),
                    Timestamp = DateTime.Now
                }, cancellationToken);

                await db.SaveChangesAsync(cancellationToken);
                await txn.CommitAsync(cancellationToken);
                return new ConsultationStatusResponse
                {
                    IsSuccess = true,
                    ConsultationId = consultation.Id,
                    Status = "Consultation request rejected by doctor successfully"
                };
            }
            catch
            {
                await txn.RollbackAsync(cancellationToken);
                return new ConsultationStatusResponse { Status = "An error occured while rejecting consultation" };
            }
        }

        public async Task<ConsultationStatusResponse> Cancel(string consultationId, CancellationToken cancellationToken = default)
        {
            var consultation = await db.ConsultationRequests.Include(x => x.TimeSlot).Include(x => x.Events)
                .FirstOrDefaultAsync(x => x.Id == consultationId, cancellationToken);

            var validation = ValidateForCancel(consultation);
            if (validation.IsSuccess is false) return validation;


            using var txn = await db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                consultation.Status = ConsultationEvents.ConsultationCancelled.ToString();
                consultation.IsComplete = true;

                consultation.TimeSlot.IsBooked = false;
                consultation.TimeSlot.ConsultationId = null;

                await db.ConsultationEvents.AddAsync(new ConsultationEvent
                {
                    ConsultationId = consultation.Id,
                    EventName = ConsultationEvents.ConsultationCancelled.ToString(),
                    Timestamp = DateTime.Now
                }, cancellationToken);

                await db.SaveChangesAsync(cancellationToken);
                await txn.CommitAsync(cancellationToken);
                return new ConsultationStatusResponse
                {
                    IsSuccess = true,
                    ConsultationId = consultation.Id,
                    Status = "Consultation request cancelled by member successfully"
                };
            }
            catch
            {
                await txn.RollbackAsync(cancellationToken);
                return new ConsultationStatusResponse { Status = "An error occured while cancelling consultation" };
            }
        }

        public async Task<List<ConsultationRequest>> GetByDoctorProfileId(string doctorProfileId, CancellationToken cancellationToken = default)
        {
            return await db.ConsultationRequests.Where(x => x.DoctorProfileId == doctorProfileId)
                .Include(x => x.TimeSlot).Include(x => x.Events)
                .ToListAsync(cancellationToken);
        }

        public async Task<ConsultationRequest> GetById(string consultationId, CancellationToken cancellationToken = default)
        {
            var consultation = await db.ConsultationRequests
                .Include(x => x.TimeSlot).Include(x => x.Events)
                .SingleOrDefaultAsync(x => x.Id == consultationId, cancellationToken);
            return consultation;
        }

        #region Private Methods
        private ConsultationStatusResponse ValidateForAccept(ConsultationRequest consultation)
        {
            if (consultation is null)
            {
                return new ConsultationStatusResponse { Status = "Invalid ConsultationId provided" };
            }

            if (consultation.IsComplete || consultation.Events.Any(x => x.EventName == ConsultationEvents.ConsultationAccepted.ToString()))
            {
                return new ConsultationStatusResponse { Status = "This consultation cannot be accepted anymore" };
            }
            return new ConsultationStatusResponse { IsSuccess = true };
        }

        private ConsultationStatusResponse ValidateForReject(ConsultationRequest consultation)
        {
            if (consultation is null)
            {
                return new ConsultationStatusResponse { Status = "Invalid ConsultationId provided" };
            }

            if (consultation.IsComplete || consultation.Events.Any(x => x.EventName == ConsultationEvents.ConsultationAccepted.ToString()))
            {
                return new ConsultationStatusResponse { Status = "This consultation cannot be rejected anymore" };
            }
            return new ConsultationStatusResponse { IsSuccess = true };
        }

        private ConsultationStatusResponse ValidateForCancel(ConsultationRequest consultation)
        {
            if (consultation is null)
            {
                return new ConsultationStatusResponse { Status = "Invalid ConsultationId provided" };
            }

            if (consultation.IsComplete || consultation.Events.Any(x => x.EventName == ConsultationEvents.PaymentAccepted.ToString()))
            {
                return new ConsultationStatusResponse { Status = "This consultation cannot be cancelled anymore" };
            }
            return new ConsultationStatusResponse { IsSuccess = true };
        }

        private ConsultationStatusResponse ValidateForRequest(DoctorTimeSlot timeSlot)
        {
            if (timeSlot is null)
            {
                return new ConsultationStatusResponse { Status = "Invalid TimeSlotId provided" };
            }

            if (timeSlot.IsBooked)
            {
                return new ConsultationStatusResponse { Status = "This time slot was already booked" };
            }

            return new ConsultationStatusResponse { IsSuccess = true };
        }
        #endregion Private Methods
    }
}