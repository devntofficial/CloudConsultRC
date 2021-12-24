﻿namespace CloudConsult.UI.Services.Routes;

public static class GatewayRoutes
{
    public static class IdentityService
    {
        public const string GetToken = "/api/get-token";
        public const string CreateUser = "/api/create-user";
        public const string GetUserRoles = "/api/get-user-roles";
        public const string GenerateOtp = "/api/generate-otp";
        public const string ValidateOtp = "/api/validate-otp";
    }

    public static class DoctorService
    {
        public const string CreateProfile = "/api/create-doctor-profile";
        public const string GetProfileById = "/api/get-doctor-profile/{ProfileId}";
        public const string GetProfileByIdentityId = "/api/get-doctor-profile-by-identity/{IdentityId}";
        public const string UpdateProfile = "/api/update-doctor-profile/{ProfileId}";
        public const string KycUpload = "/api/upload-doctor-kyc-documents/{ProfileId}";
    }

    public static class ConsultationService
    {
        public const string AddAvailableTimeslots = "/api/add-available-timeslots";
        public const string GetAvailableTimeslotById = "/api/get-available-timeslots/{ProfileId}";
    }
}