using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using shop_giay_server.Controllers;
using shop_giay_server.data;
using System;
using shop_giay_server;
using System.Collections.Generic;

namespace shop_giay_server._Controllers
{
    public class CustomerController : GeneralController<Customer, CustomerDTO>
    {
        private readonly IAuthRepository _authRepo;

        public CustomerController(IAsyncRepository<Customer> repo, ILogger<CustomerController> logger, IMapper mapper, IAuthRepository authRepo)
            : base(repo, logger, mapper)
        { 
            _authRepo = authRepo;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CustomerDTO model)
        {
            // Validate
            var existed = await _repository.ExistWhere(c => c.Email == model.Email
                    || c.PhoneNumber == model.PhoneNumber
                    || c.Username == model.Username);

            if (existed)
            {
                return BadRequest(ResponseDTO.BadRequest("Customer has been existed."));
            }


            var item = _mapper.Map<Customer>(model);
            return await this._AddItem(item);
        }

        [HttpPost]
        [Route("client/customer")]
        public async Task<IActionResult> ClientAddCustomer([FromBody] BodyCreateCustomer model) 
        {
            // Validate
            var existed = await _repository.ExistWhere(c => c.Email == model.Email
                    || c.PhoneNumber == model.PhoneNumber
                    || c.Username == model.Username);

            if (existed)
            {
                return BadRequest(ResponseDTO.BadRequest("Khách hàng đã tồn tại."));
            }

            // Create user
            if (await _authRepo.UserExists(model.Username)) 
            {
                return Ok(ResponseDTO.BadRequest("Tên đăng nhập đã tồn tại."));
            }
            var user = new User {
                Username = model.Username,
                RoleId = 3 // todo: hardcode client role id 
            };
            var createdUser = await _authRepo.Register(user, model.Password);
            
            var customer = new Customer {
                DeleteFlag = false,
                Username = createdUser.Username,
                PasswordHash = createdUser.PasswordHash,
                PasswordSalt = createdUser.PasswordSalt,
                Name = model.Name,
                Gender = model.Gender,
                Point = model.Point,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                DateOfBirth = new DateTime().FromEpochSeconds(model.DateOfBirth),
                CustomerTypeId = model.CustomerTypeId
            };
            
            // Create customer 
            var result = await _repository.Add(customer);
            return Ok(ResponseDTO.Ok(result));
        }

        [HttpPost]
        [Route("client/customer/getInfo")]
        public async Task<IActionResult> ClientGetCustomerInfo() 
        {
            var sessionUsername = HttpContext.Session.GetString(SessionConstant.Username);
            if (string.IsNullOrEmpty(sessionUsername)) {
                return Ok(ResponseDTO.BadRequest());
            }

            var customer = await _repository.FirstOrDefault(c => c.Username == sessionUsername);
            if (customer == null) {
                return Ok(ResponseDTO.NotFound());
            }

            return Ok(ResponseDTO.Ok(new {
                Id = customer.Id,
                customer.Name, 
                DateOfBirth = customer.DateOfBirth.GetEpochSeconds(),
                Email = customer.Email, 
                PhoneNumber = customer.PhoneNumber,
                Gender = customer.Gender
            }));    
        }

        [HttpGet]
        [Route("client/customer/getAddresses")]
        public async Task<IActionResult> ClientGetCustomerAddress() 
        {
            var sessionUsername = HttpContext.Session.GetString(SessionConstant.Username);
            if (string.IsNullOrEmpty(sessionUsername)) {
                return Ok(ResponseDTO.BadRequest());
            }

            var customer = await _repository.FirstOrDefault(c => c.Username == sessionUsername);
            if (customer == null) {
                return Ok(ResponseDTO.NotFound());
            }

            var listResult = new List<dynamic>();
            foreach (var address in customer.Addresses) 
            {
                listResult.Add(new {
                    Id = address.Id,
                    CustomerId = customer.Id,
                    City = address.City,
                    address.District, 
                    address.Ward, 
                    address.Street
                });
            }
            return Ok(ResponseDTO.Ok(listResult));
        }

        // Customer Update
        [Route("admin/[controller]/{id:int}")]
        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDTO dto)
        {
            if (id == dto.Id) 
            {
                return BadRequest(ResponseDTO.BadRequest("URL ID and Item ID does not matched."));
            }
            var entity = _mapper.Map<Customer>(dto);
            entity.Id = id;

            var updatedItem = await _repository.Update(entity);
            if (updatedItem == null) 
            {
                return BadRequest(ResponseDTO.BadRequest("Item ID is not existed."));
            }
            return Ok(ResponseDTO.Ok(entity));
        }
        // address update
    }


    public class BodyCreateCustomer 
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Gender { get; set; }
        public float Point { get; set; }
        public long DateOfBirth { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int CustomerTypeId { get; set; }
    }
}
