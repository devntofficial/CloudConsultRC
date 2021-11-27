namespace CloudConsult.Consultation.Domain.Events
{
    public record ConsultationBooked
    {
        public string Id { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string BookingDate { get; set; }
        public string BookingTimeSlot { get; set; }
    }
}