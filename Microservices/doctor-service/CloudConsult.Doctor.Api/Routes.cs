namespace CloudConsult.Doctor.Api
{
    public static class Routes
    {
        private const string Root = "api/v{version:apiVersion}";

        public static class Doctor
        {
            public const string CreateDoctor = Root + "/doctor";
            public const string GetDoctorById = Root + "/doctor/{DoctorId}";
            public const string UpdateDoctor = Root + "/doctor/{DoctorId}";
        }
    }
}