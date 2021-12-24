using CloudConsult.UI.Redux.Actions.Doctor;
using CloudConsult.UI.Redux.States.Doctor;
using Fluxor;

namespace CloudConsult.UI.Redux.Reducers.Doctor
{
    public static class KycReducers
    {
        [ReducerMethod(typeof(KycUploadAction))]
        public static KycState OnKycUploadAction(KycState state)
        {
            return state with
            {
                Processing = true
            };
        }

        [ReducerMethod(typeof(KycUploadSuccessAction))]
        public static KycState OnKycUploadSuccessAction(KycState state)
        {
            return state with
            {
                Processing = false
            };
        }
    }
}
