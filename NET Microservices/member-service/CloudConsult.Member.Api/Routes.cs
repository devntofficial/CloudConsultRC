namespace CloudConsult.Member.Api
{
    public static class Routes
    {
        private const string Root = "api/v{version:apiVersion}/members";

        public static class Profile
        {
            public const string Create = Root;
            public const string Update = Root + "/{ProfileId}";
            public const string GetById = Root + "/{ProfileId}";
            public const string GetByIdentityId = Root + "/identity/{IdentityId}";
        }
    }
}
