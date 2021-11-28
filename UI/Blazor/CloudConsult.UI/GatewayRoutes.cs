namespace CloudConsult.UI;

public static class GatewayRoutes
{
    public static class IdentityService
    {
        public const string GetToken = "/api/get-token";
        public const string CreateUser = "/api/create-user";
        public const string GetUserRoles = "/api/get-user-roles";
    }

    public static class DoctorService
    {
        public const string CreateProfile = "/api/create-doctor-profile";
        public const string GetProfileById = "/api/get-doctor-profile/{DoctorId}";
        public const string UpdateProfile = "/api/update-doctor-profile/{DoctorId}";
    }

    public static class ConsultationService
    {
        public const string AddAvailableTimeslots = "/api/add-available-timeslots";
        public const string GetAvailableTimeslotById = "/api/get-available-timeslots/{DoctorId}";
    }
}