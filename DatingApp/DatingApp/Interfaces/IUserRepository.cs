using DatingApp.Entities;

namespace DatingApp.Interfaces
{
    public interface IUserRepository
    {
        void update(AppUser user);
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByNameAsync(string name);
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<bool> SaveAllAsync(); 
    }
}
