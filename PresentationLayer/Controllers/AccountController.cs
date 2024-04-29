using BusinessAccessLayer.Dto;
using BusinessAccessLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        //SignInManager
        private readonly SignInManager<User> _signInManager;
        //userManager
        private readonly UserManager<User> _userManager;
        //TokenService
        private readonly iTokenService _tokenService;
        
        
        //dependency Injection

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, iTokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        
        //login
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDto login)
        {
            //login
            //1. check if user exists
          
            //if (!UserExists(login.Username)) return BadRequest("Username or password is incorrect");
            
            var user = _userManager.Users.SingleOrDefault(x => x.UserName == login.Username);
            if (user == null) return Unauthorized("Username or password is incorrect");

            
            //2. check if password is correct
            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            //2.a. if password is incorrect return unauthorized
            if (!result.Succeeded) return Unauthorized("Username or password is incorrect");
            //3 . generate token
            var token = await _tokenService.CreateToken(user);
            var userDto = new UserDto()
            {
                Username = user.UserName,
                Token = token

            };



            //4. return token
            return Ok(userDto);
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterDto user)
        {
            //register
            //1. check if user exists
            if(UserExists(user.Username)) return BadRequest("User already exists");
            //2. create user
            var newUser = new User()
            {
                UserName = user.Username,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Username,
                Birthday = user.Birthday

            };

            var result = await _userManager.CreateAsync(newUser, user.Password);

            if(!result.Succeeded) return BadRequest(result.Errors);
            //2.1 Add User to role
            var roleResult = await _userManager.AddToRoleAsync(newUser, "Owner");
            if(!roleResult.Succeeded) return BadRequest(roleResult.Errors);
            
            //3. Generate Token

            var token = await _tokenService.CreateToken(newUser);


            var userDto = new UserDto()
            {
                Username = newUser.UserName,
                Token = token

            };

            //4. return token

            

            return Ok(userDto);
        }

        private bool UserExists(string username)
        {
            //add code to check if user exists

            return _userManager.Users.Any(x => x.UserName.ToLower() == username.ToLower());

        }
    }
}
