using System.ComponentModel.DataAnnotations;

namespace VOD.UI.Models;

public class AuthenticatedUserModel
{
    public string AccessToken { get; set; }

    public string UserName { get; set; }
}
