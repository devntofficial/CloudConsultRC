namespace CloudConsult.Consultation.Api;

public static class Routes
{
    private const string Root = "api/v{version:apiVersion}";

    public static class Availability
    {
        public const string Add = Root + "/availability";
        public const string GetByDoctorId = Root + "/doctor/{DoctorId}/availability";
    }

    public static class Consultation
    {
        public const string Request = Root + "/consultations";
        public const string GetById = Root + "/consultations/{ConsultationId}";
        public const string GetByDoctorId = Root + "/doctor/{DoctorId}/consultations";
        public const string Accept = Root + "/consultations/{ConsultationId}/accept";
        public const string Reject = Root + "/consultations/{ConsultationId}/reject";
        public const string Cancel = Root + "/consultations/{ConsultationId}/cancel";
    }
}