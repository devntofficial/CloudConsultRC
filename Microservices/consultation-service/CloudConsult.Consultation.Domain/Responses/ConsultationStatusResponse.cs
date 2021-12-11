namespace CloudConsult.Consultation.Domain.Responses
{
    public class ConsultationStatusResponse
    {
        public string ConsultationId { get; set; }
        public bool IsSuccess { get; set; } = false;
        public string Status { get; set; }
    }
}
