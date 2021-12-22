namespace CloudConsult.UI.Redux.Actions.Authentication
{
    public class OtpGenerationAction
    {
        public string IdentityId { get; }
        public OtpGenerationAction(string IdentityId)
        {
            this.IdentityId = IdentityId;
        }
    }

    public class OtpGenerationSuccessAction { }

    public class OtpVerificationAction
    {
        public string IdentityId { get; }
        public int Otp { get; }
        public OtpVerificationAction(string IdentityId, int Otp)
        {
            this.IdentityId = IdentityId;
            this.Otp = Otp;
        }
    }

    public class OtpVerificationSuccessAction { }
}
