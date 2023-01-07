namespace VOD.Token.Common.DTOs;

public class TokenDTO
{
    public string Token { get; set; } = "";
    public DateTime TokenExpires { get; set; } = default;
    public bool TokenHasExpired => TokenExpires == default ? true : !(TokenExpires.Subtract(DateTime.UtcNow).Minutes > 0);
        
    public TokenDTO(string token, DateTime expires)
    {
        Token = token;
        TokenExpires = expires;
    }

    public TokenDTO()
    {
    }
}
