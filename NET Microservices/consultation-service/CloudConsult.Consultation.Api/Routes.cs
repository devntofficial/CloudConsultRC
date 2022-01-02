namespace CloudConsult.Consultation.Api;

public static class Routes
{
    private const string Root = "api/v{version:apiVersion}";

    public static class TimeSlot
    {
        public const string Add = Root + "/timeslots";
        public const string AddSingle = Root + "/doctor/{ProfileId}/timeslot";
        public const string GetByDoctorId = Root + "/doctor/{DoctorId}/timeslots/all";
        public const string GetByRange = Root + "/doctor/{ProfileId}/timeslots/range";
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