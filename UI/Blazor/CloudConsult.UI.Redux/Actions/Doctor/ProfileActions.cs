using CloudConsult.UI.Data.Doctor;

namespace CloudConsult.UI.Redux.Actions.Doctor
{
    public class CreateProfileAction
    {
        public ProfileData Data { get; set; }
        public CreateProfileAction(ProfileData Data)
        {
            this.Data = Data;
        }
    }

    public class CreateProfileSuccessAction
    {
        public string ProfileId { get; set; }
        public CreateProfileSuccessAction(string ProfileId)
        {
            this.ProfileId = ProfileId;
        }
    }

    public class GetProfileAction
    {
        public string ProfileId { get; set; }
        public GetProfileAction(string ProfileId)
        {
            this.ProfileId = ProfileId;
        }
    }

    public class GetProfileSuccessAction
    {
        public ProfileData Data { get; set; }
        public string ProfileId { get; }

        public GetProfileSuccessAction(string ProfileId, ProfileData Data)
        {
            this.Data = Data;
            this.ProfileId = ProfileId;
        }
    }

    public class UpdateProfileAction
    {
        public string ProfileId { get; set; }
        public ProfileData Data { get; set; }
        public UpdateProfileAction(string ProfileId, ProfileData Data)
        {
            this.ProfileId = ProfileId;
            this.Data = Data;
        }
    }

    public class UpdateProfileSuccessAction {}
}
