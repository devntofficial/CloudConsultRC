using CloudConsult.UI.Data.Doctor;

namespace CloudConsult.UI.Redux.Actions.Doctor
{
    public class GetProfileByIdentityAction
    {
        public string IdentityId { get; set; }
        public GetProfileByIdentityAction(string IdentityId)
        {
            this.IdentityId = IdentityId;
        }
    }

    public class GetProfileByIdentitySuccessAction
    {
        public ProfileData Data { get; set; }
        public string ProfileId { get; }

        public GetProfileByIdentitySuccessAction(string ProfileId, ProfileData Data)
        {
            this.Data = Data;
            this.ProfileId = ProfileId;
        }
    }
}
