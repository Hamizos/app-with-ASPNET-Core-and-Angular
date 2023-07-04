using DatingApp.Entities;

namespace DatingApp.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
