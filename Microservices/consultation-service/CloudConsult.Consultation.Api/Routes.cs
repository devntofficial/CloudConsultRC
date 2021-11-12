namespace CloudConsult.Consultation.Api
{
    public static class Routes
    {
        private const string Root = "api/v{version:apiVersion}";

        public static class Availability
        {
            public const string AddAvailability = Root + "/availability";
            public const string GetByDoctorId = Root + "/doctor/{DoctorId}/availability";
        }
        
        public static class Consultation
        {
            public const string BookConsultation = Root + "/consultation";
            public const string GetConsultationById = Root + "/consultation/{ConsultationId}";
        }
    }
}