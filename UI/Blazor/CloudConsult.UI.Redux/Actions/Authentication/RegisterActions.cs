using CloudConsult.UI.Data.Authentication;

namespace CloudConsult.UI.Redux.Actions.Authentication;

public class RegisterAction
{
    public RegisterData Data { get; }
    public RegisterAction(RegisterData Data)
    {
        this.Data = Data;
    }
}

public class RegisterSuccessAction
{
    public string IdentityId { get; }
    public RegisterData Data { get; }
    public RegisterSuccessAction(string IdentityId, RegisterData Data)
    {
        this.IdentityId = IdentityId;
        this.Data = Data;
    }
}

public class GetRolesAction { }

public class GetRolesSuccessAction
{
    public List<RoleData> Roles { get; }
    public GetRolesSuccessAction(List<RoleData> Roles)
    {
        this.Roles = Roles;
    }
}