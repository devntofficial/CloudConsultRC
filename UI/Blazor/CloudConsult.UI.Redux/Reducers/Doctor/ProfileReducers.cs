using CloudConsult.UI.Redux.Actions.Doctor;
using CloudConsult.UI.Redux.Actions.Shared;
using CloudConsult.UI.Redux.States.Doctor;
using Fluxor;

namespace CloudConsult.UI.Redux.Reducers.Doctor
{
    public static class ProfileReducers
    {
        [ReducerMethod(typeof(CreateProfileAction))]
        public static ProfileState OnCreateProfileAction(ProfileState state)
        {
            return state with
            {
                Processing = true
            };
        }

        [ReducerMethod]
        public static ProfileState OnCreateProfileSuccessAction(ProfileState state, CreateProfileSuccessAction action)
        {
            return state with
            {
                Processing = false,
                ProfileId = action.ProfileId
            };
        }

        [ReducerMethod(typeof(GetProfileAction))]
        public static ProfileState OnGetProfileAction(ProfileState state)
        {
            return state with
            {
                Processing = true
            };
        }

        [ReducerMethod]
        public static ProfileState OnGetProfileSuccessAction(ProfileState state, GetProfileSuccessAction action)
        {
            return state with
            {
                Processing = false,
                Data = action.Data
            };
        }

        [ReducerMethod]
        public static ProfileState OnGatewayErrorAction(ProfileState state, GatewayErrorAction action)
        {
            return state with
            {
                Processing = false,
                Errors = action.Errors
            };
        }

        [ReducerMethod(typeof(UpdateProfileAction))]
        public static ProfileState OnUpdateProfileAction(ProfileState state)
        {
            return state with
            {
                Processing = true
            };
        }

        [ReducerMethod]
        public static ProfileState OnUpdateProfileSuccessAction(ProfileState state, UpdateProfileSuccessAction action)
        {
            return state with
            {
                Processing = false
            };
        }
    }
}
