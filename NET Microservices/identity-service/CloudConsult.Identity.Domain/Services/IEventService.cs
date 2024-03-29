﻿using CloudConsult.Identity.Domain.Events;

namespace CloudConsult.Identity.Domain.Services
{
    public interface IEventService
    {
        Task<IEnumerable<OtpGenerated>> GetPendingOtpGeneratedEvents(CancellationToken cancellationToken = default);
        void SetOtpGeneratedEventPublished(string eventId, bool value);
    }
}
