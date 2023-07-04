using AutoMapper;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;

        public UsersController(DataContext context, IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await this.userRepository.GetAllUsersAsync();

            var userToReturn = this.mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(userToReturn);
        }

        
        [HttpGet("{name}", Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string name)
        {
            var user = await this.userRepository.GetUserByNameAsync(name);
            return Ok(this.mapper.Map<MemberDto>(user));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst("UserName")?.Value;
            var user = await this.userRepository.GetUserByNameAsync(username);
            if (user != null)
            {
                this.mapper.Map(memberUpdateDto, user);
                this.userRepository.update(user);

                if (await this.userRepository.SaveAllAsync()) return NoContent();
            }
            

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]

        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var username = User.FindFirst("UserName")?.Value;

            var user = await this.userRepository.GetUserByNameAsync(username);

            var result = await this.photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await this.userRepository.SaveAllAsync())
            {
                //return this.mapper.Map<PhotoDto>(photo); 
                return CreatedAtRoute("GetUser", new {username = user.Username} ,this.mapper.Map<PhotoDto>(photo));
            }
                

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]

        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var username = User.FindFirst("UserName")?.Value;

            var user = await this.userRepository.GetUserByNameAsync(username);

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;

            if (await this.userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]

        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var username = User.FindFirst("UserName")?.Value;

            var user = await this.userRepository.GetUserByNameAsync(username);

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if(photo.PublicId != null)
            {
                await this.photoService.DeletePhotoAsync(photo.PublicId);
            }

            user.Photos.Remove(photo);

            await this.userRepository.SaveAllAsync();

            return Ok();
        }
    }
}
