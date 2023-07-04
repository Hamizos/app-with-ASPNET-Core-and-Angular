using DatingApp.Data;
using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Interfaces
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;

        public UserRepository(DataContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await context.AppUsers
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await context.AppUsers
                .Include (p => p.Photos)
                .FirstOrDefaultAsync(x => x.Id == id);  
        }

        public async Task<AppUser> GetUserByNameAsync(string name)
        {
            return await context.AppUsers
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.Username == name);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void update(AppUser user)
        {
            context.Entry(user).State = EntityState.Modified;
        }
    }
}
