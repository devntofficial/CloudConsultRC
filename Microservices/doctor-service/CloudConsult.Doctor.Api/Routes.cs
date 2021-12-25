namespace CloudConsult.Doctor.Api;

public static class Routes
{
    private const string Root = "api/v{version:apiVersion}/doctors";

    public static class Profile
    {
        public const string Create = Root;
        public const string Update = Root + "/{ProfileId}";
        public const string GetAll = Root;
        public const string GetById = Root + "/{ProfileId}";
        public const string GetByIdentityId = Root + "/identity/{IdentityId}";
    }

    public static class Kyc
    {
        public const string GetPending = Root + "/kyc/pending";
        public const string GetMetadata = Root + "/{ProfileId}/kyc/metadata";
        public const string Upload = Root + "/{ProfileId}/kyc/upload";
        public const string DownloadAll = Root + "/{ProfileId}/kyc/download";
        public const string DownloadOne = Root + "/{ProfileId}/kyc/download/{FileName}";
        public const string Approve = Root + "/{ProfileId}/kyc/approve";
        public const string Reject = Root + "/{ProfileId}/kyc/reject";
    }
}