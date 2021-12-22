using CloudConsult.UI.Redux.Actions.Doctor;
using CloudConsult.UI.Redux.Actions.Shared;
using CloudConsult.UI.Redux.States.Doctor;
using Fluxor;

namespace CloudConsult.UI.Redux.Reducers.Doctor
{
    public static class DashboardReducers
    {
        [ReducerMethod(typeof(GetProfileByIdentityAction))]
        public static DashboardState OnGetProfileByIdentityAction(DashboardState state)
        {
            return state with
            {
                Processing = true
            };
        }

        [ReducerMethod]
        public static DashboardState OnGetProfileByIdentitySuccessAction(DashboardState state, GetProfileByIdentitySuccessAction action)
        {
            return state with
            {
                Processing = false,
                ProfileId = action.ProfileId
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
    }
}
