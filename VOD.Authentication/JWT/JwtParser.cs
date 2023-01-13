namespace VOD.Authentication.JWT;

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

        if (parsedRoles.Length == 0) claims.Add(new Claim(ClaimTypes.Role, parsedRoles[0])); ;

        foreach (var parsedRole in parsedRoles)
            claims.Add(new Claim(ClaimTypes.Role, parsedRole.Trim('"')));

        keyValuePairs.Remove(ClaimTypes.Role);
    }

    public static IEnumerable<Claim> ParseClaimsFromJWT(string jwt)
    {
        var claims = new List<Claim>();
        if (string.IsNullOrWhiteSpace(jwt)) return claims;
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        ExtractRolesFormJWT(claims, keyValuePairs);
        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
        return claims;
    }

    public static TokenUserDTO? ParseUserInfoFromJWT(string jwt)
    {
        try
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            ExtractRolesFormJWT(claims, keyValuePairs);
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            keyValuePairs.TryGetValue("email", out var email);

            var tokeUser = new TokenUserDTO(email?.ToString(), claims);

            return tokeUser;
        }
        catch (Exception ex)
        {
        }

        return null;
    }

    public static bool ParseIsInRoleFromJWT(string jwt, string role)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(jwt)) return false;

            List<Claim>? claims = ParseUserInfoFromJWT(jwt)?.Roles;

            if (claims is null || claims.Count.Equals(0)) return false;

            var isInRole = claims.Exists(c => c.Value.Equals(role));

            return isInRole;
        }
        catch (Exception ex)
        {
        }

        return false;
    }

    public static bool ParseIsNotInRoleFromJWT(string jwt, string role) => !ParseIsInRoleFromJWT(jwt, role);


}
