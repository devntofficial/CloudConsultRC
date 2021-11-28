namespace CloudConsult.Diagnosis.Api
{
    public static class Routes
    {
        private const string Root = "api/v{version:apiVersion}/diagnosis";

        public static class Report
        {
            public const string Upload = Root + "/{ConsultationId}/report";
            public const string GetById = Root + "/report/{ReportId}";
            public const string GetByConsultationId = Root + "/{ConsultationId}/report";
        }
    }
}