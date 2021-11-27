namespace CloudConsult.Doctor.Api
{
    public static class Routes
    {
        private const string Root = "api/v{version:apiVersion}";

        public static class Profile
        {
            public const string CreateProfile = Root + "/doctor/profile";
            public const string UpdateProfile = Root + "/doctor/profile/{ProfileId}";
            public const string GetProfileById = Root + "/doctor/profile/{ProfileId}";
            public const string GetProfileByIdentityId = Root + "/doctor/profile/identity/{IdentityId}";
        }
    }
}