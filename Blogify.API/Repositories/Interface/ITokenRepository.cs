using Microsoft.AspNetCore.Identity;

namespace Blogify.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
