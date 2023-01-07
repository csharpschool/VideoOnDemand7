namespace VOD.UI.Authentication
{
    public class JwtParser
    {
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }

            return Convert.FromBase64String(base64);
        }

        private static void ExtractRolesFormJWT(List<Claim> claims, Dictionary<string, object> keyValuePairs)
        {
            keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);
            if (roles is null) return;

            var parsedRoles = roles.ToString().Trim().TrimStart('[').TrimEnd(']').Split(',');
            
            if(parsedRoles.Length == 0) claims.Add(new Claim(ClaimTypes.Role, parsedRoles[0])); ;

            foreach (var parsedRole in parsedRoles)
                claims.Add(new Claim(ClaimTypes.Role, parsedRole.Trim('"')));

            keyValuePairs.Remove(ClaimTypes.Role);
        }

        public static IEnumerable<Claim> ParseClaimsFromJWT(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            ExtractRolesFormJWT(claims, keyValuePairs);
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }
    }
}
