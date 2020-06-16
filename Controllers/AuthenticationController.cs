using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using shop_giay_server._Controllers;
using shop_giay_server.data.Authentication;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace shop_giay_server.Controllers
{
    [Route("api/client/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _repository;
        private readonly IConfiguration _config;
        public AuthenticationController(IAuthenticationRepository repository, IConfiguration config)
        {
            _config = config;
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Login(CustomerLoginDto dto)
        {
            var customer = await _repository.Login(dto.Username, dto.Password);

            if (customer == null) return BadRequest(ResponseDTO.Unauthorized());

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, customer.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var data = "AuthorizedToken: " + tokenString;

            return Ok(new ResponseDTO(data, "200", "Login successfully"));
        }
    }

    public class CustomerLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}