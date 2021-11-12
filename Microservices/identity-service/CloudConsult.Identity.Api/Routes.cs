namespace CloudConsult.Identity.Api
{
    public static class Routes
    {
        private const string Root = "api/v{version:apiVersion}";

        public static class Identity
        {
            public const string GetToken = Root + "/token/generate";
            public const string CreateUser = Root + "/user";
        }
    }
}