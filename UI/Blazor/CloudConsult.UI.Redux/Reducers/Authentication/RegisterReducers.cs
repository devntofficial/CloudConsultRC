using CloudConsult.UI.Redux.Actions.Authentication;
using CloudConsult.UI.Redux.Actions.Shared;
using CloudConsult.UI.Redux.States.Authentication;
using Fluxor;

namespace CloudConsult.UI.Redux.Reducers.Authentication
{
    public static class RegisterReducers
    {
        [ReducerMethod(typeof(RegisterAction))]
        public static RegisterState OnRegisterAction(RegisterState state)
        {
            return state with
            {
                Processing = true
            };
        }

        [ReducerMethod]
        public static RegisterState OnRegisterSuccessAction(RegisterState state, RegisterSuccessAction action)
        {
            return state with
            {
                Processing = false,
                IdentityId = action.IdentityId
            };
        }

        [ReducerMethod(typeof(GetRolesAction))]
        public static RegisterState OnGetRolesAction(RegisterState state)
        {
            return state with
            {
                Loading = true
            };
        }

        [ReducerMethod]
        public static RegisterState OnGetRolesSuccessAction(RegisterState state, GetRolesSuccessAction action)
        {
            return state with
            {
                Loading = false,
                Roles = action.Roles
            };
        }

        [ReducerMethod]
        public static RegisterState OnGatewayErrorAction(RegisterState state, GatewayErrorAction action)
        {
            return state with
            {
                Processing = false,
                Loading = false,
                Errors = action.Errors
            };
        }
    }
}
