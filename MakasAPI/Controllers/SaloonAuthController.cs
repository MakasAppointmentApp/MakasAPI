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
    [Route("api/SaloonAuth")]
    public class SaloonAuthController : Controller
    {
        private ISaloonAuthRepository _authRepository;
        private IConfiguration _configuration;

        public SaloonAuthController(ISaloonAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] SaloonForRegisterDto userForRegisterDto)
        {
            
            if (await _authRepository.UserExist(userForRegisterDto.SaloonPhone, userForRegisterDto.SaloonEmail))
            {
                ModelState.AddModelError("Email", "Email has already exist");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userToCreate = new Saloon
            {
                SaloonPhone = userForRegisterDto.SaloonPhone,
                SaloonEmail = userForRegisterDto.SaloonEmail,
                SaloonName = userForRegisterDto.SaloonName,
                SaloonGender = userForRegisterDto.SaloonGender,
                SaloonCity = userForRegisterDto.SaloonCity,
                SaloonDistrict = userForRegisterDto.SaloonDistrict

            };
            var createdUser = await _authRepository.Register(userToCreate, userForRegisterDto.SaloonPassword);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] SaloonForLoginDto userForLoginDto)
        {
            var user = await _authRepository.Login(userForLoginDto.SaloonPhone, userForLoginDto.SaloonPassword);

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
                    new Claim(ClaimTypes.Name, user.SaloonName)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            //DTO TANIMLA ONU GÖNDER TOKEN VE USERID İÇİN
            return Ok(tokenString);
        }
        [HttpGet]
        [Route("detail")]
        public ActionResult GetSaloonById(int id)
        {
            var saloon = _authRepository.GetSaloonById(id);
            return Ok(saloon);
        }
    }
}
