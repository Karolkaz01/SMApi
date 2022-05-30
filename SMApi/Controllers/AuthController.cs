using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SMApi.Models;
using SMApi.ModelsDto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SMApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly UserServices _services;

        public AuthController(IConfiguration configuration, UserServices userServices)
        {
            _configuration = configuration;
            _services = userServices;
        }
        
        //Get name and account type of current user

        [HttpGet]
        [Route("MyAccount")]
        [Authorize]
        public ActionResult<object> GetMe()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var userAccountType = User.FindFirstValue(ClaimTypes.Role);
            return Ok( new { userName, userAccountType });
        }

        //Create user as employee

        [HttpPost]
        [Route("Registration")]
        public ActionResult<Object> Registration(UserDto userDto)
        {
            if(_services.GetByName(userDto.Name) != null)
                return BadRequest("User with that name already exist");

            CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = _services.Insert(userDto.Name, Role.Employee, passwordHash, passwordSalt);
            return Ok(user);
        }

        //Login to exist account

        [HttpPost]
        [Route("Login")]
        public ActionResult<User> Login(UserDto userDto)
        {
            var user = _services.GetByName(userDto.Name);
            if(user == null)
                return BadRequest("User not Found");

            if (!VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Wrong password");

            string token = CreateToken(user);
            return Ok(token);
        }

        private void CreatePasswordHash(string password , out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.AccountType.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            
            return jwt;
        }

    }
}
