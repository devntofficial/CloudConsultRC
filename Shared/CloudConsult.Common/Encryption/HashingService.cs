using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;
using System.Security.Cryptography;
using System.Text;

namespace CloudConsult.Common.Encryption;

public class HashingService : IHashingService
{
    private readonly RandomNumberGenerator _random;

    public HashingService()
    {
        this._random = RandomNumberGenerator.Create();
    }

    public string GenerateRandomSalt()
    {
        var salt = new byte[16];
        _random.GetBytes(salt);
        return Convert.ToBase64String(salt);
    }

    public string GenerateHash(string value)
    {
        var hashedValue = Argon2.Hash(value);
        return hashedValue;
    }

    public string GenerateHashWithSalt(string value, string salt)
    {
        var valueBytes = Encoding.UTF8.GetBytes(value);
        var saltBytes = Encoding.UTF8.GetBytes(salt);


        var config = new Argon2Config
        {
            Password = valueBytes,
            Salt = saltBytes,
            Type = Argon2Type.HybridAddressing,
            Version = Argon2Version.Nineteen,
            TimeCost = 3,
            MemoryCost = 65536,
            Lanes = 4,
            Threads = 1, // use Environment.ProcessorCount in production
                         //config.Secret = secret; // additional security using secret key
            HashLength = 32
        };


        var hashFunction = new Argon2(config);

        using var hash = hashFunction.Hash();
        var hashedValue = config.EncodeString(hash.Buffer);

        return hashedValue;
    }

    public bool VerifyHash(string value, string hash)
    {
        return Argon2.Verify(hash, value);
    }

    public bool VerifyHashWithSalt(string value, string hash, string salt)
    {
        var valueBytes = Encoding.UTF8.GetBytes(value);
        var saltBytes = Convert.FromBase64String(salt);
        var config = new Argon2Config { Password = valueBytes, Salt = saltBytes, Threads = 1 };
        SecureArray<byte> hashB = null;
        try
        {
            if (config.DecodeString(hash, out hashB) && hashB != null)
            {
                var argon2ToVerify = new Argon2(config);
                using var hashToVerify = argon2ToVerify.Hash();
                return Argon2.FixedTimeEquals(hashB, hashToVerify);
            }
            else
            {
                return false;
            }
        }
        finally
        {
            hashB?.Dispose();
        }
    }

    public int GenerateRandomOtp(int length)
    {
        var startValue = 10 ^ length;
        var endValue = (10 * length) - 1;
        return RandomNumberGenerator.GetInt32(startValue, endValue);
    }
}