using CloudConsult.UI.Data.Authentication;

namespace CloudConsult.UI.Redux.Actions.Authentication
{
    public class LoginAction
    {
        public LoginData Data { get; }

        public LoginAction(LoginData Data)
        {
            this.Data = Data;
        }
    }

    public class LoginSuccessAction
    {
        public string IdentityId { get; }
        public string Role { get; set; }

        public LoginSuccessAction(string IdentityId, string Role)
        {
            this.IdentityId = IdentityId;
            this.Role = Role;
        }
    }

    public class LoginUnverifiedAction
    {
        public string IdentityId { get; }

        public LoginUnverifiedAction(string IdentityId)
        {
            this.IdentityId = IdentityId;
        }
    }
}
