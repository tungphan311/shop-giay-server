using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using shop_giay_server._Controllers;
using shop_giay_server.data;
using shop_giay_server.Dtos;
using shop_giay_server.models;

namespace shop_giay_server.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repo;
        private readonly IConfiguration config;
        private readonly DataContext context;
        private readonly IMapper mapper;
        public AuthController(IAuthRepository repo, IConfiguration config, DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
            this.config = config;
            this.repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            if (await repo.UserExists(userForRegisterDto.Username))
            {
                return BadRequest(ResponseDTO.BadRequest("Tên đăng nhập này đã tồn tại."));
            }

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username,
                RoleId = userForRegisterDto.RoleId,
                Name = userForRegisterDto.Name,
                Email = userForRegisterDto.Email,
                PhoneNumber = userForRegisterDto.PhoneNumber
            };


            var createdUser = await repo.Register(userToCreate, userForRegisterDto.Password);

            return Ok(new ResponseDTO(null, "200", "Thêm tài khoản thành công", 1));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await repo.Login(userForLoginDto.Username, userForLoginDto.Password);

            if (userFromRepo == null)
            {
                return Unauthorized();
            }

            var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == userFromRepo.RoleId);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username),
                new Claim(ClaimTypes.Role, role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }


        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var users = await repo.GetUser();
            var items = mapper.Map<List<ResponseUserDto>>(users);

            return Ok(ResponseDTO.Ok(items, items.Count()));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var users = await repo.GetUserById(id);

            if (users == null)
            {
                return BadRequest(ResponseDTO.NotFound());
            }

            var items = mapper.Map<ResponseUserDto>(users);

            return Ok(ResponseDTO.Ok(items));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateDto dto)
        {
            var user = await repo.GetUserById(id);

            if (user == null)
            {
                return BadRequest(ResponseDTO.NotFound());
            }

            user.Username = dto.Username;
            user.Email = dto.Email;
            user.RoleId = dto.RoleId;
            user.Name = dto.Name;
            user.PhoneNumber = dto.PhoneNumber;

            await repo.Update(user);
            return Ok(ResponseDTO.Ok(user));
        }
    }

    public class ResponseUserDto : BaseDTO
    {
        public string Username { get; set; }

        public int RoleId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }

    public class UpdateDto : BaseDTO
    {
        public string Username { get; set; }

        public int RoleId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}