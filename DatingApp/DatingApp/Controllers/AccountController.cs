using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            this.context = context;
            this.tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(AppUser user)
        {
            user.PasswordHash = BC.HashPassword(user.PasswordHash);
            this.context.Add(user);
            await this.context.SaveChangesAsync();
            return new UserDto
            {
                Username = user.Username,
                Token = this.tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(AppUser Appuser)
        {
                  
               
            try
            {
                var userinfo = await this.context.AppUsers
                    .Include(p => p.Photos)
                    .SingleOrDefaultAsync(x => x.Username == Appuser.Username);
                bool isValidPassword = BC.Verify(Appuser.PasswordHash, userinfo.PasswordHash);
                if (userinfo != null && isValidPassword)
                {
                    return new UserDto
                    {
                        Username = Appuser.Username,
                        Token = this.tokenService.CreateToken(userinfo),
                        
                    };  
                }
            }
            catch(Exception e)
            {
             
                 return BadRequest("Invalid Username Or Password");
            }
              
                  return BadRequest("Invalid Username Or Password");


        }


    }
}
