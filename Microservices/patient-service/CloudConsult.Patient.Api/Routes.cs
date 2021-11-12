namespace CloudConsult.Patient.Api
{
    public static class Routes
    {
        private const string Root = "api/v{version:apiVersion}";

        public static class Patient
        {
            public const string CreatePatient = Root + "/patient";
            public const string GetPatientById = Root + "/patient/{PatientId}";
            public const string UpdatePatient = Root + "/patient/{PatientId}";
        }
    }
}
