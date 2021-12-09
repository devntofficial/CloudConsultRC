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
        public const string Book = Root + "/consultation";
        public const string GetById = Root + "/consultation/{ConsultationId}";
        public const string GetByDoctorId = Root + "/doctor/{DoctorId}/consultations";
    }
}