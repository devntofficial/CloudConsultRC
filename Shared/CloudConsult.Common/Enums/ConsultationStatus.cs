using System.ComponentModel;

namespace CloudConsult.Common.Enums
{
    public enum ConsultationEvents
    {
        [Description("ConsultationRequested")]
        ConsultationRequested,
        [Description("ConsultationAccepted")]
        ConsultationAccepted,
        [Description("ConsultationRejected")]
        ConsultationRejected,
        [Description("ConsultationCancelled")]
        ConsultationCancelled,
        [Description("PaymentAccepted")]
        PaymentAccepted,
        [Description("PaymentRejected")]
        PaymentRejected,
        [Description("DiagnosisReportPublished")]
        DiagnosisReportPublished,
        [Description("ProcessComplete")]
        ProcessComplete
    }

    public static class ConsultationStatusExtensions
    {
        public static string ToString(this ConsultationEvents val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
                .GetType()
                .GetField(val.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
