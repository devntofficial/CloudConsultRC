using CloudConsult.UI.Redux.Actions.Authentication;
using CloudConsult.UI.Redux.States.Authentication;
using Fluxor;

namespace CloudConsult.UI.Redux.Reducers.Authentication
{
    public static class OtpReducers
    {
        [ReducerMethod]
        public static OtpState OnOtpGenerationAction(OtpState state, OtpGenerationAction action)
        {
            return state with
            {
                Loading = true
            };
        }

        [ReducerMethod(typeof(OtpGenerationSuccessAction))]
        public static OtpState OnOtpGenerationSuccessAction(OtpState state)
        {
            return state with
            {
                Loading = false
            };
        }

        [ReducerMethod]
        public static OtpState OnOtpVerificationAction(OtpState state, OtpVerificationAction action)
        {
            return state with
            {
                Processing = true
            };
        }

        [ReducerMethod(typeof(OtpVerificationSuccessAction))]
        public static OtpState OnOtpVerificationSuccessAction(OtpState state)
        {
            return state with
            {
                Processing = false
            };
        }
    }
}
