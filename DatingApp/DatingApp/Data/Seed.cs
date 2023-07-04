using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BC = BCrypt.Net.BCrypt;

namespace DatingApp.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.AppUsers.AnyAsync()) return;
            
                var userData = await File.ReadAllTextAsync("Data/UserData.json");
                var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
                foreach(var user in users)
                {
                    using var hmac = new HMACSHA512();

                    user.Username = user.Username.ToLower();
                    user.PasswordHash = BC.HashPassword(user.PasswordHash);

                context.AppUsers.Add(user);
                }

            await context.SaveChangesAsync();
            
        }
    }
}
