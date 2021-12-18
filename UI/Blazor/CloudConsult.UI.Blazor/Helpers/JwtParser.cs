using System.Security.Claims;
using System.Text.Json;

namespace CloudConsult.UI.Blazor.Helpers
{
    public class JwtParser
    {
        public static IEnumerable<Claim> ParseClaimsFromToken(string token)
        {
            var claims = new List<Claim>();
            var payload = token.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValues = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValues == null)
            {
                return claims;
            }

            ExtractRolesFromToken(claims, keyValues);
            claims.AddRange(keyValues.Select(x => new Claim(x.Key, x.Value.ToString() ?? string.Empty)));

            return claims;
        }

        private static void ExtractRolesFromToken(ICollection<Claim> claims, IDictionary<string, object> keyValues)
        {
            keyValues.TryGetValue("Roles", out var roles);

            if (roles is null)
            {
                return;
            }

            var parsedRoles = roles.ToString()?.TrimStart('[').TrimEnd(']').Split(',');

            if (parsedRoles is { Length: > 0 })
            {
                foreach (var role in parsedRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Trim('"')));
                }
            }

            keyValues.Remove("Roles");
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
