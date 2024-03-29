﻿using CloudConsult.UI.Data.Doctor;

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
        public ProfileResponseData Data { get; set; }
        public string ProfileId { get; }

        public GetProfileByIdentitySuccessAction(string ProfileId, ProfileResponseData Data)
        {
            this.Data = Data;
            this.ProfileId = ProfileId;
        }
    }
}
