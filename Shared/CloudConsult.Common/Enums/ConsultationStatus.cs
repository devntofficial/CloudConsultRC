using System.ComponentModel;

namespace CloudConsult.Common.Enums
{
    public enum ConsultationStatus
    {
        [Description("DoctorApprovalPending")]
        DoctorApprovalPending,
        [Description("PaymentPending")]
        PaymentPending,
        [Description("PaymentRejected")]
        PaymentRejected,
        [Description("Confirmed")]
        Confirmed,
        [Description("DiagnosisReportPending")]
        DiagnosisReportPending,
        [Description("DiagnosisReportPublished")]
        DiagnosisReportPublished,
        [Description("Complete")]
        Complete
    }

    public static class ConsultationStatusExtensions
    {
        public static string ToString(this ConsultationStatus val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
                .GetType()
                .GetField(val.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
