using CloudConsult.UI.Redux.Actions.Authentication;
using CloudConsult.UI.Redux.Actions.Shared;
using CloudConsult.UI.Redux.States.Authentication;
using Fluxor;

namespace CloudConsult.UI.Redux.Reducers.Authentication
{
    public static class LoginReducers
    {
        [ReducerMethod(typeof(LoginAction))]
        public static LoginState OnLoginAction(LoginState state)
        {
            return state with
            {
                Processing = true
            };
        }

        [ReducerMethod]
        public static LoginState OnLoginSuccessAction(LoginState state, LoginSuccessAction action)
        {
            return state with
            {
                Processing = false,
                IdentityId = action.IdentityId
            };
        }

        [ReducerMethod]
        public static LoginState OnLoginUnverifiedAction(LoginState state, LoginUnverifiedAction action)
        {
            return state with
            {
                Processing = false,
                IdentityId = action.IdentityId
            };
        }

        [ReducerMethod]
        public static LoginState OnGatewayErrorAction(LoginState state, GatewayErrorAction action)
        {
            return state with
            {
                Processing = false,
                Errors = action.Errors
            };
        }
    }
}
