namespace CloudConsult.Member.Api
{
    public static class Routes
    {
        private const string Root = "api/v{version:apiVersion}";

        public static class Member
        {
            public const string CreateMember = Root + "/member";
            public const string GetMemberById = Root + "/member/{MemberId}";
            public const string UpdateMember = Root + "/member/{MemberId}";
        }
    }
}
