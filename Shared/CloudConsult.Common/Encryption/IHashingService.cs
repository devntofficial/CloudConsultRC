namespace CloudConsult.Common.Encryption
{
    public interface IHashingService
    {
        string GenerateRandomSalt();
        string GenerateHash(string value);
        string GenerateHashWithSalt(string value, string salt);
        bool VerifyHash(string value, string hash);
        bool VerifyHashWithSalt(string value, string hash, string salt);
    }
}