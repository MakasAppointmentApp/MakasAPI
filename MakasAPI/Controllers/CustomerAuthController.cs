using MakasAPI.Data.AuthRepositories;
using MakasAPI.Dtos.DtosForAuth;
using MakasAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MakasAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/CustomerAuth")]
    public class CustomerAuthController : Controller
    {
        private ICustomerAuthRepository _authRepositoy;
        private IConfiguration _configuration;

        public CustomerAuthController(ICustomerAuthRepository authRepositoy, IConfiguration configuration)
        {
            _authRepositoy = authRepositoy;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] CustomerForRegisterDto userForRegisterDto)
        {
            if (await _authRepositoy.UserExist(userForRegisterDto.CustomerEmail))
            {
                ModelState.AddModelError("Email", "Email has already exist");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userToCreate = new Customer
            {
                CustomerEmail = userForRegisterDto.CustomerEmail,
                CustomerName = userForRegisterDto.CustomerName,
                CustomerSurname = userForRegisterDto.CustomerSurname,
                
            };
            var createdUser = await _authRepositoy.Register(userToCreate, userForRegisterDto.CustomerPassword);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] CustomerForLoginDto userForLoginDto)
        {
            var user = await _authRepositoy.Login(userForLoginDto.CustomerEmail, userForLoginDto.CustomerPassword);

            if (user == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.CustomerName+" "+user.CustomerSurname)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(tokenString);
        }
        [HttpGet]
        [Route("detail")]
        public ActionResult GetCustomerById(int id)
        {
            var customer = _authRepositoy.GetCustomerById(id);
            return Ok(customer);
        }
    }
}
