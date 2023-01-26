
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController: BaseApiController
    {
        private readonly DataContext _context;
        private readonly ItokenService _tokenService;
        public AccountController(DataContext context, ItokenService tokenService){
            _tokenService = tokenService;
            _context = context;

        }
       [HttpPost("register")]
       public async Task<ActionResult<UserDto>> Register(RegisterDto register){


        if(await UserExists(register.username)) return BadRequest("Username is taken");

        using var hmac = new HMACSHA512();
        var user = new AppUser{
            UserName= register.username.ToLower(),
            PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(register.password)),
            PasswordSalt=hmac.Key
        };

        _context.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto{
            Username=user.UserName,
            Token=_tokenService.CreateToken(user)

        };
       }

      [HttpPost("login")]
      public async Task<ActionResult<UserDto>> Login(LoginDto login)
      {
        var user = await _context.Users.SingleOrDefaultAsync(x=> x.UserName == login.username);
        if(user== null) return Unauthorized("invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var ComputeHash =hmac.ComputeHash(Encoding.UTF8.GetBytes(login.password));

        for(int i=0;i<ComputeHash.Length;i++){
            if(ComputeHash[i]!=user.PasswordHash[i]) return Unauthorized("invalid password");

        }
        return new UserDto{
            Username=user.UserName,
            Token=_tokenService.CreateToken(user)

        };

      }

       private async Task<bool> UserExists(string username){

        return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
       }
    }
}