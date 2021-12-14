namespace CloudConsult.Payment.Api;

public static class Routes
{
    private const string Root = "api/v{version:apiVersion}/payments";

    public static class Payment
    {
        public const string Accept = Root + "/{ConsultationId}/accept";
        public const string Reject = Root + "/{ConsultationId}/reject";
    }
}