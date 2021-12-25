using CloudConsult.UI.Redux.Actions.Doctor;
using CloudConsult.UI.Redux.Actions.Shared;
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

        [ReducerMethod(typeof(GetKycMetadataAction))]
        public static KycState OnGetKycMetadataAction(KycState state)
        {
            return state with
            {
                Loading = true
            };
        }

        [ReducerMethod]
        public static KycState OnGetKycMetadataSuccessAction(KycState state, GetKycMetadataSuccessAction action)
        {
            return state with
            {
                Loading = false,
                KycDocumentsMetadata = action.KycDocumentsMetadata
            };
        }

        [ReducerMethod(typeof(GetKycMetadataNotFoundAction))]
        public static KycState OnGetKycMetadataNotFoundAction(KycState state)
        {
            return state with
            {
                Loading = false
            };
        }

        [ReducerMethod(typeof(KycDownloadAllAction))]
        public static KycState OnKycDownloadAllAction(KycState state)
        {
            return state with
            {
                Downloading = true
            };
        }

        [ReducerMethod(typeof(KycDownloadOneAction))]
        public static KycState OnKycDownloadOneAction(KycState state)
        {
            return state with
            {
                Downloading = true
            };
        }

        [ReducerMethod(typeof(KycDownloadSuccessAction))]
        public static KycState OnKycDownloadSuccessAction(KycState state)
        {
            return state with
            {
                Downloading = false
            };
        }

        [ReducerMethod]
        public static KycState OnGatewayErrorAction(KycState state, GatewayErrorAction action)
        {
            return state with
            {
                Processing = false,
                Loading = false,
                Downloading = false,
                Errors = action.Errors
            };
        }
    }
}
